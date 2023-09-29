using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LikesController : BaseApiController
    {
        private readonly UnitOfWork _unitOfWork;

        public LikesController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUserId();
            var likeduser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            var sourceUser =  await _unitOfWork.LikesRepository.GetUserWithLikes(sourceUserId);

            if(likeduser == null) return NotFound();
            if(sourceUser.UserName == username) return BadRequest("You cannot like yourself");
            var userLike = await _unitOfWork.LikesRepository.GetUserLike(sourceUserId,likeduser.Id);
            if(userLike != null) return BadRequest("You already liked this user");
            userLike = new UserLike{
                SourceUserId = sourceUserId,
                TargetUserId = likeduser.Id
            };
            sourceUser.LikedUsers.Add(userLike);

            if(await _unitOfWork.Complete()) return Ok();
            return BadRequest("Failed to like user");
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var users = await _unitOfWork.LikesRepository.GetUserLikes(likesParams);

            Response.AddPaginationHeader(new PaginationHeader(users.Currentpage, users.PageSize,
            users.TotalCount, users.TotalPages));

            return Ok(users);
        }
    }
}