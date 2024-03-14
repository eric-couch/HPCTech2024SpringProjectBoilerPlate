using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCTech2024SpringProjectBoilerPlate.Shared;

public class UserDto
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public List<Movie> FavoriteMovies { get; set; } = new();
}
