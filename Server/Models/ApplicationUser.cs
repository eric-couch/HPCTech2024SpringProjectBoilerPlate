using Microsoft.AspNetCore.Identity;

namespace HPCTech2024SpringProjectBoilerPlate.Server.Models;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
