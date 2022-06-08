using System.Net;
using CloudApi.Controllers;
using CloudApi.Data;
using CloudApi.Repository;
using CommonData.Model.Entity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CloudApi.Test.Unit.Controllers;

public class UnitTest1
{
    [Fact]
    public void DeleteHome_ShouldReturnBadRequestWhenHomeCannotBeDeleted()
    {
        // Arrange
        var mockedPersonRepository = new Mock<PersonRepository>();
        mockedPersonRepository
            .Setup(r => r.Find(1))
            .Returns(new Person(1, "TestUser", "testuser@example.com"))
            ;

        var mockedHomeRepository = new Mock<HomeRepository>();
        mockedHomeRepository
            .Setup(repo => repo.Find(1))
            .Returns((Home?)null)
            ;

        //var mockDbContext = new Mock<UroContext>();
        
        var homeController = new HomeController(
            null,
            /*mockDbContext.Object,*/
            mockedHomeRepository.Object,
            mockedPersonRepository.Object
        );

        // Act
        var result = homeController.DeleteHome(1) as StatusCodeResult;

        var actual = result?.StatusCode;
        var expected = (int)HttpStatusCode.BadRequest;
        
        // Assert
        Assert.Equal(expected, actual);
    }
}