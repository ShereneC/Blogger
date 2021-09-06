using System.Collections.Generic;
using System.Data;
using System.Linq;
using Blogger.Models;
using Dapper;

namespace Blogger.Repositories
{
  public class CommentsRepository
  {
      private readonly IDbConnection _db;
      public CommentsRepository(IDbConnection db)
      {
          _db = db;
      }
    internal List<Comment> GetCommentsByBlogId(int id)
    {
      string sql = @"
      SELECT a.*, c.*
      FROM comments c
      JOIN accounts a ON c.creatorId = a.id
      WHERE c.blogId = @id
      ";
      return _db.Query<Profile, Comment, Comment>(sql, (Profile, comment) =>
      {
          comment.Creator = Profile;
          return comment;
      }, new { id }, splitOn: "id").ToList();
    }

    internal Comment GetCommentById(int id)
    {
      string sql = @"
      SELECT *
      FROM comments
      WHERE id = @id
      ";
      return _db.QueryFirstOrDefault<Comment>(sql, new { id });
    }

    internal Comment CreateComment(Comment newComment)
    {
      string sql = @"
      INSERT INTO comments
      (body, blogId, creatorId)
      VALUES
      (@Body, @BlogId, @CreatorId);
      SELECT LAST_INSERT_ID();
      ";
      // REVIEW what does the above and below line do?
      int id = _db.ExecuteScalar<int>(sql, newComment);
      return newComment;
    }

    internal Comment EditComment(Comment updatedComment)
    {
      string sql = @"
      UPDATE comments
      SET
          body = @Body
      WHERE id = @Id;
      ";
      _db.Execute(sql, updatedComment);
      return GetCommentById(updatedComment.Id);
    }

    internal void DeleteComment(int id)
    {
      string sql = "DELETE FROM comments WHERE id = @id LIMIT 1";
      _db.Execute(sql, new { id });
    }
  }
}