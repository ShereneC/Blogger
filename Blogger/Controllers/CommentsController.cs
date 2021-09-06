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
    }
}