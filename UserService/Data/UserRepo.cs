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
    }
}