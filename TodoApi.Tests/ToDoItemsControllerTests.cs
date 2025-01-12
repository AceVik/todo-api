using Microsoft.AspNetCore.Mvc;
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

        #region GET /api/ToDoItems

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

        #endregion

        #region GET /api/ToDoItems/{id}

        [Fact]
        public async Task GetToDoItem_ReturnsItem_WhenItemExists()
        {
            // Arrange
            int existingId = 1;

            // Act
            var result = await _controller.GetToDoItem(existingId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ToDoItem>>(result);
            var returnValue = Assert.IsType<ToDoItem>(actionResult.Value);
            Assert.Equal(existingId, returnValue.Id);
        }

        [Fact]
        public async Task GetToDoItem_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Arrange
            int nonExistingId = 999;

            // Act
            var result = await _controller.GetToDoItem(nonExistingId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ToDoItem>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        #endregion

        #region POST /api/ToDoItems

        [Fact]
        public async Task PostToDoItem_CreatesItem_WhenDataIsValid()
        {
            // Arrange
            var dto = new CreateTodoItemDto
            {
                Title = "New Task",
                IsCompleted = false
            };

            // Act
            var result = await _controller.PostToDoItem(dto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ToDoItem>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var createdItem = Assert.IsType<ToDoItem>(createdAtActionResult.Value);
            Assert.Equal(dto.Title, createdItem.Title);
            Assert.Equal(dto.IsCompleted, createdItem.IsCompleted);
            Assert.True(createdItem.Id > 0);
        }

        [Fact]
        public async Task PostToDoItem_SetsIsCompletedToFalse_WhenNotSpecified()
        {
            // Arrange
            var dto = new CreateTodoItemDto
            {
                Title = "New Task",
                IsCompleted = null // Optionales Feld
            };

            // Act
            var result = await _controller.PostToDoItem(dto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ToDoItem>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var createdItem = Assert.IsType<ToDoItem>(createdAtActionResult.Value);
            Assert.Equal(dto.Title, createdItem.Title);
            Assert.False(createdItem.IsCompleted); // Standardwert
        }

        #endregion

        #region PATCH /api/ToDoItems/{id}

        [Fact]
        public async Task PatchToDoItem_UpdatesTitle_WhenTitleIsProvided()
        {
            // Arrange
            int existingId = 1;
            var dto = new PatchTodoItemDto
            {
                Title = "Updated Task Title"
            };

            // Act
            var result = await _controller.PatchToDoItem(existingId, dto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ToDoItem>>(result);
            var updatedItem = Assert.IsType<ToDoItem>(actionResult.Value);
            Assert.Equal(dto.Title, updatedItem.Title);
            Assert.False(updatedItem.IsCompleted); // Unverändert
        }

        [Fact]
        public async Task PatchToDoItem_UpdatesIsCompleted_WhenIsCompletedIsProvided()
        {
            // Arrange
            int existingId = 1;
            var dto = new PatchTodoItemDto
            {
                IsCompleted = true
            };

            // Act
            var result = await _controller.PatchToDoItem(existingId, dto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ToDoItem>>(result);
            var updatedItem = Assert.IsType<ToDoItem>(actionResult.Value);
            Assert.True(updatedItem.IsCompleted);
        }

        [Fact]
        public async Task PatchToDoItem_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Arrange
            int nonExistingId = 999;
            var dto = new PatchTodoItemDto
            {
                Title = "Non-existing Task",
                IsCompleted = true
            };

            // Act
            var result = await _controller.PatchToDoItem(nonExistingId, dto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ToDoItem>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        #endregion

        #region DELETE /api/ToDoItems/{id}

        [Fact]
        public async Task DeleteToDoItem_DeletesItem_WhenItemExists()
        {
            // Arrange
            int existingId = 1;

            // Act
            var result = await _controller.DeleteToDoItem(existingId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Null(await _context.ToDoItems.FindAsync(existingId));
        }

        [Fact]
        public async Task DeleteToDoItem_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Arrange
            int nonExistingId = 999;

            // Act
            var result = await _controller.DeleteToDoItem(nonExistingId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        #endregion
    }
}
