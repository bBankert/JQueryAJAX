using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TestingApplication.Data;
using TestingApplication.Interfaces;
using TestingApplication.Models;
using System.Data.Entity;


namespace TestingApplication.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;
        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task AddMovieAsync(Movie movie)
        {
            _context.Movies.Add(movie);
            return _context.SaveChangesAsync();
            
        }

        public Task<Movie> GetMovieByIdAsync(int id)
        {
            return _context.Movies.FirstOrDefaultAsync(m => m.ID == id);
        }

        public Task<List<Movie>> GetMoviesAsync()
        {
            return _context.Movies.ToListAsync();
        }

        public Task UpdateMovieAsync(Movie movie)
        {
            _context.Entry(movie).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }
    }
}