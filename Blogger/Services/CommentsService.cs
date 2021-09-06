using System;
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

        internal Comment GetCommentById(int id)
    {
      Comment comment = _repo.GetCommentById(id);
      if (comment == null)
      {
          throw new Exception("Invalid Id");
      }
      return comment;
    }

    internal Comment CreateComment(Comment newComment)
    {
      return _repo.CreateComment(newComment);
    }

    internal Comment EditComment(Comment updatedComment, string userId)
    {
      // in the blogsservice, I have a call to getblogbyid so that it finds the right comment, do I have to make that function for comments, just so I can do this edit? or can I just pass things along to the repo?  I think I have to do that because I need to check if CreatorId = userId in here (I think) 
      Comment original = GetCommentById(updatedComment.Id);
      if (original.CreatorId != userId)
      {
        throw new Exception("This is not your comment to edit");
      }
      updatedComment.Body = updatedComment.Body != null ? updatedComment.Body : original.Body;
      return _repo.EditComment(updatedComment);
    }
  }
}