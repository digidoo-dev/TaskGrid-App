using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskGrid_App.Helpers;

public static class StaticAppValues
{
    public static int SelectedProjectId { get; set; } = -1;
    public static List<string> NamesInOrder { get; set; } = new List<string>();
}
