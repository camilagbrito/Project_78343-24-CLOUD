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
    public class ChallengeRepository : Repository<Challenge>, IChallengeRepository
    {
        public ChallengeRepository(EcomDbContext context) : base(context)
        {
        }

        public async Task<Challenge> GetByDate()
        {
            return await _context.Challenges.AsNoTracking().Where(x => x.CreatedDate.Date == DateTime.Now.Date).FirstOrDefaultAsync();
        }

    }
}
