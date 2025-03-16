using Project_Group3_SWD.Models;

namespace Project_Group3_SWD.Services
{
    public interface IBlogService
    {
        public Task<List<Post>> GetPostsAsync(string? search, int? categoryId);
        public Task<List<Post>> Top4PostsNewest();
        public Task<Post> GetPostsById(int id);
    }
}
