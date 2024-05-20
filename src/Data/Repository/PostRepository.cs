using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(EcomDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Post>> GetPostsUsersAndComments()
        {
        
          return await _context.Posts.AsNoTracking().Include(p => p.User).Include(p => p.Comments).ThenInclude(c => c.User).OrderByDescending(p => p.CreatedDate).ToListAsync();
          
        }

        public async Task<IEnumerable<Post>> GetPostsAndCommentsByUserId(string id)
        {
            return await _context.Posts.AsNoTracking().Where(o => o.UserId == id).Include(p => p.Comments).OrderByDescending(o => o.CreatedDate).ToListAsync();
        }

        public async Task<Post> GetPostandUserById(Guid Id)
        {
            return await _context.Posts.AsNoTracking().Include(p => p.User).FirstOrDefaultAsync(p => p.Id == Id);
        }
    }
}
