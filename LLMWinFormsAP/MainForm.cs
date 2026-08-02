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
        private LLMHelper? _LLMHelper;
        private Lazy<LLMHelper> _LLMHelperLazy;
        private OllamaHelper _OllamaHelper;
        private List<ChatMessage> _ChatMessages;

        public MainForm()
        {
            _LLMHelperLazy = new Lazy<LLMHelper>(() => new LLMHelper());
            _LLMHelper = null;
            //_LLMHelper.Load();
            _OllamaHelper = new OllamaHelper();
            _ChatMessages = new List<ChatMessage>();
            InitializeComponent();
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                //Ollama
                txtResponse.Text = "思考中...";
                var response = _OllamaHelper.SendPrompt(txtPrompt.Text);
                var sb = new System.Text.StringBuilder();

                await foreach (var item in response)
                {
                    sb.Append(item.response);
                    txtResponse.Text = sb.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSendRAG_Click(object sender, EventArgs e)
        {
            try
            {
                txtResponse.Text = "思考中...";
                string prompt = txtPrompt.Text;
                var message = new ChatMessage(ChatRole.User, prompt);
                _ChatMessages.Add(message);
                var response = _OllamaHelper.SendPromptWithRAG(prompt);
                var sb = new System.Text.StringBuilder();
                await foreach (var item in response)
                {
                    sb.Append(item.response);
                    txtResponse.Text = sb.ToString();
                }
                string fullResponse = txtResponse.Text;
                message = new ChatMessage(ChatRole.Assistant, fullResponse);
                _ChatMessages.Add(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _LLMHelper = _LLMHelperLazy.Value;
            _LLMHelper.ThreadStop();
        }

        private async void btnPromptTest_Click(object sender, EventArgs e)
        {
            try
            {
                txtResponse.Text = "思考中...";
                var response = await _OllamaHelper.GetCategoryByPrompt(txtPrompt.Text);
                txtResponse.Text = System.Text.Json.JsonSerializer.Serialize(response);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnPromptWithHistory_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
