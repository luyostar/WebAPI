using DoubanData;
using DoubanData.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoubanAPI.Services
{
    public class MoviesRepository
    {
        private readonly DataContext _dbContext;
        public MoviesRepository(DataContext dataContext)
        {
            _dbContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task<List<Movies>> GetAllMoviesAsync()
        {
            var items = _dbContext.Db_Movies as IQueryable<Movies>;
            return await items.ToListAsync();
        }

        public async Task<Movies> GetMoviesByMidAsync(string mId)
        {
            if (string.IsNullOrWhiteSpace(mId))
            {
                throw new ArgumentNullException(nameof(mId));
            }

            var item = _dbContext.Db_Movies.Where(x => x.Mid == mId);

            return await item.FirstOrDefaultAsync();
        }

        public async Task<List<Movies>> GetMoviesAsync(MoviesParameters moviesParameters)
        {
            if (moviesParameters==null)
            {
                throw new ArgumentNullException(nameof(moviesParameters));
            }
            var items = _dbContext.Db_Movies
                .Where(x => x.TypeId == moviesParameters.TypeId)
                .Skip(moviesParameters.PageSize * (moviesParameters.PageNumber - 1))
                .Take(moviesParameters.PageSize)
                ;
            return await items.ToListAsync();

        }

        public void AddMovie(Movies movies)
        {
            if (movies == null) throw new ArgumentNullException(nameof(movies));

            _dbContext.Entry(movies).State = EntityState.Added;
            _dbContext.Db_Movies.Add(movies);
        }

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() >= 0;
        }





    }
}
