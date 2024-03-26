using HPCTech2024SpringProjectBoilerPlate.Shared;

namespace HPCTech2024SpringProjectBoilerPlate.Server.Services;

public interface IUserService
{
    Task<UserDto> GetMovies(string userName);
    Task<List<UserEditDto>> GetAllUsers();
    Task<bool> UpdateUser(UserEditDto user);
    Task<bool> DeleteUser(string userId);
    Task<bool> ToggleAdminService(string userId);
    Task<bool> ToggleEmailConfirmedService(string userId);
}
