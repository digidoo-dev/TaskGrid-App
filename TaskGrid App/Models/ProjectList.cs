using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskGrid_App.Models
{
    class ProjectList
    {
        public List<int> ProjectIds { get; set; } = new List<int>();
        public List<string> ProjectNames { get; set; } = new List<string>();
    }
}
