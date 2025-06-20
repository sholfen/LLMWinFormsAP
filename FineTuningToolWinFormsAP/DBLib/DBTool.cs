using Dapper;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace FineTuningToolWinFormsAP.DBLib
{
    public class DBTool
    {
        //protected SqlConnection _sqlConnection { get; set; }
        protected MySqlConnection _sqlConnection { get; set; }

        public DBTool() 
        {
            StreamReader sr = new StreamReader(@"Config.json");
            string jsonStr = sr.ReadToEnd();
            dynamic jsonModel = JsonSerializer.Deserialize<ExpandoObject>(jsonStr);
            string connectionObj = jsonModel.ConnectionString.ToString();
            dynamic connectionModel = JsonSerializer.Deserialize<ExpandoObject>(connectionObj);
            //MySqlConnection mySqlConnection = new MySqlConnection(connectionModel.Local.ToString());
            _sqlConnection = new MySqlConnection(connectionModel.Local.ToString());
        }

        public async Task InserData(FineTuningBaseClassList fineTuningBaseClassList)
        {
            foreach (var item in fineTuningBaseClassList.messages)
            {
                string query =
                    $"INSERT INTO demo.FineTuningData (role,content) VALUES (@role,@content)";
                 await _sqlConnection.ExecuteAsync(query, item);
            }
        }

        public async Task<List<FineTuningBaseClassList>> Query()
        {       
            List<FineTuningBaseClassList> finalList = new List<FineTuningBaseClassList>();
            string sql = "SELECT * FROM demo.FineTuningData";
            var list = await _sqlConnection.QueryAsync<FineTuningBaseClass>(sql);
            int count = 0;
            FineTuningBaseClassList jsonLineResult = new FineTuningBaseClassList
            {
                messages = new List<FineTuningBaseClass>()
            };
            foreach (var item in list)
            {
                if (count >= 2)
                {
                    jsonLineResult.messages.Add(item);
                    jsonLineResult = new FineTuningBaseClassList
                    {
                        messages = new List<FineTuningBaseClass>()
                    };
                    count = 0;
                    continue;
                }
                jsonLineResult.messages.Add(item);
                count++;
            }
            return finalList;
        }
    }
}
