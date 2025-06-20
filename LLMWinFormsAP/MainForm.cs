using LLMLib;

namespace LLMWinFormsAP
{
    public partial class MainForm : Form
    {
        private LLMHelper _LLMHelper;
        private OllamaHelper _OllamaHelper;

        public MainForm()
        {
            //_LLMHelper = new LLMHelper();
            //_LLMHelper.Load();
            _OllamaHelper = new OllamaHelper();
            InitializeComponent();
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            //string answer = _LLMHelper.SendPrompt(txtPrompt.Text);
            //txtResponse.Text = answer;

            //other
            //_LLMHelper.Prompt = txtPrompt.Text;
            //_LLMHelper.Handler = () => txtResponse.Text = _LLMHelper.Response;
            //_LLMHelper.ThreadStart();

            //Ollama
            txtResponse.Text = "思考中...";
            var response = _OllamaHelper.SendPrompt(txtPrompt.Text);
           
            bool flag = true;
            await foreach (var item in response)
            {
                if (flag)
                {
                    txtResponse.Text = string.Empty;
                    flag = false;
                }
                txtResponse.Text += item.response;
            }
        }

        private async void btnSendRAG_Click(object sender, EventArgs e)
        {
            txtResponse.Text = "思考中...";
            var response = _OllamaHelper.SendPromptWithRAG(txtPrompt.Text);
            bool flag = true;
            await foreach (var item in response)
            {
                if (flag)
                {
                    txtResponse.Text = string.Empty;
                    flag = false;
                }
                txtResponse.Text += item.response;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _LLMHelper.ThreadStop();
        }

        private async void btnPromptTest_Click(object sender, EventArgs e)
        {
            txtResponse.Text = "思考中...";
            var response = await _OllamaHelper.GetCategoryByPrompt(txtPrompt.Text);
            txtResponse.Text = System.Text.Json.JsonSerializer.Serialize(response);
        }
    }
}
