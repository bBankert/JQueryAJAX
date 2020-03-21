using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestingApplication.Data;
using TestingApplication.Interfaces;
using TestingApplication.Models;
using TestingApplication.Repositories;
using System.Net;

namespace TestingApplication.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieRepository _repository;
        public MovieController()
        {
            _repository = new MovieRepository(new ApplicationDbContext());
        }
        public MovieController(IMovieRepository repository)
        {
            _repository = repository;
        }
        // GET: Movie
        public async Task<ActionResult> Index(int id=0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult((int)HttpStatusCode.NotFound, "Could not find movie");
            }
            var movie = await _repository.GetMovieByIdAsync(id);
            if(movie != null)
            {
                return View(movie);
            }
            else
            {
                return new HttpStatusCodeResult((int)HttpStatusCode.NotFound, "Could not find movie");
            }
        }
        // GET /Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddMovieAsync(movie);   
            }
            else
            {
                return new HttpStatusCodeResult((int)HttpStatusCode.BadRequest,"Bad Request");
            }
            return RedirectToAction("Index","Home");
        }

        //way to run async methods in synchronous methods...
        //private T invokeAsyncMethod<T> (Func<Task<T>> func)
        //{
        //    return Task.Factory.StartNew(func)
        //        .Unwrap().GetAwaiter().GetResult();
        //}

        // GET: Update
        public async Task<ActionResult> Update(int id = 0)
        {

            var model = await _repository.GetMovieByIdAsync(id);
            if(model == null)
            {
                return new HttpStatusCodeResult((int)HttpStatusCode.NotFound, "Movie not found");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Update(Movie movie)
        {
            if (ModelState.IsValid)
            {
                await _repository.UpdateMovieAsync(movie);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}