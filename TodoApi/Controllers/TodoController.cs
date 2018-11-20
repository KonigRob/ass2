using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;
using System;

namespace TodoApi.Controllers
{
    [Route("api/todo")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                 //Create a new TodoItem if collection is empty,
                 //which means you can't delete all TodoItems.
                _context.TodoItems.Add(new TodoItem
                {
                    FirstName = "lName",
                    LastName = "fName",
                    BirthDate = new DateTime(2008, 5, 1, 8, 30, 52),
                    Street = "Dummy Street",
                    City = "Bitchmond",
                    Province = "BC",
                    PostalCode = "No",
                    Country = "Canada",
                    Latitude = 1.0,
                    Longitude = 1.0,
                    IsNaughty = true,
                    DateCreated = new DateTime(2008, 5, 1, 8, 30, 52)

                });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public ActionResult<List<TodoItem>> GetAll()
        {
            return _context.TodoItems.ToList();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public ActionResult<TodoItem> GetById(int id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        public IActionResult Create(TodoItem item)
        {
            _context.TodoItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        //[HttpGet("{id}", Name = "GetTodo")]

        [HttpPut("{id}")]
        public IActionResult Update(int id, TodoItem item)
        {
            var todo = _context.TodoItems.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.FirstName = item.FirstName;
            todo.LastName = item.LastName;
            todo.BirthDate = item.BirthDate;
            todo.Street = item.Street;
            todo.City = item.City;
            todo.Province = item.Province;
            todo.PostalCode = item.PostalCode;
            todo.Country = item.Country;
            todo.Latitude = item.Latitude;
            todo.Longitude = item.Longitude;
            todo.IsNaughty = item.IsNaughty;
            todo.DateCreated = item.DateCreated;

            _context.TodoItems.Update(todo);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var todo = _context.TodoItems.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todo);
            _context.SaveChanges();
            return NoContent();
        }
    }
}