﻿using Business.Interfaces;
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
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(EcomDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Post>> GetPostsUsersAndComments()
        {
        
          return await _context.Posts.AsNoTracking().Include(p => p.User).Include(p => p.Comments).ToListAsync();
          
        }
    }
}