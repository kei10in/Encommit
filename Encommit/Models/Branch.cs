using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encommit.Models
{
    public class Branch
    {
        public Branch(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
