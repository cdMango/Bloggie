using Bloggie.Web.Models.Domain;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages.Blog;

public class Details : PageModel
{
    private readonly IBlogPostRepository _blogPostRepository;
    private readonly IBlogPostLikeRepository _blogPostLikeRepository;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public BlogPost BlogPost { get; set; }
   
    public int TotalLikes { get; set; }

    public bool Liked { get; set; }
    public Details(IBlogPostRepository blogPostRepository, 
        IBlogPostLikeRepository blogPostLikeRepository,
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager)
    {
        _blogPostRepository = blogPostRepository;
        _blogPostLikeRepository = blogPostLikeRepository;
        _signInManager = signInManager;
        _userManager = userManager;
    }
    public async Task<IActionResult> OnGet(string urlHandle)
    {
        BlogPost = await _blogPostRepository.GetAsync(urlHandle);

        if (BlogPost != null)
        {
            if (_signInManager.IsSignedIn(User))
                {
                    var likes = await _blogPostLikeRepository.GetLikesForBlog(BlogPost.Id);
                    var userId = _userManager.GetUserId(User);
                    Liked = likes.Any(x => x.UserId == Guid.Parse(userId)); 
                }

            TotalLikes = await _blogPostLikeRepository.GetTotalLikesForBlog(BlogPost.Id);
            
        }
        return Page(); 
    }
}