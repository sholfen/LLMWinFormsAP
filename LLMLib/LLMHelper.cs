using Microsoft.ML.OnnxRuntimeGenAI;
using System.Text;

namespace LLMLib
{
    public class LLMHelper
    {
        private string _modelPath = string.Empty;
        private Thread _thread;

        private Model? _model;

        public LLMHelper( string path)
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
        }

        private void SetModelPath(string modelPath)
        {
            _modelPath = modelPath;
        }

        public void Load()
        {
            _model = new Model(_modelPath);
        }
     
        public void ThreadStop()
        {
            if (_thread != null && _thread.IsAlive)
            {
                _thread.Interrupt();
                _thread.Join();
            }
        }
    }
}
