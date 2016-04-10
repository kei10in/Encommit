using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encommit.Models
{
    public class RemoteBranch
    {
        public RemoteBranch(string name, string remote)
        {
            Name = name;
            Remote = remote;
        } 

        public string Name { get; }
        public string Remote { get; }
    }
}
