using A_LIÊM_SHOP.Models;

namespace A_LIÊM_SHOP.Repositories
{
    public interface IBlogRepository
    {
        public Task<List<Post>> GetPostsAsync(string? search, int? categoryId);
        public Task<List<Post>> Top4PostsNewest();
        public Task<Post> GetPostsById(int id);
    }
}
