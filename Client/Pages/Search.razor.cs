﻿using HPCTech2024SpringProjectBoilerPlate.Shared;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;

namespace HPCTech2024SpringProjectBoilerPlate.Client.Pages;

public partial class Search
{
    [Inject]
    HttpClient Http { get; set; }

    private string searchTerm;
    private MovieSearchResult searchResults = new MovieSearchResult();
    private List<MovieSearchResultItem> OMDBMovies { get; set; }
    int TotalItems = 0;
    int page = 1;
    private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?apikey=";
    private readonly string OMDBAPIKey = "86c39163";
    public SfGrid<MovieSearchResultItem> movieGrid;
    public SfPager Page;


    public async Task PageClick(PagerItemClickEventArgs args)
    {
        page = args.CurrentPage;
        await SearchOMDB();
    }

    private async Task SearchOMDB()
    {
        try
        {
            searchResults = await Http.GetFromJsonAsync<MovieSearchResult>($"{OMDBAPIUrl}{OMDBAPIKey}&s={searchTerm}&page={page}");
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