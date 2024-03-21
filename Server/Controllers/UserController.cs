using Microsoft.AspNetCore.Mvc;
using HPCTech2024SpringProjectBoilerPlate.Shared;
using HPCTech2024SpringProjectBoilerPlate.Server.Data;
using HPCTech2024SpringProjectBoilerPlate.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace HPCTech2024SpringProjectBoilerPlate.Server.Controllers;

// call to the server (http://localhost:xxxx/api/user) will call some method in here
// if that call is specific like (http://localhost:xxxx/api/user/) or (http://localhost:xxxx/api/add-movie) calls the appropriate method
public class UserController : Controller
{

    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser>  _userManager;

    public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    [Route("api/user")]
    public async Task<UserDto> GetUserMovies(string userName)
    {
        var movies = await _context.Users.Include(u => u.FavoriteMovies).Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.UserName,
            FirstName = u.FirstName,
            LastName = u.LastName,
            FavoriteMovies = u.FavoriteMovies
        }).FirstOrDefaultAsync(u => u.Username == userName);

        return movies;
    }

    [HttpPost]
    [Route("api/add-movie")]
    public async Task<IActionResult> AddMovie(string username, [FromBody] Movie movie)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
        {
            return NotFound();
        } else
        {
            user.FavoriteMovies.Add(movie);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }

    [HttpPost]
    [Route("api/remove-movie")]
    public async Task<IActionResult> RemoveFavoriteMovie(string username, [FromBody] Movie movie)
    {
        try
        {
            var movietoRemove = _context.Users.Include(u => u.FavoriteMovies)
                                .FirstOrDefault(u => u.UserName == username)
                                .FavoriteMovies.FirstOrDefault(m => m.imdbId == movie.imdbId);
            
            _context.Users.FirstOrDefault(u => u.UserName == username)
                            .FavoriteMovies.Remove(movietoRemove);
            _context.SaveChanges();
            return Ok();
        } catch (Exception e)
        {
            return NotFound();
        }
        
    }

    [HttpGet]
    [Route("api/get-roles/{id}")]
    [Authorize(Roles ="Admin")]
    public async Task<List<string>> GetRoles(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            return null;
        }
        var roles = await _userManager.GetRolesAsync(user);
        return roles.ToList();
    }
}
