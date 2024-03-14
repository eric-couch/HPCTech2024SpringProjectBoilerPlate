using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using HPCTech2024SpringProjectBoilerPlate.Shared;
using System.Net.Http.Json;

namespace HPCTech2024SpringProjectBoilerPlate.Client.Pages;

public partial class Index
{
    [Inject]
    AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject]
    HttpClient Http { get; set; }

    private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?apikey=";
    private readonly string OMDBAPIKey = "86c39163";
    public List<OMDBMovie> UserFavoriteMovies { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
        if (UserAuth is not null && UserAuth.IsAuthenticated)
        {
            UserDto userDto = await Http.GetFromJsonAsync<UserDto>("api/user?userName=" + UserAuth.Name);
            if (userDto?.FavoriteMovies?.Any() ?? false)
            {
                foreach (var movie in userDto.FavoriteMovies)
                {
                    OMDBMovie oMDBMovie = await Http.GetFromJsonAsync<OMDBMovie>($"{OMDBAPIUrl}{OMDBAPIKey}&i={movie.imdbId}");
                    UserFavoriteMovies.Add(oMDBMovie);
                }
            }
        }
    }
}
