using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAGLib.Models
{
    public class EmbeddingResult
    {

        public string model { get; set; } = string.Empty;
        public float[][] embeddings { get; set; } = Array.Empty<float[]>();
    }
}
