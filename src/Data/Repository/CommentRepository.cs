using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(EcomDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Comment>> GetCommentsAndUserByPostId(Guid id)
        {
            return await _context.Comments.AsNoTracking().Where(c => c.PostId == id).Include(p => p.User).OrderByDescending(p => p.CreatedDate).ToListAsync();
           
        }
    }
}
