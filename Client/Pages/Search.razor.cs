using HPCTech2024SpringProjectBoilerPlate.Shared;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Notifications;
using Microsoft.AspNetCore.Components.Authorization;
using HPCTech2024SpringProjectBoilerPlate.Client.HttpRepository;

namespace HPCTech2024SpringProjectBoilerPlate.Client.Pages;

public partial class Search
{
    [Inject]
    HttpClient Http { get; set; }
    [Inject]
    AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject]
    IUserHttpRepository UserHttpRepository { get; set; }

    private string searchTerm;
    private MovieSearchResult searchResults = new MovieSearchResult();
    private List<MovieSearchResultItem> OMDBMovies { get; set; }
    int TotalItems = 0;
    int page = 1;
    public SfGrid<MovieSearchResultItem> movieGrid;
    public SfPager Page;
    public SfToast ToastObj;
    private string? toastContent = string.Empty;
    private string? toastSuccess = "e-toast-success";

    //private List<MovieSearchResultItem> SelectedMovies { get; set; } = new List<MovieSearchResultItem>();
    private MovieSearchResultItem SelectedMovie { get; set; }


    public async Task PageClick(PagerItemClickEventArgs args)
    {
        page = args.CurrentPage;
        await SearchOMDB();
    }

    public async Task GetSelectedRows(RowSelectEventArgs<MovieSearchResultItem> args)
    {
        SelectedMovie = args.Data;
    }

    public async Task ToolbarClickHandler(ClickEventArgs args)
    {
        if (args.Item.Id == "GridMovieAdd")
        {
            if (SelectedMovie is not null)
            {
                Movie newMovie = new Movie
                {
                    imdbId = SelectedMovie.imdbID
                };

                var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
                if (UserAuth is not null && UserAuth.IsAuthenticated)
                {
                    var res = await Http.PostAsJsonAsync($"api/add-movie?username={UserAuth.Name}", newMovie);
                    if (res.IsSuccessStatusCode)
                    {
                        toastContent = $"{SelectedMovie.Title} added to your favorites!";
                        StateHasChanged();
                        await ToastObj.ShowAsync();
                    }
                    else
                    {
                        toastContent = $"Failed to add movie {SelectedMovie.Title} to your favorites!  Something went wrong";
                        toastSuccess = "e-toast-danger";
                        StateHasChanged();
                        await ToastObj.ShowAsync();
                    }
                } else
                {
                    toastContent = $"Login to add movies to your collection of favorites.";
                    toastSuccess = "e-toast-danger";
                    StateHasChanged();
                    await ToastObj.ShowAsync();
                }
            } else
            {
                toastContent = $"Please select a movie";
                toastSuccess = "e-toast-warning";
                StateHasChanged();
                await ToastObj.ShowAsync();
            }
        }
    }

    private async Task SearchOMDB()
    {
        try
        {
            //searchResults = await Http.GetFromJsonAsync<MovieSearchResult>($"{OMDBAPIUrl}{OMDBAPIKey}&s={searchTerm}&page={page}");
            searchResults = await UserHttpRepository.SearchOMDBApi(searchTerm, page);
            if (searchResults is not null)
            {
                OMDBMovies = searchResults.Search.ToList();
                TotalItems = int.Parse(searchResults.totalResults);
                StateHasChanged();
            }
            else
            {
                // log no results
                // display no results to user in toast or in grid message 
            }
        }
        catch (Exception e)
        {
            // unpack errors to display to user (user friendly message = "something went wrong")
            // log errors
            Console.WriteLine(e.Message);
        }
    }
}
