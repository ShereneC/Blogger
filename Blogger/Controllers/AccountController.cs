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

        [HttpPut]
        // I did write this, but it is not working, when I try to edit the name of the account, in postman it says ""Object reference not set to an instance of an object.""
        public async Task<ActionResult<Account>> Edit([FromBody] Account updatedAccount)
        {
            try
            {
            Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
            updatedAccount.Id = userInfo.Id;
            Account account = _accountService.Edit(updatedAccount, userInfo.Email);
            return Ok(account);
            }
            catch (Exception err)
            {
            return BadRequest(err.Message);
            }

        }

    }


}