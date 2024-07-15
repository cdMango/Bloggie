using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories;

public class BlogPostCommentRespository :IBlogPostCommentRepository
{
    private readonly BloggieDbContext _bloggieDbContext;

    public BlogPostCommentRespository(BloggieDbContext bloggieDbContext)
    {
        _bloggieDbContext = bloggieDbContext;
    }
    public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
    {
        await _bloggieDbContext.BlogPostComment.AddAsync(blogPostComment);
        await _bloggieDbContext.SaveChangesAsync();
        return blogPostComment; 
    }
}