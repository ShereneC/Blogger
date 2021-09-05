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
    [Route("[controller]")]
        [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly BlogsService _bs;

        public AccountController(AccountService accountService, BlogsService blogsService)
        {
            _accountService = accountService;
            _bs = blogsService;
        }

        [HttpGet]
        public async Task<ActionResult<Account>> Get()
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                return Ok(_accountService.GetOrCreateProfile(userInfo));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{blogs}")]
        public async Task<ActionResult<List<Blog>>> GetBlogsByCreatorId()
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                 List<Blog> blogs = _bs.GetBlogsByCreatorId(userInfo.Id);
                 return Ok(blogs);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
    }


}