using Microsoft.EntityFrameworkCore;
using System.Linq;
using taskmanager.Model;
using taskmanager.Model;
using Task = taskmanager.Model.Task;
using TaskStatus = taskmanager.Model.TaskStatus;


namespace taskmanager.Services
{
    public class TaskServices
    {
        private readonly Db _db;
        public TaskServices(Db db)
        {
            
            _db = db;
        }
        public void AddTask(Task task)
        {
            task.AddDate = DateTime.Now; // تنظیم تاریخ افزودن
            _db.Tasks.Add(task);
            _db.SaveChanges();
        }

        // خواندن تمام تسک‌های یک کاربر
        public List<Task> GetAllTasks(string userId)
        {
            return _db.Tasks
                      .Include(t => t.user)
                      .Where(t => t.user.Id == userId)
                      .OrderByDescending(t => t.AddDate)
                      .ToList();
        }

        // به‌روزرسانی تسک
        public void UpdateTask(Task updatedTask)
        {
            var existingTask = _db.Tasks.FirstOrDefault(t => t.id == updatedTask.id);
            if (existingTask != null)
            {
                existingTask.Title = updatedTask.Title;
                existingTask.Description = updatedTask.Description;
                existingTask.Category = updatedTask.Category;
                existingTask.Status = updatedTask.Status;
                existingTask.DateCompletion = updatedTask.Status == TaskStatus.Completed
                    ? DateTime.Now // تنظیم تاریخ اتمام اگر تسک کامل شد
                    : default;

                _db.SaveChanges();
            }
        }

        // حذف تسک
        public void RemoveTask(int taskId)
        {
            var task = _db.Tasks.FirstOrDefault(t => t.id == taskId);
            if (task != null)
            {
                _db.Tasks.Remove(task);
                _db.SaveChanges();
            }
        }

        // تغییر وضعیت تسک
        public void ChangeTaskStatus(int taskId, TaskStatus newStatus)
        {
            var task = _db.Tasks.FirstOrDefault(t => t.id == taskId);
            if (task != null)
            {
                task.Status = newStatus;
                task.DateCompletion = newStatus == TaskStatus.Completed
                    ? DateTime.Now // اگر وضعیت کامل شد، تاریخ اتمام تنظیم شود
                    : default;

                _db.SaveChanges();
            }
        }
    }
}
