namespace Bloggie.Web.Repositories;
using Bloggie.Web.Models.Domain;


public interface ITagRepository
{
    Task<IEnumerable<Tag>> GetAllAsync(); 
}