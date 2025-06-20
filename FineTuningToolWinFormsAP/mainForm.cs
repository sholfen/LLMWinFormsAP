using System.Text.Json;
using System.Threading.Tasks;

namespace FineTuningToolWinFormsAP
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        private async void btnInsert_Click(object sender, EventArgs e)
        {
            DBLib.DBTool dbTool = new DBLib.DBTool();
            await dbTool.InserData(new FineTuningBaseClassList
            {
                messages = new List<FineTuningBaseClass>
                {
                    new FineTuningBaseClass { role = Role.system, content = txtSystem.Text },
                    new FineTuningBaseClass { role = Role.user, content = txtUser.Text },
                    new FineTuningBaseClass { role = Role.assistant, content = txtAssistant.Text }
                }
            });
            MessageBox.Show("Insert button clicked!");
        }

        private async void btnExport_Click(object sender, EventArgs e)
        {
            DBLib.DBTool dbTool = new DBLib.DBTool();
            var list = await dbTool.Query();
            StreamWriter sw = new StreamWriter(@"FineTuningData.jsonl");
            foreach (var item in list)
            {
                string jsonStr = JsonSerializer.Serialize(item);
                sw.WriteLine(jsonStr);
            }
            sw.Close();
        }
    }
}
