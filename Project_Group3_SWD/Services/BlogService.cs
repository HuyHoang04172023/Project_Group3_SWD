using Project_Group3_SWD.Models;
using Project_Group3_SWD.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Project_Group3_SWD.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public async Task<List<Post>> GetPostsAsync(string? search, int? categoryId)
        {
            return await _blogRepository.GetPostsAsync(search, categoryId);
        }

        public async Task<Post> GetPostsById(int id)
        {
            return await _blogRepository.GetPostsById(id);
        }

        public async Task<List<Post>> Top4PostsNewest()
        {
            return await _blogRepository.Top4PostsNewest();
        }
    }
}
