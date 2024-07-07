using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories;

public class BlogPostRepository : IBlogPostRepository
{
    private IBlogPostRepository _blogPostRepositoryImplementation;
    private readonly BloggieDbContext bloggieDbContext; 

    public BlogPostRepository(BloggieDbContext bloggieDbContext)
    {
        this.bloggieDbContext = bloggieDbContext; 
    }
    
    public async Task<IEnumerable<BlogPost>> GetAllAsync()
    {
        return await bloggieDbContext.BlogPosts.ToListAsync(); 
    }

    
    public async Task<BlogPost> GetAsync(Guid id)
    {
        return await bloggieDbContext.BlogPosts.Include(nameof(BlogPost.Tags)).FirstOrDefaultAsync(); 
    }

    public async Task<BlogPost> AddAsync(BlogPost blogPost)
    {
        await bloggieDbContext.BlogPosts.AddAsync(blogPost);
        await bloggieDbContext.SaveChangesAsync();
        return blogPost;
    }
    public async Task<BlogPost> GetAsync(string urlHandle)
    {
        return await bloggieDbContext.BlogPosts.FirstOrDefaultAsync(x => x.UrlHandle == urlHandle); 

    }

    public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
    {
        var exisitingBlogPost = await bloggieDbContext.BlogPosts.FindAsync(blogPost.Id);

        if (exisitingBlogPost != null)
        {
            exisitingBlogPost.Heading = blogPost.Heading;
            exisitingBlogPost.PageTitle = blogPost.PageTitle;
            exisitingBlogPost.Content = blogPost.Content;
            exisitingBlogPost.ShortDescription = blogPost.ShortDescription;
            exisitingBlogPost.FeaturedImageURL = blogPost.FeaturedImageURL;
            exisitingBlogPost.UrlHandle = blogPost.UrlHandle;
            exisitingBlogPost.PublishedDateTime = blogPost.PublishedDateTime;
            exisitingBlogPost.Author = blogPost.Author;
            exisitingBlogPost.Visible = blogPost.Visible; 
        }

        await bloggieDbContext.SaveChangesAsync();
        return exisitingBlogPost; 
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existingBlog = await bloggieDbContext.BlogPosts.FindAsync(id);

        if (existingBlog != null)
        {
            bloggieDbContext.BlogPosts.Remove(existingBlog);
            await bloggieDbContext.SaveChangesAsync();
            return true; 
        }

        return false; 
    }
}