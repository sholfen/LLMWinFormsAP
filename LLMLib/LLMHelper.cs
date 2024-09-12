using Microsoft.ML.OnnxRuntimeGenAI;
using System.Text;

namespace LLMLib
{
    public class LLMHelper
    {
        private string _modelPath = string.Empty;

        private Model? _model;

        public LLMHelper(string path)
        {
            if(string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path is invalid.");
            }
            SetModelPath(path);
        }

        public LLMHelper()
        {
            SetModelPath("D:\\LLM\\onnx\\Phi-3-mini-4k-instruct-onnx\\cuda\\cuda-fp16");
            //SetModelPath("D:\\LLM\\onnx\\Phi-3-small-8k-instruct-onnx-cuda\\cuda-int4-rtn-block-32");
            //SetModelPath("D:\\LLM\\onnx\\Phi-3-medium-128k-instruct-onnx-cuda\\cuda-fp16");
        }

        private void SetModelPath(string modelPath)
        {
            _modelPath = modelPath;
        }

        public void Load()
        {
            _model = new Model(_modelPath);
        }

        public string SendPrompt(string prompt)
        {
            string systemPrompt = "You are a knowledgeable and friendly assistant. Answer the following question as clearly and concisely as possible, providing any relevant information and examples.";
            string userPrompt = prompt;
            var tokenizer = new Tokenizer(_model);

            // 組合 Prompt：將 System Prompt 和 User Prompt 組合在一起
            // combine system prompt and user prompt
            var fullPrompt = $"<|system|>{systemPrompt}<|end|><|user|>{userPrompt}<|end|><|assistant|>";
            var tokens = tokenizer.Encode(fullPrompt);

            var generatorParams = new GeneratorParams(_model);
            generatorParams.SetSearchOption("max_length", 2048);
            //generatorParams.SetSearchOption("temperature", 0.3);
            generatorParams.SetInputSequences(tokens);

            //return response
            StringBuilder response = new StringBuilder();
            var generator = new Generator(_model, generatorParams);
            while (!generator.IsDone())
            {
                generator.ComputeLogits();
                generator.GenerateNextToken();
                var outputTokens = generator.GetSequence(0);
                var newToken = outputTokens.Slice(outputTokens.Length - 1, 1);
                var output = tokenizer.Decode(newToken);
                response.Append(output);
            }
            return response.ToString();
        }

        public string Prompt { get; set; } = string.Empty;
        public string Response { get; set; } = string.Empty;

        public Action? Handler { get; set; }

        public void SendPromptForThread()
        {
            if(Handler == null)
            {
                throw new NullReferenceException("not setting handler");
            }

            Response = string.Empty;
            string systemPrompt = "You are a knowledgeable and friendly assistant. Answer the following question as clearly and concisely as possible, providing any relevant information and examples.";
            string userPrompt = Prompt;
            var tokenizer = new Tokenizer(_model);

            // 組合 Prompt：將 System Prompt 和 User Prompt 組合在一起
            // combine system prompt and user prompt
            var fullPrompt = $"<|system|>{systemPrompt}<|end|><|user|>{userPrompt}<|end|><|assistant|>";
            var tokens = tokenizer.Encode(fullPrompt);

            var generatorParams = new GeneratorParams(_model);
            generatorParams.SetSearchOption("max_length", 2048);
            //generatorParams.SetSearchOption("temperature", 0.3);
            generatorParams.SetInputSequences(tokens);

            StringBuilder response = new StringBuilder();
            var generator = new Generator(_model, generatorParams);
            while (!generator.IsDone())
            {
                generator.ComputeLogits();
                generator.GenerateNextToken();
                var outputTokens = generator.GetSequence(0);
                var newToken = outputTokens.Slice(outputTokens.Length - 1, 1);
                var output = tokenizer.Decode(newToken);
                response.Append(output);
                Response = response.ToString();
                Handler();
            }
        }

        public void ThreadStart()
        {
            Thread thread = new Thread(SendPromptForThread);
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
