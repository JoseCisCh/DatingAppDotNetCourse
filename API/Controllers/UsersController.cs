using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using AutoMapper;
using API.DTOs;

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

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers() {
           var users = await _userRepository.GetMembersAsync();

           return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username){

            // The find method only work with Primary keys of a given table.
            return await _userRepository.GetMemberAsync(username);
            
        }
    }
}