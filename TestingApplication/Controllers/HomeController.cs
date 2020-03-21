using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestingApplication.Data;
using TestingApplication.Interfaces;
using TestingApplication.Repositories;

namespace TestingApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieRepository _repository;
        public HomeController()
        {
            _repository = new MovieRepository(new ApplicationDbContext());
        }
        public HomeController(IMovieRepository repository)
        {
            _repository = repository;
        }
        // GET: Home
        public async Task<ActionResult> Index()
        {
            var movieList = await _repository.GetMoviesAsync();


            return View(movieList);
        }
    }
}