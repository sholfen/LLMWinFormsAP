using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FineTuningToolWinFormsAP
{
    public enum Role
    {
        system,
        user,
        assistant
    }

    public class FineTuningBaseClass
    {
        public Role role { get; set; } = Role.system;
        public string content { get; set; } = string.Empty;
    }

    public class FineTuningBaseClassList
    {
        public List<FineTuningBaseClass> messages { get; set; } = new List<FineTuningBaseClass>();
    }
}
