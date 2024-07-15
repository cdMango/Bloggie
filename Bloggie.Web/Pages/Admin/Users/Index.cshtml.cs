using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages.Admin.Users;

public class Index : PageModel
{
    private readonly IUserRepository _userRepository;

    public List<User> Users { get; set; }
    public Index(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<IActionResult> OnGet()
    {
        var users = await _userRepository.GetAll();
        Users = new List<User>(); 
        foreach (var user in users)
        {
            Users.Add(new Models.ViewModels.User()
            {
                Id = Guid.Parse(user.Id),
                Username = user.UserName,
                Email = user.Email
            });
        }

        return Page(); 
    }
}