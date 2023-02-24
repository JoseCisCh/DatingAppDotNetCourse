using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers {

    /* NEXT LINES: Lesson 15.
        We need to add the next ATTRIBUTES to the class
        To add this attributes we need Microsoft.AspNetCore.Mvc
    */

    [ApiController]
    [Route("api/[controller]")] // /api/users
    public class UsersController  {

        readonly DataContext _context; // It is a convention for some to mark _property for private properties of a class.
        public UsersController(DataContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id){

            // The find method only work with Primary keys of a given table.
            var user = await _context.Users.FindAsync(id);
            return user;
        }
    }
}