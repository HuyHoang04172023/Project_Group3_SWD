using A_LIÊM_SHOP.Models;
using X.PagedList;

namespace A_LIÊM_SHOP.ViewModels
{
    public class BlogViewModel
    {
        public IPagedList<Post> Blogs { get; set; } = default!;
        public List<Category> Categories { get; set; }
        public List<Post> Top4 { get; set; }
        public string? Search { get; set; }
        public int? CategoryId { get; set; }
        public int CurrentPage { get; set; } = 1;
    }
}
