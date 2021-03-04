using System;
using System.Linq;
using TodoApiDTO.Controllers;
using Xunit;

namespace TodoApiDTO.Test
{
    public class TodoItemsControllerTest
    {
        public TodoItemsController Setup()
        {
            return new TodoItemsController();
        }
        [Fact]
        public void GetTodoItemsReturnsEightItems()
        {
            var controller = Setup();
            var todoItems = controller.GetTodoItems();
            Assert.Equal(8, todoItems.Count());
        }
    }
}