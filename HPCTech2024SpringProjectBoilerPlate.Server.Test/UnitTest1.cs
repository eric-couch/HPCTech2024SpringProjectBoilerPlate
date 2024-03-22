using HPCTech2024SpringProjectBoilerPlate.Server.Services;
using HPCTech2024SpringProjectBoilerPlate.Shared;
using HPCTech2024SpringProjectBoilerPlate.Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace HPCTech2024SpringProjectBoilerPlate.Server.Test;

public class Tests
{

    private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetMovies_ShouldReturnUserDto_WhenUserExists()
    {
        // Arrange
        UserDto returnUser = new UserDto
        {
            Id = "bcb8f5e8-6645-4d44-a511-4698f4576044",
            Username = "eric.couch@example.net",
            FirstName = null,
            LastName = null,
            FavoriteMovies = new List<Movie>() {
                new Movie()
                {
                        Id = 1,
                  imdbId = "tt0816692"
                },
                new Movie()
                {
                        Id = 2,
                  imdbId = "tt0477348"
                },
                new Movie()
                {
                        Id = 3,
                  imdbId = "tt2543164"
                }
            }
        };
        string? userName = "eric.couch@example.net";
        _userServiceMock.Setup(x => x.GetMovies(userName)).ReturnsAsync(returnUser);

        var userController = new UserController(_userServiceMock.Object);
        // Act
        var response = await userController.GetUserMovies(userName);
        //var result = (OkObjectResult)response.
        //Assert.That(result.StatusCode, Is.EqualTo(200));

        // Assert 
        Assert.That(response, Is.TypeOf<UserDto>());
        Assert.That(response.Username, Is.EqualTo("eric.couch@example.net"));
        Assert.That(response.FavoriteMovies.Count, Is.EqualTo(3));
        Assert.That(response.FavoriteMovies[0].imdbId, Is.EqualTo("tt0816692"));    

    }
}