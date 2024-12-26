using taskmanager.Model;

namespace taskmanager.Model
{
    public class Task
    {
        public int id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.Pending;// تغییر داده شد

        public string? Category { get; set; }
        public DateTime? AddDate { get; set; }
        public DateTime? DateCompletion { get; set; }

        public User user { get; set; }
        public string? UserId { get; set; } // اضافه کردن شناسه کاربر

    }

    // تعریف Enum
    public enum TaskStatus
    {
        Pending = 0,
        InProgress = 1,
        Completed = 2
    }

}
