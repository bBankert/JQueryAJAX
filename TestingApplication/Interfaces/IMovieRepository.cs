using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingApplication.Models;

namespace TestingApplication.Interfaces
{
    public interface IMovieRepository
    {
        Task<Movie> GetMovieByIdAsync(int id);
        Task<List<Movie>> GetMoviesAsync();
        Task AddMovieAsync(Movie movie);
        Task UpdateMovieAsync(Movie movie);

    }
}
