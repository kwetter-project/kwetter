using UserService.Models;

namespace UserService.Data
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _context;

        public UserRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            _context.Users.Add(user);
        }

        public void DeleteUser(string name)
        {
            _context.Remove(_context.Users.Single(a => a.Username == name));
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserById(string name)
        {
            return _context.Users.FirstOrDefault
            (p => p.Username == name);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void updatePassword(User user)
        {

            _context.Users.Attach(user);
            var entry = _context.Users.Entry(user);
            entry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;


        }

        public void updateUserToken(User user)
        {

            _context.Users.Attach(user);
            var entry = _context.Users.Entry(user);
            entry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public bool userExist(string name)
        {
            return _context.Users.Any(p => p.Username == name);
        }

        private string DetermineShardKey(string username)
        {
            // Define ranges and their corresponding shard keys
            var range1 = new { Start = "A", End = "F", ShardKey = "Shard1" };
            var range2 = new { Start = "G", End = "M", ShardKey = "Shard2" };
            var range3 = new { Start = "N", End = "Z", ShardKey = "Shard3" };

            // Determine the range based on the first character of the username
            var firstChar = username.Substring(0, 1).ToUpper();

            // Assign the corresponding shard key based on the range
            var shardKey = string.Empty;
            if (string.Compare(firstChar, range1.Start, StringComparison.OrdinalIgnoreCase) >= 0 &&
                string.Compare(firstChar, range1.End, StringComparison.OrdinalIgnoreCase) <= 0)
            {
                shardKey = range1.ShardKey;
            }
            else if (string.Compare(firstChar, range2.Start, StringComparison.OrdinalIgnoreCase) >= 0 &&
                     string.Compare(firstChar, range2.End, StringComparison.OrdinalIgnoreCase) <= 0)
            {
                shardKey = range2.ShardKey;
            }
            else if (string.Compare(firstChar, range3.Start, StringComparison.OrdinalIgnoreCase) >= 0 &&
                     string.Compare(firstChar, range3.End, StringComparison.OrdinalIgnoreCase) <= 0)
            {
                shardKey = range3.ShardKey;
            }
            else
            {
                // Handle cases where the first character doesn't fall into any defined range
                shardKey = "DefaultShard";
            }

            return shardKey;
        }

    }
}