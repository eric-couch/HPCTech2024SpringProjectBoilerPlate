using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Duende.IdentityServer.EntityFramework.Options;
using HPCTech2024SpringProjectBoilerPlate.Server.Models;
using HPCTech2024SpringProjectBoilerPlate.Shared;

namespace HPCTech2024SpringProjectBoilerPlate.Server.Data;

public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
{
    public ApplicationDbContext(
        DbContextOptions options,
        IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
    {
    }

    public DbSet<Movie> Movies => Set<Movie>();

}
