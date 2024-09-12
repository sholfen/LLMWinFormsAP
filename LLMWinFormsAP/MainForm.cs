using LLMLib;

namespace LLMWinFormsAP
{
    public partial class MainForm : Form
    {
        private LLMHelper _LLMHelper;
        public MainForm()
        {
            _LLMHelper = new LLMHelper();
            _LLMHelper.Load();
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            //string answer = _LLMHelper.SendPrompt(txtPrompt.Text);
            //txtResponse.Text = answer;

            //other
            _LLMHelper.Prompt = txtPrompt.Text;
            _LLMHelper.Handler = () => txtResponse.Text = _LLMHelper.Response;
            _LLMHelper.ThreadStart();
        }
    }
}
