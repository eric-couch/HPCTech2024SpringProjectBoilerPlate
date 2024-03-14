using Microsoft.AspNetCore.Identity;
using HPCTech2024SpringProjectBoilerPlate.Shared;

namespace HPCTech2024SpringProjectBoilerPlate.Server.Models;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public List<Movie> FavoriteMovies { get; set; } = new List<Movie>();
}
