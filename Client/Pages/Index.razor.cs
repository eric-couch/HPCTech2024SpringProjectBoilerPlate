using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using HPCTech2024SpringProjectBoilerPlate.Shared;
using HPCTech2024SpringProjectBoilerPlate.Shared.Wrapper;
using System.Net.Http.Json;
using HPCTech2024SpringProjectBoilerPlate.Client.HttpRepository;
using Syncfusion.Blazor.Notifications;

namespace HPCTech2024SpringProjectBoilerPlate.Client.Pages;

public partial class Index
{
    [Inject]
    AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject]
    public IUserHttpRepository UserMoviesHttpRepository { get; set; }

    public List<OMDBMovie> UserFavoriteMovies { get; set; } = new();
    public OMDBMovie Movie { get; set; }

    public SfToast ToastObj;
    private string? toastContent = string.Empty;
    private string? toastSuccess = "e-toast-success";
    protected override async Task OnInitializedAsync()
    {
        var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
        if (UserAuth is not null && UserAuth.IsAuthenticated)
        {
            DataResponse<List<OMDBMovie>> dataResponse = await UserMoviesHttpRepository.GetUserMovies(UserAuth.Name);
            if (dataResponse.Succeeded)
            {
                UserFavoriteMovies = dataResponse.Data;
            } else
            {
                toastContent = "Error retrieving user favorite movies";
                toastSuccess = "e-toast-danger";
                await ToastObj.ShowAsync();
            }
        }
    }

    private async Task ShowMovieDetails(OMDBMovie movie)
    {
        Movie = movie;
        await Task.CompletedTask;
    }

    private async Task RemoveMovie(OMDBMovie movie)
    {
        var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
        if (UserAuth is not null && UserAuth.IsAuthenticated)
        {
            var res = await UserMoviesHttpRepository.DeleteUserMovie(UserAuth.Name, movie);
            UserFavoriteMovies.Remove(movie);
            StateHasChanged();

        }
    }
}
