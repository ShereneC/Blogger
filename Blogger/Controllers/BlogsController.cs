using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blogger.Models;
using Blogger.Services;
using CodeWorks.Auth0Provider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogger.Controllers
{

    [ApiController]
    [Route("/api/[controller]")]
    public class BlogsController : ControllerBase
    {
        private readonly BlogsService _bs;
        private readonly CommentsService _cs;
        public BlogsController(BlogsService bs, CommentsService cs)
        {
            _bs =bs;
            _cs =cs;
        }

        [HttpGet]
        public ActionResult<List<Blog>> GetAllBlogs()
        {
            try
            {
                 List<Blog> blogs = _bs.GetAllBlogs();
                 return Ok(blogs);
            }
            catch (Exception err)
            {
 return BadRequest(err.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Blog> GetBlogById(int id)
        {
            try
            {
                 Blog blog = _bs.GetBlogById(id);
                 return Ok(blog);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

                [HttpGet("{id}/comments")]
        public ActionResult<List<Comment>> GetCommentsByBlogId(int id)
        // above how does it get this int id??? It pulls it out of the url path, right???
        {
            try
            {
                 List<Comment> comments = _cs.GetCommentsByBlogId(id);
                 return Ok(comments);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPost]
        [Authorize]

        public async Task<ActionResult<Blog>> CreateBlog([FromBody] Blog newBlog)
        {
            try
            {
                 Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                 newBlog.CreatorId = userInfo.Id;
                 Blog blog = _bs.CreateBlog(newBlog);
                 return Ok(blog);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Blog>> EditBlog([FromBody] Blog updatedBlog, int id)
        {
            try
            {
                 Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                 updatedBlog.Id = id;
                 updatedBlog.CreatorId = userInfo.Id;
                 Blog blog = _bs.EditBlog(updatedBlog, userInfo.Id);
                 // REVIEW what does the following line do?
                 blog.Creator = userInfo;
                 // REVIEW need to call get by id right here to return the updated blog
                 return Ok(blog);
            }
            catch (Exception err)
            {
            return BadRequest(err.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<String>> DeleteBlog(int id)
        {
            try
            {
                 // NEVER TRUST THE CLIENT TO TELL YOU WHO THEY ARE
                 Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                 _bs.DeleteBlog(id, userInfo.Id);
                 return Ok("Blog has been deleted");
            }
            catch (Exception err)
            {
            return BadRequest(err.Message);
            }
        }
    }
}