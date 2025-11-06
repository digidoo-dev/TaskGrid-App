using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskGrid_App.Models;

public class TaskCreationInfo
{
    public List<int> SeparatorProjectGridIds { get; set; } = new List<int>();
    public List<string> SeparatorNames { get; set; } = new List<string>();
    public List<int> HeaderProjectGridIds { get; set; } = new List<int>();
    public List<string> HeaderNames { get; set; } = new List<string>();
}
