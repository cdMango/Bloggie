using Bloggie.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories;

public class BlogPostLikeRepository : IBlogPostLikeRepository
{
    private readonly BloggieDbContext _bloggieDbContext;

    public BlogPostLikeRepository(BloggieDbContext bloggieDbContext)
    {
        _bloggieDbContext = bloggieDbContext;
    }
    
    public async Task<int> GetTotalLikesForBlog(Guid blogPostId)
    {
        return await _bloggieDbContext.BlogPostLike.CountAsync(x => x.BlogPostId == blogPostId); 
    }
}