using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLMLib.Models
{
    public class ImageResult
    {
        public ImageResultData[] data { get; set; } = Array.Empty<ImageResultData>();
    }

    public class ImageResultData
    {
        public string? revised_prompt { get; set; } = string.Empty;
        public string? url { get; set; } = string.Empty;
    }
}
