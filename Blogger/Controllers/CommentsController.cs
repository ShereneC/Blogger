using System;
using System.Threading.Tasks;
using Blogger.Models;
using Blogger.Services;
using Microsoft.AspNetCore.Authorization;
using CodeWorks.Auth0Provider;
using Microsoft.AspNetCore.Mvc;

namespace Blogger.Controllers
{

    [ApiController]
    [Route("/api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly CommentsService _cs;
        public CommentsController(CommentsService cs)
        {
            _cs =cs;
        }

        [HttpPost]
        [Authorize]

        public async Task<ActionResult<Comment>> CreateComment([FromBody] Comment newComment) 
        {
            try
            {
                 Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                 newComment.CreatorId = userInfo.Id;
                 Comment comment = _cs.CreateComment(newComment);
                 return Ok(comment);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Comment>> EditComment([FromBody] Comment updatedComment, int id)
        {
            try
            {
                 Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                 updatedComment.Id = id;
                 updatedComment.CreatorId = userInfo.Id;
                 Comment comment = _cs.EditComment(updatedComment, userInfo.Id);
                 comment.Creator = userInfo;
                 // REVIEW do i need to call getCommentsByBlogId right here?
                 return Ok(comment);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
    }
}