using CDS.Markdown;
using LLMLib;
using Microsoft.Extensions.AI;
using OpenAI.Chat;
using Qdrant.Client.Grpc;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;

namespace LLMWinFormsAP
{
    public partial class MainForm : Form
    {
        private LLMHelper _LLMHelper;
        private OllamaHelper _OllamaHelper;
        private List<ChatMessage> _ChatMessages;

        public MainForm()
        {
            //_LLMHelper = new LLMHelper();
            //_LLMHelper.Load();
            _OllamaHelper = new OllamaHelper();
            _ChatMessages = new List<ChatMessage>();
            InitializeComponent();
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
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
            string prompt = txtPrompt.Text;
            var message = new ChatMessage(ChatRole.User, prompt);
            _ChatMessages.Add(message);
            var response = _OllamaHelper.SendPromptWithRAG(prompt);
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
            string fullResponse = txtResponse.Text;
            message = new ChatMessage(ChatRole.Assistant, fullResponse);
            _ChatMessages.Add(message);
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

        private async void btnPromptWithHistory_Click(object sender, EventArgs e)
        {
            txtResponse.Text += txtPrompt.Text + Environment.NewLine + Environment.NewLine;
            await foreach (var item in _OllamaHelper.SendPromptWithChatMessages(txtPrompt.Text))
            {
                txtResponse.Text += item.Text;
            }
            txtResponse.Text += Environment.NewLine;



            //var viewer = new MarkdownViewer();
            await resMarkdownViewer.LoadMarkdownFromStringAsync(txtResponse.Text);
            //await viewer.LoadMarkdownFromStringAsync(txtResponse.Text);
            //tabPage1.Controls.Add(viewer);
        }
    }
}
