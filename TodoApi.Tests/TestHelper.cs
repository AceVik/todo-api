using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Tests
{
    public static class TestHelper
    {
        public static ToDoContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ToDoContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Jeder Test erhält eine neue Datenbank
                .Options;

            var context = new ToDoContext(options);
            context.ToDoItems.AddRange(
                new ToDoItem { Id = 1, Title = "Task 1", IsCompleted = false },
                new ToDoItem { Id = 2, Title = "Task 2", IsCompleted = true }
            );
            context.SaveChanges();

            return context;
        }
    }
}