using Business.Models;

namespace Business.Interfaces
{
    public interface IPostRepository: IRepository<Post>
    {
        Task<IEnumerable<Post>> GetPostsUsersAndComments();

        Task<IEnumerable<Post>> GetPostsAndCommentsByUserId(string id);

        Task<Post> GetPostandUserById(Guid Id);
    }
}
