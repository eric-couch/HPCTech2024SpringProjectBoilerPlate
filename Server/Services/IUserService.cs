using HPCTech2024SpringProjectBoilerPlate.Shared;

namespace HPCTech2024SpringProjectBoilerPlate.Server.Services;

public interface IUserService
{
    Task<UserDto> GetMovies(string userName);
}
