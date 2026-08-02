using System.Text.Json;
using System.Threading.Tasks;

namespace FineTuningToolWinFormsAP
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private async void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                DBLib.DBTool dbTool = new DBLib.DBTool();
                await dbTool.InsertData(new FineTuningBaseClassList
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
            catch (Exception ex)
            {
                MessageBox.Show($"發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                DBLib.DBTool dbTool = new DBLib.DBTool();
                var list = await dbTool.Query();
                using StreamWriter sw = new StreamWriter(@"FineTuningData.jsonl");
                foreach (var item in list)
                {
                    string jsonStr = JsonSerializer.Serialize(item);
                    sw.WriteLine(jsonStr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
