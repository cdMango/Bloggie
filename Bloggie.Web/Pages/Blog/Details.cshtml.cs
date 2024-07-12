using Bloggie.Web.Models.Domain;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages.Blog;

public class Details : PageModel
{
    private readonly IBlogPostRepository _blogPostRepository;
    private readonly IBlogPostLikeRepository _blogPostLikeRepository;

    public BlogPost BlogPost { get; set; }

    public int TotalLikes { get; set; }
    public Details(IBlogPostRepository blogPostRepository, 
        IBlogPostLikeRepository blogPostLikeRepository)
    {
        _blogPostRepository = blogPostRepository;
        _blogPostLikeRepository = blogPostLikeRepository;
    }
    public async Task<IActionResult> OnGet(string urlHandle)
    {
        BlogPost = await _blogPostRepository.GetAsync(urlHandle);

        if (BlogPost != null)
        {
           TotalLikes =  await _blogPostLikeRepository.GetTotalLikesForBlog(BlogPost.Id); 
        }
        return Page(); 
    }
}