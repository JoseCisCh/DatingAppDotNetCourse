using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using AutoMapper;
using API.DTOs;
using System.Security.Claims;
using API.Extensions;
using System.Reflection.Metadata.Ecma335;
using API.Helpers;

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
        private IPhotoService _photoService { get; }

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            this._photoService = photoService;
            this._userRepository = userRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery] UserParams userParams) {
           var users = await _userRepository.GetMembersAsync(userParams);
           Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize
                                                , users.TotalCount, users.TotalPages));

           return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username){

            // The find method only work with Primary keys of a given table.
            return await _userRepository.GetMemberAsync(username);
            
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto) {
            
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return NotFound();

            _mapper.Map(memberUpdateDto, user);

            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file) 
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return NotFound();

            var result = await _photoService.AddPhotoAsnc(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (user.Photos.Count == 0) photo.IsMain = true;

            user.Photos.Add(photo);

            if (await _userRepository.SaveAllAsync()){
                return CreatedAtAction(nameof(GetUser), new {username = user.UserName},
                                        _mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId) {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null ) return NotFound();

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

            if (currentMain != null) {
                currentMain.IsMain = false;
            }

            photo.IsMain = true;

            // no CONTENT response of a successfull http put request
            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Problem setting main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId) {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            // Not needed to check if user is valid as this endpoint should only 
            // be able to be trigerred if the user is authenticated.

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if(photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null) {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);

                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            // Ok response of a successfull http delete request.
            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Problem deleting photo");

        } 
    }
}