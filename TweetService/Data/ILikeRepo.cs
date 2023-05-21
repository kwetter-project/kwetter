using TweetService.Models;

namespace TweetService.Data
{
    public interface ILikeRepo
    {
        bool SaveChanges();
        IEnumerable<Like> GetAllLikes();
        Like GetLikeById(int id);
        void CreateLike(Like like);
        void DeleteLike(int id);
    }
}