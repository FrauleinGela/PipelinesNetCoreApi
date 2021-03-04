using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApiDTO.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}