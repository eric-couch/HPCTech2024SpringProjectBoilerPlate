using RichardSzalay.MockHttp;
using HPCTech2024SpringProjectBoilerPlate.Client.HttpRepository;

namespace HPCTech2024SpringProjectBoilerPlate.Client.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    

    [Test]
    public async Task Test_GetMovies_ReturnUserAndFavoriteMovies_Success()
    {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        string testUserResponse = """
                {
                "id": "bcb8f5e8-6645-4d44-a511-4698f4576044",
                "username": "eric.couch@example.net",
                "firstName": null,
                "lastName": null,
                "favoriteMovies": [
                    {
                        "id": 1,
                        "imdbId": "tt0816692"
                    },
                    {
                        "id": 2,
                        "imdbId": "tt0477348"
                    },
                    {
                        "id": 3,
                        "imdbId": "tt2543164"
                    }
                ]
            }
            """;
        string testInterstellarResponse = """
                {
                "Title": "Interstellar",
                "Year": "2014",
                "Rated": "PG-13",
                "Released": "07 Nov 2014",
                "Runtime": "169 min",
                "Genre": "Adventure, Drama, Sci-Fi",
                "Director": "Christopher Nolan",
                "Writer": "Jonathan Nolan, Christopher Nolan",
                "Actors": "Matthew McConaughey, Anne Hathaway, Jessica Chastain",
                "Plot": "When Earth becomes uninhabitable in the future, a farmer and ex-NASA pilot, Joseph Cooper, is tasked to pilot a spacecraft, along with a team of researchers, to find a new planet for humans.",
                "Language": "English",
                "Country": "United States, United Kingdom, Canada",
                "Awards": "Won 1 Oscar. 44 wins & 148 nominations total",
                "Poster": "https://m.media-amazon.com/images/M/MV5BZjdkOTU3MDktN2IxOS00OGEyLWFmMjktY2FiMmZkNWIyODZiXkEyXkFqcGdeQXVyMTMxODk2OTU@._V1_SX300.jpg",
                "Ratings": [
                    {
                        "Source": "Internet Movie Database",
                        "Value": "8.7/10"
                    },
                    {
                        "Source": "Rotten Tomatoes",
                        "Value": "73%"
                    },
                    {
                        "Source": "Metacritic",
                        "Value": "74/100"
                    }
                ],
                "Metascore": "74",
                "imdbRating": "8.7",
                "imdbVotes": "2,071,776",
                "imdbID": "tt0816692",
                "Type": "movie",
                "DVD": "24 May 2016",
                "BoxOffice": "$188,020,017",
                "Production": "N/A",
                "Website": "N/A",
                "Response": "True"
            }
            """;

        string testNoCountryResponse = """
                        {
                "Title": "No Country for Old Men",
                "Year": "2007",
                "Rated": "R",
                "Released": "21 Nov 2007",
                "Runtime": "122 min",
                "Genre": "Crime, Drama, Thriller",
                "Director": "Ethan Coen, Joel Coen",
                "Writer": "Joel Coen, Ethan Coen, Cormac McCarthy",
                "Actors": "Tommy Lee Jones, Javier Bardem, Josh Brolin",
                "Plot": "Violence and mayhem ensue after a hunter stumbles upon the aftermath of a drug deal gone wrong and over two million dollars in cash near the Rio Grande.",
                "Language": "English, Spanish",
                "Country": "United States",
                "Awards": "Won 4 Oscars. 164 wins & 139 nominations total",
                "Poster": "https://m.media-amazon.com/images/M/MV5BMjA5Njk3MjM4OV5BMl5BanBnXkFtZTcwMTc5MTE1MQ@@._V1_SX300.jpg",
                "Ratings": [
                    {
                        "Source": "Internet Movie Database",
                        "Value": "8.2/10"
                    },
                    {
                        "Source": "Rotten Tomatoes",
                        "Value": "93%"
                    },
                    {
                        "Source": "Metacritic",
                        "Value": "92/100"
                    }
                ],
                "Metascore": "92",
                "imdbRating": "8.2",
                "imdbVotes": "1,051,288",
                "imdbID": "tt0477348",
                "Type": "movie",
                "DVD": "21 Apr 2016",
                "BoxOffice": "$74,283,625",
                "Production": "N/A",
                "Website": "N/A",
                "Response": "True"
            }
            """;
        string testArrivalResponse = """
                {
                "Title": "Arrival",
                "Year": "2016",
                "Rated": "PG-13",
                "Released": "11 Nov 2016",
                "Runtime": "116 min",
                "Genre": "Drama, Mystery, Sci-Fi",
                "Director": "Denis Villeneuve",
                "Writer": "Eric Heisserer, Ted Chiang",
                "Actors": "Amy Adams, Jeremy Renner, Forest Whitaker",
                "Plot": "A linguist works with the military to communicate with alien lifeforms after twelve mysterious spacecraft appear around the world.",
                "Language": "Russian, Mandarin, English",
                "Country": "United States, Canada",
                "Awards": "Won 1 Oscar. 69 wins & 268 nominations total",
                "Poster": "https://m.media-amazon.com/images/M/MV5BMTExMzU0ODcxNDheQTJeQWpwZ15BbWU4MDE1OTI4MzAy._V1_SX300.jpg",
                "Ratings": [
                    {
                        "Source": "Internet Movie Database",
                        "Value": "7.9/10"
                    },
                    {
                        "Source": "Rotten Tomatoes",
                        "Value": "94%"
                    },
                    {
                        "Source": "Metacritic",
                        "Value": "81/100"
                    }
                ],
                "Metascore": "81",
                "imdbRating": "7.9",
                "imdbVotes": "763,030",
                "imdbID": "tt2543164",
                "Type": "movie",
                "DVD": "31 Jan 2017",
                "BoxOffice": "$100,546,139",
                "Production": "N/A",
                "Website": "N/A",
                "Response": "True"
            }
            """;
        
        mockHttp.When("https://localhost:7176/api/user?userName=eric.couch@example.net")
            .Respond("application/json", testUserResponse);
        mockHttp.When("https://www.omdbapi.com/?apikey=86c39163&i=tt0816692")
            .Respond("application/json", testInterstellarResponse);
        mockHttp.When("https://www.omdbapi.com/?apikey=86c39163&i=tt0477348")
            .Respond("application/json", testNoCountryResponse);
        mockHttp.When("https://www.omdbapi.com/?apikey=86c39163&i=tt2543164")
            .Respond("application/json", testArrivalResponse);

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri("https://localhost:7176");
        //var userMoviesHttpRepository = new UserMoviesHttpRepository(client);

        //// Act
        //var response = await userMoviesHttpRepository.GetUserMovies("eric.couch@example.net");
        //var movies = response.Data;

        //// Assert 
        //Assert.That(movies.Count, Is.EqualTo(3));
        //Assert.That(movies[0].Title, Is.EqualTo("Interstellar"));
        //Assert.That(movies[1].Title, Is.EqualTo("No Country for Old Men"));
        //Assert.That(movies[2].Title, Is.EqualTo("Arrival"));

    }
}