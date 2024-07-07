using System.Data;
using System.Text.Json;
using Bloggie.Web.Data;
using Bloggie.Web.Enums;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Bloggie.Web.Pages.Admin.Blogs;

public class EditModel : PageModel
{
    private readonly IBlogPostRepository blogPostRepository;

    private readonly BloggieDbContext bloggieDbContext;
    
    [BindProperty]
    public BlogPost BlogPost { get; set; }
    
    [BindProperty]
    public IFormFile FeaturedImage { get; set; }

    [BindProperty]
    public string Tags { get; set; }    
    public EditModel(IBlogPostRepository blogPostRepository)
    {
        this.blogPostRepository = blogPostRepository;
    }
    public async Task OnGet(Guid id)
    {
        BlogPost = await blogPostRepository.GetAsync(id); 
    }

    public async Task<IActionResult> OnPostEdit()
    {
        try
        {
            
            await blogPostRepository.UpdateAsync(BlogPost);

            ViewData["Notification"] = new Notification
            {
                Message = "Record Updated Successfully",
                Type = Enums.NotificationType.Success
            };
        }
        catch (Exception ex)
        {
            ViewData["Notification"] = new Notification
            {
                Type = Enums.NotificationType.Error,
                Message = "Something Went Wrong (Written by me)"
            };
            
        }

        //return RedirectToPage("/Admin/Blogs/List"); 
        return Page();
    }

    public async Task<IActionResult> OnPostDelete()
    {
        //creates an domain model 
        var deleted = await blogPostRepository.DeleteAsync(BlogPost.Id);

        if (deleted)
        {
            var notification = new Notification
            {
                Type = Enums.NotificationType.Success,
                Message = "Blog Deleted (Written by me)"
            };
        
            //What the fuck is this 
            TempData["Notification"] =  JsonSerializer.Serialize(notification);
            
            return RedirectToPage("/admin/blogs/list");
        }
        return Page(); 
    }
}