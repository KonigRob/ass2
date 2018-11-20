using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;
using System;
using TodoApi.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace TodoApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            
        }

        [HttpGet("children")]
        public ActionResult<List<ApplicationUser>> GetAll()
        {
            var currentUser = GetUser();
            if(!_userManager.IsInRoleAsync(GetUser(), "Admin").Result) {
                return StatusCode(401, new { error = "Bad access" });
            }
            return _userManager.GetUsersInRoleAsync("Child").Result.ToList();
        }

        [HttpGet("children/{id}")]
        public IActionResult GetById(string id)
        {
            var currentUser = GetUser();
            if (!_userManager.IsInRoleAsync(GetUser(), "Admin").Result)
            {
                return StatusCode(401, new { error = "Bad access" });
            }
            var child = _context.ApplicationUsers.Where(user => user.Id == id).FirstOrDefault();
            if (child == null) { throw new Exception(id + " not found!"); }
            return Ok(child);
        }

        [HttpPut("children/{id}")]
        public ActionResult UpdateChild(string id, [FromBody] ApplicationUser item)
        {
            var currentUser = GetUser();
            if (!_userManager.IsInRoleAsync(GetUser(), "Admin").Result)
            {
                return StatusCode(401, new { error = "Bad access" });
            }
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var child = _context.ApplicationUsers.Where(user => user.Id == id).FirstOrDefault();
            if (child == null){ throw new Exception(id + " not found!"); }

            child.Email = child.Email;
            child.UserName = child.UserName;
            child.FirstName = child.FirstName;
            child.LastName = child.LastName;
            child.BirthDate = child.BirthDate;
            child.Street = child.Street;
            child.City = child.City;
            child.Province = child.Province;
            child.Country = child.Country;
            child.PostalCode = child.PostalCode;
            child.Latitude = child.Latitude;
            child.Longitude = child.Longitude;
            child.IsNaughty = child.IsNaughty;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("children/{id}")]
        public IActionResult Delete(string id)
        {
            var currentUser = GetUser();
            if (!_userManager.IsInRoleAsync(GetUser(), "Admin").Result)
            {
                return StatusCode(401, new { error = "Bad access" });
            }
            var child = _context.ApplicationUsers.Where(user => user.Id == id).FirstOrDefault();
            if (child == null) { throw new Exception(id + " not found!"); }
            _context.ApplicationUsers.Remove(child);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpGet("account")]
        public ActionResult<ApplicationUser> GetCheck() {
            return GetUser();
        }

        [HttpPut("account")]
        public ActionResult<ApplicationUser> PutNewUser([FromBody] ApplicationUser user) {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var child = GetUser();
            child.Email = child.Email;
            child.UserName = child.UserName;
            child.FirstName = child.FirstName;
            child.LastName = child.LastName;
            child.BirthDate = child.BirthDate;
            child.Street = child.Street;
            child.City = child.City;
            child.Province = child.Province;
            child.Country = child.Country;
            child.PostalCode = child.PostalCode;
            child.Latitude = child.Latitude;
            child.Longitude = child.Longitude;
            child.IsNaughty = child.IsNaughty;

            _context.SaveChanges();
            return user;
        }

        //Yeah, I found this from.  It really helped, and yeah. :p
        //https://stackoverflow.com/questions/50120968/extract-values-from-httpcontext-user-claims
        private ApplicationUser GetUser() {
            var email = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.Email).First().Value;

            var user = _context.ApplicationUsers.Where(usr => usr.Email == email).FirstOrDefault();
            if (user == null) {
                throw new Exception("user not found");
            }
            return user;
        }
    }
}
