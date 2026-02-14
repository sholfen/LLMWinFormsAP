using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngineManager
{
    public interface ILBSearchManager
    {
        void CreateIndex<T>(T contents) where T : class;
        //void Search<T>(string keyword) where T : class;
        List<string> Search<T>(string keyword) where T : class;
    }
}
