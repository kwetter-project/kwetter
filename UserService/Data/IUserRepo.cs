using UserService.Models;

namespace UserService.Data
{
    public interface IUserRepo
    {
        bool SaveChanges();
        IEnumerable<User> GetAllUsers();
        User GetUserById(string name);
        void CreateUser(User user);
        void DeleteUser(string name);
        void updatePassword(User user);
        void updateUserToken(User user);
        bool userExist(string name);
    }
}