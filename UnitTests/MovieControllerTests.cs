using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestingApplication.Controllers;
using TestingApplication.Interfaces;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using TestingApplication.Models;

namespace UnitTests
{
    [TestClass]
    public class MovieControllerTests
    {
        private Mock<IMovieRepository> _repo;
        private MovieController _controller;

        [TestInitialize]
        public void Setup()
        {
            //Arrange
            _repo = new Mock<IMovieRepository>();
            _controller = new MovieController(_repo.Object);
        }

        [TestMethod]
        public async Task Index_ShouldRedirectToIndexHome_GivenNoId()
        {
            //Arrange
                                    //Act
            var result = await _controller.Index() as HttpStatusCodeResult;

            //Assert
            Assert.AreEqual((int)HttpStatusCode.NotFound,result.StatusCode);
        }

        [TestMethod]
        public async Task Index_ShouldRedirectToIndexHome_GivenInvalidItemId()
        {
            //Arrange
                                    int invalidId = 52;
            //Act
            var result = await _controller.Index(invalidId) as HttpStatusCodeResult;
            _repo.Verify(r => r.GetMovieByIdAsync(invalidId), Times.Once, "Never called get movie by id");
            //Assert
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [TestMethod]
        public async Task Index_ShouldDisplayTheModel_GivenAValidId()
        {
            //Arrange
                                    int testId = 1;
            Movie testMovie = new Movie { ID = 1, Title = "title", Genre = "genre", Rating = 1 };
            _repo.Setup(r => r.GetMovieByIdAsync(testId)).ReturnsAsync(testMovie).Verifiable();
            //Act
            await _controller.Create(testMovie);
            _repo.Verify(r => r.AddMovieAsync(testMovie), "Never called add movie");

            var result = await _controller.Index(testId) as ViewResult;
            _repo.Verify(r => r.GetMovieByIdAsync(testId),Times.Once, "Never called get movie by id");
            _repo.Verify();
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testMovie, result.Model);
        }
        [TestMethod]
        public void Create_ReturnsEmptyView_OnGet()
        {
            //Arrange
                                    //Act
            var result = _controller.Create() as ViewResult;

            //Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public async Task Create_ReturnsBadRequest_GivenInvalidModel()
        {
            //Arrange
                                    var expected = (int)HttpStatusCode.BadRequest;
            _controller.ModelState.AddModelError("error", "some error");
            //Act
            var result = await _controller.Create(null) as HttpStatusCodeResult;
            //Assert
            Assert.AreEqual(expected,result.StatusCode);
        }

        [TestMethod]
        public async Task Create_RedirectsBackToHome_GivenValidModel()
        {
            //Arrange
            
            
            //created movie values
            int id = 4;
            string title = "Test Title";
            string genre = "Test Genre";
            int rating = 2;
            Movie newMovie = new Movie
            {
                ID = id,
                Title = title,
                Genre = genre,
                Rating = rating
            };

            //Act
            var result = await _controller.Create(newMovie);
            _repo.Verify(r => r.AddMovieAsync(newMovie), Times.Once, "Never called add movie");
            var resultRoute = result as RedirectToRouteResult;
            //Assert

            Assert.AreEqual("Index", resultRoute.RouteValues["action"]);
            Assert.AreEqual("Home", resultRoute.RouteValues["controller"]);
        }

        
        
        [TestMethod]
        public async Task GetUpdate_ShouldReturnTheModelInTheView()
        {

            //Arrange
                        int testId = 1;
            Movie testMovie = new Movie { ID = 1, Title = "title", Genre = "genre", Rating = 1 };
            _repo.Setup(r => r.GetMovieByIdAsync(testId)).ReturnsAsync(testMovie).Verifiable();
                        await _controller.Create(testMovie);
            _repo.Verify(r => r.AddMovieAsync(testMovie), Times.Once, "Never called create");
            //Act
            var result = await _controller.Update(testId) as ViewResult;

            _repo.Verify(m => m.GetMovieByIdAsync(testId), Times.Once(),"Never called get movie");
            _repo.Verify();
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testMovie, result.Model);
        }

        [TestMethod]
        public async Task GetUpdate_ShouldReturnToIndex_WithInvalidId()
        {
            //Arrange
                                    int failingId = 52;
            //Act
            var result = await _controller.Update(failingId) as HttpStatusCodeResult;
            _repo.Verify(r => r.GetMovieByIdAsync(failingId), Times.Once, "Did not call get movie by id");

            //Assert
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [TestMethod]
        public async Task PostUpdate_ShouldReturnBadRequest_GivenInvalidModel()
        {
            //Arrange
            var _repo = new Mock<IMovieRepository>();
            var _controller = new MovieController(_repo.Object);
            _controller.ModelState.AddModelError("error", "some error");
            //Act
            var result = await _controller.Update(movie: null) as HttpStatusCodeResult;

            //Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }
        [TestMethod]
        public async Task PostUpdate_ShouldUpdateTheMovie_GivenValidModel()
        {
            //Arrange
            Movie testMovie = new Movie { ID = 1, Title = "title", Genre = "genre", Rating = 1 };
            //Act
            await _controller.Create(testMovie);
            _repo.Verify(r => r.AddMovieAsync(testMovie), "Never called add movie");

            var result = await _controller.Update(testMovie) as RedirectToRouteResult;
            _repo.Verify(r => r.UpdateMovieAsync(testMovie), "Never called update movie");
            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Home", result.RouteValues["controller"]);
        }
    }
}
