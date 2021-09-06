using System.Data;
using Blogger.Models;
using Dapper;

namespace Blogger.Repositories
{
    public class AccountsRepository
    {
        private readonly IDbConnection _db;

        public AccountsRepository(IDbConnection db)
        {
            _db = db;
        }

        internal Account GetByEmail(string userEmail)
        {
            string sql = "SELECT * FROM accounts WHERE email = @userEmail";
            return _db.QueryFirstOrDefault<Account>(sql, new { userEmail });
        }

        internal Account GetById(string id)
        {
            string sql = "SELECT * FROM accounts WHERE id = @id";
            return _db.QueryFirstOrDefault<Account>(sql, new { id });
        }

        internal Account Create(Account newAccount)
        {
            string sql = @"
            INSERT INTO accounts
              (name, picture, email, id)
            VALUES
              (@Name, @Picture, @Email, @Id)";
            _db.Execute(sql, newAccount);
            return newAccount;
        }
// REVIEW I did not write this - already in template.  Does it need to be changed to profiles in order to work? (instead of accounts) Because account only has email on it - the profile has name, picture  Does another table need to be created called profiles?  He did not show us this in class.
        internal Account Edit(Account update)
        {
            string sql = @"
            UPDATE accounts
            SET 
              name = @Name,
              picture = @Picture
            WHERE id = @Id;";
            _db.Execute(sql, update);
            return update;
        }
    }
}
