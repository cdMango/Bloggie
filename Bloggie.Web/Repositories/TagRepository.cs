using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Page = CloudinaryDotNet.Actions.Page;

namespace Bloggie.Web.Repositories;

public class TagRepository : ITagRepository
{
    private readonly BloggieDbContext _bloggieDbContext;

    public TagRepository(BloggieDbContext bloggieDbContext)
    {
        _bloggieDbContext = bloggieDbContext;
    }
    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        var tags = await _bloggieDbContext.Tags.ToListAsync();

        return tags.DistinctBy(x => x.Name.ToLower()); 
    }
}