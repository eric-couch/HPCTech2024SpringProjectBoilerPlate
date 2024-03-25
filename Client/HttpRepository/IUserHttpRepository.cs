using HPCTech2024SpringProjectBoilerPlate.Shared;
using HPCTech2024SpringProjectBoilerPlate.Shared.Wrapper;

namespace HPCTech2024SpringProjectBoilerPlate.Client.HttpRepository;

public interface IUserHttpRepository
{
    Task<DataResponse<List<OMDBMovie>>> GetUserMovies(string userName);
    Task<bool> DeleteUserMovie(string userName, OMDBMovie movie);
    Task<DataResponse<List<UserEditDto>>> GetAllUsersAsync();
    Task<bool> ToggleAdminUser(string userId);

    //Task AddMovie(string username, Movie movie);
}
