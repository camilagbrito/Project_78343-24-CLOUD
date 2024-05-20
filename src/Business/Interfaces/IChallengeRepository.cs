using Business.Models;

namespace Business.Interfaces
{
    public interface IChallengeRepository : IRepository<Challenge>
    {
        Task<Challenge> GetByDate();
    }
}
