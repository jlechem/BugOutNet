namespace BugOutNetLibrary.Models.GridModels
{
    public class BugGridModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Project { get; set; }
        public string Category { get; set; }
        public string Priority { get; set; }
        public string AssignedTo { get; set; }
        public string Status { get; set; }
        public string LastUpdatedBy { get; set; }
        public string LastUpdatedOn { get; set; }

    }
}