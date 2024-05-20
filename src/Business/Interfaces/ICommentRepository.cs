using Business.Models;

namespace Business.Interfaces
{
    public interface ICommentRepository: IRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetCommentsAndUserByPostId(Guid id);

    }
}
