using System.Collections.Generic;
using System.Data;
using System.Linq;
using Blogger.Models;
using Dapper;

namespace Blogger.Repositories
{
  public class BlogsRepository
  {
      private readonly IDbConnection _db;

      public BlogsRepository(IDbConnection db)
      {
          _db = db;
      }
    internal List<Blog> GetAllBlogs()
    {
      string sql = @"
      SELECT * FROM blogs
      ";
      return _db.Query<Blog>(sql).ToList();
    }

    internal Blog GetBlogById(int id)
    {
      string sql = @"
      SELECT a.*, b.*
      FROM blogs b 
      JOIN accounts a ON b.creatorId = a.id
      WHERE b.id = @id
      ";
      // REVIEW what is this new thing? Why are we returning something new?
      return _db.Query<Account, Blog, Blog>(sql, (account, Blog) =>
      {
          Blog.Creator = account;
          return Blog;
      }, new { id }, splitOn: "id").FirstOrDefault();
    }

    internal Blog CreateBlog(Blog newBlog)
    {
      string sql =@"
      INSERT INTO blogs
      (title, body, imgUrl, published, creatorId)
      VALUES
      (@Title, @Body, @ImgUrl, @Published, @CreatorId);
      SELECT LAST_INSERT_ID();
      ";
      // REVIEW I was trying to make create without yet doing getbyid, but it would not let me return nothing, how could/should I have done that?  Idea - go into controller and change return type to a string and then just return "it has been created?"  I think I did this in tutoring with Justin on the castles project?
      int id = _db.ExecuteScalar<int>(sql, newBlog);
      return GetBlogById(id);
    }
  }
}