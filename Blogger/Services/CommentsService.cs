using System.Collections.Generic;
using Blogger.Models;
using Blogger.Repositories;

namespace Blogger.Services
{
  public class CommentsService
  {
      private readonly CommentsRepository _repo;
      public CommentsService(CommentsRepository repo)
      {
          _repo = repo;
      }
    internal List<Comment> GetCommentsByBlogId(int id)
    {
      return _repo.GetCommentsByBlogId(id);
    }

    internal Comment CreateComment(Comment newComment)
    {
      return _repo.CreateComment(newComment);
    }
  }
}