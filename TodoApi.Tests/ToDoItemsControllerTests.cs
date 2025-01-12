using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Tests
{
    public class ToDoItemsControllerTests
    {
        private readonly ToDoContext _context;
        private readonly ToDoItemsController _controller;

        public ToDoItemsControllerTests()
        {
            _context = TestHelper.GetInMemoryDbContext();
            _controller = new ToDoItemsController(_context);
        }

        [Fact]
        public async Task GetToDoItems_ReturnsAllItems_WhenFilterIsAll()
        {
            // Arrange
            var filter = ItemStatusFilter.All;

            // Act
            var result = await _controller.GetToDoItems(filter);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<ToDoItem>>>(result);
            var returnValue = Assert.IsType<List<ToDoItem>>(actionResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetToDoItems_ReturnsTodoItems_WhenFilterIsTodo()
        {
            // Arrange
            var filter = ItemStatusFilter.Todo;

            // Act
            var result = await _controller.GetToDoItems(filter);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<ToDoItem>>>(result);
            var returnValue = Assert.IsType<List<ToDoItem>>(actionResult.Value);
            Assert.Single(returnValue);
            Assert.False(returnValue.First().IsCompleted);
        }

        [Fact]
        public async Task GetToDoItems_ReturnsCompletedItems_WhenFilterIsCompleted()
        {
            // Arrange
            var filter = ItemStatusFilter.Completed;

            // Act
            var result = await _controller.GetToDoItems(filter);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<ToDoItem>>>(result);
            var returnValue = Assert.IsType<List<ToDoItem>>(actionResult.Value);
            Assert.Single(returnValue);
            Assert.True(returnValue.First().IsCompleted);
        }

        [Fact]
        public async Task GetToDoItems_ReturnsAllItems_WhenFilterIsNull()
        {
            // Arrange
            ItemStatusFilter? filter = null;

            // Act
            var result = await _controller.GetToDoItems(filter);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<ToDoItem>>>(result);
            var returnValue = Assert.IsType<List<ToDoItem>>(actionResult.Value);
            Assert.Equal(2, returnValue.Count);
        }
    }
}