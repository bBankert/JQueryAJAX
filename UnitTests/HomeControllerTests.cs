using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestingApplication.Controllers;
using TestingApplication.Interfaces;
using TestingApplication.Models;

namespace UnitTests
{
    [TestClass]
    public class HomeControllerTests
    {
        private Mock<IMovieRepository> _repo;
        private HomeController _controller;

        [TestInitialize]
        public void Setup()
        {
            _repo = new Mock<IMovieRepository>();
            _controller = new HomeController(_repo.Object);
        }

        [TestMethod]
        public async Task Index_ReturnsViewOfAllMovies_GivenValidModel()
        {
            //Arrange
            List<Movie> movies = new List<Movie>
            {
                new Movie{ ID = 1, Title = "test1",Rating = 2,Genre = "test1"},
                new Movie{ ID = 2, Title = "test2",Rating = 3,Genre = "test2"},
            };
            _repo.Setup(r => r.GetMoviesAsync()).ReturnsAsync(movies).Verifiable("Doesn't return movies");

            //Act
            var result = await _controller.Index() as ViewResult;
            var resultModel = result.Model as List<Movie>;
            _repo.Verify();

            //Assert
            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(movies, resultModel);
            Assert.AreEqual(movies.Count, resultModel.Count);
        }
        

        [TestMethod]
        public async Task Index_ReturnsEmptyView_GivenNoModel()
        {
            //Arrange
            _repo.Setup(r => r.GetMoviesAsync()).ReturnsAsync(new List<Movie> { }).Verifiable("didn't return empty list");
            //Act
            var result = await _controller.Index() as ViewResult;
            var resultModel = result.Model as List<Movie>;
            _repo.Verify();
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, resultModel.Count);
        }
    }
}
