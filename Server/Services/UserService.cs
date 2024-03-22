using HPCTech2024SpringProjectBoilerPlate.Shared;
using HPCTech2024SpringProjectBoilerPlate.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace HPCTech2024SpringProjectBoilerPlate.Server.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserService> _logger;

    public UserService(ApplicationDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
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
}
