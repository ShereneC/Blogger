using System;
using System.Collections.Generic;
using Blogger.Models;
using Blogger.Repositories;

namespace Blogger.Services
{
  public class BlogsService
  {
      private readonly BlogsRepository _repo;
      public BlogsService(BlogsRepository repo)
      {
          _repo = repo;
      }
    internal List<Blog> GetAllBlogs()
    {
      return _repo.GetAllBlogs();
    }
    internal Blog GetBlogById(int id)
    {
      Blog blog = _repo.GetBlogById(id);
      if (blog == null)
      {
          throw new Exception("Invalid Id");
      }
      return blog;
    }

    internal Blog CreateBlog(Blog newBlog)
    {
      return _repo.CreateBlog(newBlog);
    }

  }
}