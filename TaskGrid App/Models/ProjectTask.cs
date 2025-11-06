using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskGrid_App.Models;

public class ProjectTask
{
    public int TaskId { get; set; }

    public string TaskName { get; set; } = null!;


    public string? AssignedToUser { get; set; }


    public int SeparatorIndex { get; set; }


    public int HeaderIndex { get; set; }
}
