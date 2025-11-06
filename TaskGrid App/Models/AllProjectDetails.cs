using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskGrid_App.Models;

public class AllProjectDetails
{
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = null!;
    public string ProjectOwner { get; set; } = null!;
    public bool IsProjectOwner {  get; set; }
    public List<string> ProjectMembers { get; set; } = null!;
    public List<string> GridHeaders { get; set; } = null!;
    public List<string> GridSeparators { get; set; } = null!;
    public List<ProjectTask> ProjectTasks { get; set; } = null!;
}
