using LLMLib;

namespace LLMWinFormsAP
{
    public partial class MainForm : Form
    {
        private LLMHelper _LLMHelper;
        private OllamaHelper _OllamaHelper;

        public MainForm()
        {
            _LLMHelper = new LLMHelper();
            _LLMHelper.Load();
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
            txtResponse.Text = "«ä¦Ò¤¤...";
            var response = _OllamaHelper.SendPrompt(txtPrompt.Text);
            await foreach (var item in response)
            {
                txtResponse.Text += item.response;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _LLMHelper.ThreadStop();
        }
    }
}
