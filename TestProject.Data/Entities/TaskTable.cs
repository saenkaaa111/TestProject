namespace TestProject.Data
{
    public class TaskTable
    {
        public Guid Id { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
