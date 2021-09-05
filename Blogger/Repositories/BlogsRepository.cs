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
      SELECT a.*, b.*
      FROM blogs b 
      JOIN accounts a ON b.creatorId = a.id
      ";
      return _db.Query<Profile, Blog, Blog>(sql, (profile, blog) =>
      {
          blog.Creator = profile;
          return blog;
      }, splitOn: "id").ToList();
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
      return _db.Query<Profile, Blog, Blog>(sql, (profile, blog) =>
      {
          blog.Creator = profile;
          return blog;
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

    internal Blog EditBlog(Blog updatedBlog)
    {
      string sql = @"
      UPDATE blogs
      SET
          title = @Title,
          body = @Body,
          imgUrl =@ImgUrl,
          published =@Published
      WHERE id = @Id;
      ";
      _db.Execute(sql, updatedBlog);
      // REVIEW I think this is another instance of returning what we just sent in, when we need to do a get and return the actually updated whole blog.  
      // return updatedBlog;
      // is it as simple as...
      return GetBlogById(updatedBlog.Id);
      // this will return an object of type Blog, right? Yes, it worked!!!!
    }

    internal void DeleteBlog(int id)
    {
      string sql = "DELETE FROM blogs WHERE id = @id LIMIT 1";
      _db.Execute(sql, new { id });
    }
  }
}