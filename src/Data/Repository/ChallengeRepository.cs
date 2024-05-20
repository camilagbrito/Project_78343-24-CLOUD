using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class ChallengeRepository : Repository<Challenge>, IChallengeRepository
    {
        public ChallengeRepository(EcomDbContext context) : base(context)
        {
        }

        public async Task<Challenge> GetByDate()
        {
            return await _context.Challenges.AsNoTracking().Where(x => x.Date.Date == DateTime.Now.Date).FirstOrDefaultAsync();
        }

    }
}
