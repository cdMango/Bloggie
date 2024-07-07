using Bloggie.Web.Models.Domain;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IBlogPostRepository _blogPostRepository;
    
    public List<BlogPost> Blogs { get; set; }
    public IndexModel(ILogger<IndexModel> logger, IBlogPostRepository blogPostRepository)
    {
        _logger = logger;
        _blogPostRepository = blogPostRepository;
    }

    public async Task<IActionResult>  OnGet()
    {
        Blogs =(await _blogPostRepository.GetAllAsync()).ToList();
        return Page(); 
    }
}
