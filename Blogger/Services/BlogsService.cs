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

    internal Blog EditBlog(Blog updatedBlog, string userId)
    {
      Blog original = GetBlogById(updatedBlog.Id);
      if (original.CreatorId != userId) 
      {
        // if this gets thrown does it end the function? or do I need to put in an else statement? He did not use an else on tower (delete)
        throw new Exception("This is not your blog to edit");
      }
      updatedBlog.Title = updatedBlog.Title != null ? updatedBlog.Title : original.Title;
      updatedBlog.Body = updatedBlog.Body != null ? updatedBlog.Body : original.Body;
      updatedBlog.ImgUrl = updatedBlog.ImgUrl != null ? updatedBlog.ImgUrl : original.ImgUrl;
      //I don't know how to tell it not to expect this one, if I put in null, then it will say that a bool is never null and it won't let me do that.
      updatedBlog.Published = updatedBlog.Published != original.Published ? updatedBlog.Published : original.Published;
      return _repo.EditBlog(updatedBlog);

    }

    internal void DeleteBlog(int blogId, string userId)
    {
      Blog blogToDelete = GetBlogById(blogId);
      if (blogToDelete.CreatorId != userId)
      {
        throw new Exception("This is not your blog to delete");
      }
      _repo.DeleteBlog(blogId);
    }
  }
}