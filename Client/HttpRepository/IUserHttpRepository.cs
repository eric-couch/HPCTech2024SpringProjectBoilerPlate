using HPCTech2024SpringProjectBoilerPlate.Shared;
using HPCTech2024SpringProjectBoilerPlate.Shared.Wrapper;

namespace HPCTech2024SpringProjectBoilerPlate.Client.HttpRepository;

public interface IUserHttpRepository
{
    Task<DataResponse<List<OMDBMovie>>> GetUserMovies(string userName);
    Task<MovieSearchResult> SearchOMDBApi(string searchTerm, int page);
    Task<bool> DeleteUserMovie(string userName, OMDBMovie movie);
    Task<DataResponse<List<UserEditDto>>> GetAllUsersAsync();
    Task<bool> UpdateUser(UserEditDto user);
    Task<bool> DeleteUser(string userId);
    Task<bool> ToggleAdminUser(string userId);
    Task<bool> ToggleEmailConfirmedUser(string userId);
    //ToggleEmailConfirmedUser
    //Task AddMovie(string username, Movie movie);
}
