using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
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
    private readonly IBlogPostCommentRepository _blogPostCommentRepository;

    public List<BlogComment> Comments { get; set; }
    public BlogPost BlogPost { get; set; }
   
    public int TotalLikes { get; set; }

    public bool Liked { get; set; }
    
    [BindProperty]
    public Guid BlogPostId { get; set; }
    
    
    [BindProperty]
    public string CommentDescription { get; set; }
    


    public Details(IBlogPostRepository blogPostRepository, 
        IBlogPostLikeRepository blogPostLikeRepository,
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        IBlogPostCommentRepository blogPostCommentRepository)
    {
        _blogPostRepository = blogPostRepository;
        _blogPostLikeRepository = blogPostLikeRepository;
        _signInManager = signInManager;
        _userManager = userManager;
        _blogPostCommentRepository = blogPostCommentRepository;
    }
    public async Task<IActionResult> OnGet(string urlHandle)
    {
        BlogPost = await _blogPostRepository.GetAsync(urlHandle);

        if (BlogPost != null)
        {
            BlogPostId = BlogPost.Id; 
            if (_signInManager.IsSignedIn(User))
                {
                    var likes = await _blogPostLikeRepository.GetLikesForBlog(BlogPost.Id);
                    var userId = _userManager.GetUserId(User);
                    Liked = likes.Any(x => x.UserId == Guid.Parse(userId));
                    await GetComments(); 
                    var blogPostComments =  await _blogPostCommentRepository.GetAllAsync(BlogPost.Id); 
                }

            TotalLikes = await _blogPostLikeRepository.GetTotalLikesForBlog(BlogPost.Id);
            
        }
        return Page(); 
    }

    public async Task<IActionResult> OnPost(string urlHandle)
    {
        if(_signInManager.IsSignedIn(User) && !string.IsNullOrWhiteSpace(CommentDescription)){

            var userId = _userManager.GetUserId(User);
            var comment = new BlogPostComment()
            {

                BlogPostid = BlogPostId,
                Description = CommentDescription,
                DateAdded = DateTime.Now,
                UserId = Guid.Parse(userId)
            };
            await _blogPostCommentRepository.AddAsync(comment);

        }
        return RedirectToPage("/Blog/Details", new { urlHandle = urlHandle });
    }

    private async Task GetComments()
    {
        var blogPostComments = await _blogPostCommentRepository.GetAllAsync((BlogPost.Id));

        var blogCommentsViewModel = new List<BlogComment>(); 
        foreach (var blogPostComment in blogPostComments)
        {
            blogCommentsViewModel.Add(new BlogComment
            {
                DateAdded = blogPostComment.DateAdded, 
                Description = blogPostComment.Description, 
                Username = (await _userManager.FindByIdAsync(blogPostComment.UserId.ToString())).UserName
            });
        }

        Comments = blogCommentsViewModel; 
    }
}