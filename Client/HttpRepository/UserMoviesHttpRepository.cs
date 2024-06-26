﻿using HPCTech2024SpringProjectBoilerPlate.Shared;
using HPCTech2024SpringProjectBoilerPlate.Shared.Wrapper;
using static System.Net.WebRequestMethods;
using System.Net.Http.Json;
using System.Text.Json;
using Blazored.LocalStorage;

namespace HPCTech2024SpringProjectBoilerPlate.Client.HttpRepository;

public class UserMoviesHttpRepository : IUserHttpRepository
{
    private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?apikey=";
    private readonly string OMDBAPIKey = "86c39163";
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;

    public UserMoviesHttpRepository(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorageService = localStorage;
    }

    public async Task<MovieSearchResult> SearchOMDBApi(string searchTerm, int page)
    {
        var searchResults = await _httpClient.GetFromJsonAsync<MovieSearchResult>($"{OMDBAPIUrl}{OMDBAPIKey}&s={searchTerm}&page={page}");
        if (searchResults?.Search?.Any() ?? false)
        {
            foreach (var movie in searchResults.Search)
            {
                var movieDetails = await _localStorageService.GetItemAsync<OMDBMovie>(movie.imdbID);
                if (movieDetails == null)
                {
                    movieDetails = await _httpClient.GetFromJsonAsync<OMDBMovie>($"{OMDBAPIUrl}{OMDBAPIKey}&i={movie.imdbID}");
                    await _localStorageService.SetItemAsync(movie.imdbID, movieDetails);
                }
            }
        }
        return searchResults;
    }

    public async Task<bool> ToggleAdminUser(string userId)
    {
        var res = await _httpClient.GetFromJsonAsync<bool>($"api/toggle-admin?userId={userId}");
        return res;
        // add error handling
    }
    public async Task<bool> ToggleEmailConfirmedUser(string userId)
    {
        var res = await _httpClient.GetFromJsonAsync<bool>($"api/toggle-email-confirmed?userId={userId}");
        return res;
        // add error handling
    }
    public async Task<DataResponse<List<UserEditDto>>> GetAllUsersAsync()
    {
        try
        {
            var users = await _httpClient.GetFromJsonAsync<List<UserEditDto>>("api/users");
            return new DataResponse<List<UserEditDto>>()
            {
                Data = users,
                Message = "Success",
                Succeeded = true
            };
        } catch (Exception ex)
        {
            // add logging
            return new DataResponse<List<UserEditDto>>()
            {
                Errors = new Dictionary<string, string[]> { { "Error", new string[] { ex.Message } } },
                Data = new List<UserEditDto>(),
                Message = ex.Message,
                Succeeded = false
            };
        }
    }

    public async Task<bool> UpdateUser(UserEditDto user)
    {
        var res = await _httpClient.PostAsJsonAsync("api/update-user", user);
        if (res.IsSuccessStatusCode) 
        {
            return true;
        }
        return false;
    }

    public async Task<bool> DeleteUser(string userId)
    {
        var res = await _httpClient.GetFromJsonAsync<bool>($"api/delete-user?userId={userId}");
        return res;
    }

    public async Task<bool> DeleteUserMovie(string userName, OMDBMovie movie)
    {
        var newMovie = new Movie()
        {
            imdbId = movie.imdbID
        };
        var res = await _httpClient.PostAsJsonAsync($"api/remove-movie?username={userName}", newMovie);
        if (res.IsSuccessStatusCode)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public async Task<DataResponse<List<OMDBMovie>>> GetUserMovies(string userName)
    {
        try
        {
            var MovieDetails = new List<OMDBMovie>();
            UserDto userDto = await _httpClient.GetFromJsonAsync<UserDto>("api/user?userName=" + userName);
            if (userDto?.FavoriteMovies?.Any() ?? false)
            {
                foreach (var movie in userDto.FavoriteMovies)
                {
                    OMDBMovie oMDBMovie = new OMDBMovie();
                    oMDBMovie = await _localStorageService.GetItemAsync<OMDBMovie>(movie.imdbId);
                    if (oMDBMovie == null)
                    {
                        oMDBMovie = await _httpClient.GetFromJsonAsync<OMDBMovie>($"{OMDBAPIUrl}{OMDBAPIKey}&i={movie.imdbId}");
                        await _localStorageService.SetItemAsync(movie.imdbId, oMDBMovie);
                    }
                    MovieDetails.Add(oMDBMovie);
                }
                return new DataResponse<List<OMDBMovie>>() 
                { 
                    Data = MovieDetails, 
                    Message = "Success",
                    Succeeded = true 
                };
            }
            return new DataResponse<List<OMDBMovie>>()
            {
                Data = MovieDetails,
                Message = "Success",
                Succeeded = true
            };

        }
        catch (NotSupportedException ex)
        {
            Console.WriteLine(ex.Message);
            return new DataResponse<List<OMDBMovie>>()
            {
                Errors = new Dictionary<string, string[]> { { "Not Supported", new string[] { ex.Message } } },
                Data = new List<OMDBMovie>(),
                Message = "Not Supported",
                Succeeded = false
            };
        }
        //catch (HttpRequestException ex)
        //{
        //    Console.WriteLine(ex.Message);
        //}
        //catch (JsonException ex)
        //{
        //    Console.WriteLine(ex.Message);
        //}
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);  // later this will become a log event
            return new DataResponse<List<OMDBMovie>>()
            {
                Errors = new Dictionary<string, string[]> { { "Error", new string[] { ex.Message } } },
                Data = new List<OMDBMovie>(),
                Message = ex.Message,
                Succeeded = false
            };
        }
        
    }
}
