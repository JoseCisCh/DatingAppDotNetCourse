using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers {

    /* NEXT LINES: Lesson 15.
        We need to add the next ATTRIBUTES to the class
        To add this attributes we need Microsoft.AspNetCore.Mvc
    */

/* This can be removed as we created our BaseApiController
    [ApiController] 
    [Route("api/[controller]")] // /api/users

*/
    [Authorize]
    public class UsersController: BaseApiController  {

        readonly DataContext _context; // It is a convention for some to mark _property for private properties of a class.
        public UsersController(DataContext context)
        {
            this._context = context;
        }

        [AllowAnonymous]
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