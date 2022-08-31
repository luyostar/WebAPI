using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoubanAPI.Attributes;
using DoubanAPI.Services;
using DoubanData.Model;

namespace DoubanAPI.Controllers
{
    [AllowCrossSiteJson]
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MoviesRepository _moviesRepository;
        public MoviesController(MoviesRepository moviesRepository)
        {
            _moviesRepository = moviesRepository?? throw new ArgumentNullException(nameof(moviesRepository));
        }
        // GET: api/<MoviesController>
        [HttpGet]
        public async Task<ActionResult<List<Movies>>> GetMovies([FromQuery]MoviesParameters moviesParameters)
        {
            var items = await _moviesRepository.GetMoviesAsync(moviesParameters);

            return new JsonResult(items);
        }

        // GET api/<MoviesController>/all
        [HttpGet("all")]
        public async Task<ActionResult<List<Movies>>> GetAllMovies()
        {
            List<Movies> items=null;

            items = await _moviesRepository.GetAllMoviesAsync();            

            return new JsonResult(items);            
        }
        [HttpGet("{mid}",Name =nameof(GetMovieById))]
        public async Task<ActionResult<Movies>> GetMovieById(string mid)
        {
            if (string.IsNullOrWhiteSpace(mid)) return new JsonResult(new { isSuccess = false, curDescription = "movie id is invalid" });

            var item = await _moviesRepository.GetMoviesByMidAsync(mid);

            return new JsonResult(item);

        }

        // POST api/<MoviesController>
        [HttpPost]
        public async Task<ActionResult> AddMovie(Movies movies)
        {
            _moviesRepository.AddMovie(movies);
            await _moviesRepository.SaveAsync();

            return new JsonResult(new { isSuccess=true, curDescription = "Add Success"});
        }
    }
}
