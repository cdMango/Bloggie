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
        return await bloggieDbContext.BlogPosts.Include(nameof(BlogPost.Tags)).FirstOrDefaultAsync(x => x.Id == id); 
    }

    public async Task<BlogPost> AddAsync(BlogPost blogPost)
    {
        await bloggieDbContext.BlogPosts.AddAsync(blogPost);
        await bloggieDbContext.SaveChangesAsync();
        return blogPost;
    }
    public async Task<BlogPost> GetAsync(string urlHandle)
    {
        return await bloggieDbContext.BlogPosts.Include(nameof(BlogPost.Tags)).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle); 

    }

    public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
    {
        var exisitingBlogPost = await bloggieDbContext.BlogPosts.Include(nameof(BlogPost.Tags)).
            FirstOrDefaultAsync(x => x.Id == blogPost.Id);

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
            
            // Delete the exisiting tags
            if (blogPost.Tags != null && blogPost.Tags.Any())
            {
                //delete exisiting tags
                bloggieDbContext.Tags.RemoveRange(exisitingBlogPost.Tags);
                
                //add new tags
                blogPost.Tags.ToList().ForEach(x => x.BlogPostId = exisitingBlogPost.Id);
                await bloggieDbContext.Tags.AddRangeAsync(); 
                

            }
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