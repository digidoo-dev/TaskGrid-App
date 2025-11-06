using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskGrid_App.Models;

class TaskDetails
{
    public int TaskId { get; set; }
    public string TaskName { get; set; } = null!;
    public string? TaskDescription { get; set; }
    public string ProjectName { get; set; } = null!;
    public string? AssignedToUser { get; set; }
    public bool AssignedToYou { get; set; }
}
