using HPCTech2024SpringProjectBoilerPlate.Shared;
using HPCTech2024SpringProjectBoilerPlate.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Duende.IdentityServer.Validation;
using HPCTech2024SpringProjectBoilerPlate.Server.Models;

namespace HPCTech2024SpringProjectBoilerPlate.Server.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserService> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(ApplicationDbContext context, ILogger<UserService> logger, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<UserDto> GetMovies(string userName)
    {
        var movies = await _context.Users.Include(u => u.FavoriteMovies).Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.UserName,
            FirstName = u.FirstName,
            LastName = u.LastName,
            FavoriteMovies = u.FavoriteMovies
        }).FirstOrDefaultAsync(u => u.Username == userName);
        int movieCount = movies.FavoriteMovies.Count;
        _logger.LogInformation("User {userName} retrieving {movieCount} favorite movies.  Logged at {Placeholder:MMMM dd, yyyy}",
                       movies.Username, movieCount, DateTimeOffset.UtcNow);

        return movies;
    }

    public async Task<List<UserEditDto>> GetAllUsers()
    {
        var users = (from u in _context.Users
                     let query = (from ur in _context.Set<IdentityUserRole<string>>()
                                  where ur.UserId.Equals(u.Id)
                                  join r in _context.Roles on ur.RoleId equals r.Id
                                  select r.Name).ToList()
                    select new UserEditDto { 
                        Id = u.Id,
                        UserName = u.UserName,
                        Email = u.Email,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        EmailConfirmed = u.EmailConfirmed,
                        Admin = query.Contains("Admin")
                    }).ToList();
        _logger.LogInformation("retrieving all users.  Logged at {Placeholder:MMMM dd, yyyy}",
                       DateTimeOffset.UtcNow);

        return users;

    }


    public async Task<bool> ToggleAdminService(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return false;
        }
        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Contains("Admin"))
        {
            await _userManager.RemoveFromRoleAsync(user, "Admin");
        }
        else
        {
            await _userManager.AddToRoleAsync(user, "Admin");
        }
        return true;
    }
}
