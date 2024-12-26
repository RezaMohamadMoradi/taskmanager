using Microsoft.EntityFrameworkCore;
using System.Linq;
using taskmanager.Components.Pages;
using taskmanager.Model;
using static taskmanager.Components.Pages.Home;
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
        public async System.Threading.Tasks.Task AddTaskAsync(AddTask.AddTaskModel model, string userId)
        {
            var newTask = new Task
            {
                Title = model.Title,
                Description = model.Description,
                Category = model.Category,
                AddDate = model.AddDate,
                DateCompletion = model.DateCompletion,
                UserId = userId // اضافه کردن شناسه کاربر
            };

            _db.Tasks.Add(newTask);
            await _db.SaveChangesAsync();
        }


        // خواندن تمام تسک‌های یک کاربر
        public async System.Threading.Tasks.Task<List<Model.Task>> GetAllTasksAsync(string userId)
        {
            return await _db.Tasks
                .Include(t => t.user)
                .Where(t => t.user.Id == userId)
                .OrderByDescending(t => t.AddDate)
                .ToListAsync();
        }
        public async Task<Task> GetTaskByIdAsync(int taskId)
        {
            return await _db.Tasks.FirstOrDefaultAsync(t => t.id == taskId);
        }


        // به‌روزرسانی تسک
        public async System.Threading.Tasks.Task RemoveTask(int taskId)
        {
            var task = await _db.Tasks.FirstOrDefaultAsync(t => t.id == taskId);
            if (task != null)
            {
                _db.Tasks.Remove(task);
                await _db.SaveChangesAsync();
            }
        }

        public async System.Threading.Tasks.Task UpdateTask(Task updatedTask)
        {
            var existingTask = await _db.Tasks.FirstOrDefaultAsync(t => t.id == updatedTask.id);
            if (existingTask != null)
            {
                existingTask.Title = updatedTask.Title;
                existingTask.Description = updatedTask.Description;
                existingTask.Category = updatedTask.Category;
                existingTask.Status = updatedTask.Status;
                existingTask.DateCompletion = updatedTask.DateCompletion;

                // در اینجا تغییرات را در پایگاه داده ذخیره می‌کنیم
                await _db.SaveChangesAsync();
            }
        }


        // تغییر وضعیت تسک
        public async System.Threading.Tasks.Task ChangeTaskStatus(int taskId)
        {
            var task = _db.Tasks.FirstOrDefault(t => t.id == taskId);
            if (task != null)
            {
                task.Status = TaskStatus.Completed;
               

                await _db.SaveChangesAsync();
            }
        }
    }
}
