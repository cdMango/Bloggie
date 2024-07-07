using System.Text.Json;
using Bloggie.Web.Data;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Repositories;

namespace Bloggie.Web.Pages.Admin.Blogs;

public class AddModel : PageModel
{
    public IBlogPostRepository BlogPostRepository { get; }

    //read only DBcontext used for seedeing purposes
    private readonly IBlogPostRepository blogPostRepository; 
    
    //Binding domainmodels, using AddBlogPost
    [BindProperty]
    public AddBlogPost AddBlogPostRequest { get; set; }

    [BindProperty]
    public IFormFile FeaturedImage { get; set; }
    //Default Constructor

    [BindProperty]
    public string Tags { get; set; }
    public AddModel(IBlogPostRepository blogPostRepository)
    {
        this.blogPostRepository = blogPostRepository; 
    }
    
    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPost()
    {

        //Published Datetime varaible is different from the legacy code, should be PublishedDate
        var blogpost = new BlogPost()
        {
            Heading = AddBlogPostRequest.Heading,
            PageTitle = AddBlogPostRequest.PageTitle,
            Content = AddBlogPostRequest.Content,
            ShortDescription = AddBlogPostRequest.ShortDescription,
            FeaturedImageURL = AddBlogPostRequest.FeaturedImageURL,
            UrlHandle = AddBlogPostRequest.UrlHandle,
            PublishedDateTime = AddBlogPostRequest.PublishedDateTime,
            Author = AddBlogPostRequest.Author,
            Visible = AddBlogPostRequest.Visible,
            Tags = new List<Tag>(Tags.Split(',').Select(x => new Tag(){Name = x.Trim()})) 
        };

        await blogPostRepository.AddAsync(blogpost);

        var notification = new Notification
        {
            Type = Enums.NotificationType.Success,
            Message = "New Blog Created (Written by me)"
        };
        
        //What the fuck is this 
        TempData["Notification"] =  JsonSerializer.Serialize(notification);
        return RedirectToPage("/Admin/Blogs/List");
    }
}