using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;
using System;
using TodoApi.Data;

namespace TodoApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;

            if (_context.ApplicationUsers.Count() == 0)
            {
                //Create a new TodoItem if collection is empty,
                //which means you can't delete all TodoItems.
                _context.ApplicationUsers.Add(new ApplicationUser
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
                    IsNaughty = true

                });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public ActionResult<List<ApplicationUser>> GetAll()
        {
            return _context.ApplicationUsers.ToList();
        }

        [HttpGet("{id}", Name = "GetUser")]
        public ActionResult<ApplicationUser> GetById(int id)
        {
            var item = _context.ApplicationUsers.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        public IActionResult Create(ApplicationUser item)
        {
            _context.ApplicationUsers.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, ApplicationUser item)
        {
            var todo = _context.ApplicationUsers.Find(id);
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

            _context.ApplicationUsers.Update(todo);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var todo = _context.ApplicationUsers.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.ApplicationUsers.Remove(todo);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
