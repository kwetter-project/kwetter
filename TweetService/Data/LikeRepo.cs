using TweetService.Models;

namespace TweetService.Data
{
    public class LikeRepo : ILikeRepo
    {
        private readonly AppDbContext _context;

        public LikeRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreateLike(Like like)
        {
            if (like == null)
            {
                throw new ArgumentNullException(nameof(like));
            }
            _context.Likes.Add(like);
        }

        public void DeleteLike(int id)
        {
            _context.Remove(_context.Likes.Single(a => a.Id == id));
        }

        public IEnumerable<Like> GetAllLikes()
        {
            return _context.Likes.ToList();
        }

        public Like GetLikeById(int id)
        {
            return _context.Likes.FirstOrDefault
            (p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}