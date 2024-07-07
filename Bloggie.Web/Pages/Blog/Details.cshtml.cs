using Bloggie.Web.Models.Domain;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages.Blog;

public class Details : PageModel
{
    private readonly IBlogPostRepository _blogPostRepository;

    public BlogPost BlogPost { get; set; }

    public Details(IBlogPostRepository blogPostRepository)
    {
        _blogPostRepository = blogPostRepository;
    }
    public async Task<IActionResult> OnGet(string urlHandle)
    {
        BlogPost =await _blogPostRepository.GetAsync(urlHandle);
        return Page(); 
    }
}