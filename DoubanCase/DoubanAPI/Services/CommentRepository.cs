using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoubanData;
using DoubanData.Model;
using Microsoft.EntityFrameworkCore;


namespace DoubanAPI.Services
{
    public class CommentRepository
    {
        private readonly DataContext _dbContext;
        public CommentRepository(DataContext dataContext)
        {
            _dbContext = dataContext??throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task<List<Comment>> GetCommentsAsync()
        {
            // if (comment is null) throw new ArgumentNullException(nameof(comment));

            // var items = _dbContext.Db_Comment as IQueryable<Comment>;

            var items = _dbContext.Db_Comment.FromSqlRaw("select * from db_comment");

           
            return await items.ToListAsync();


        }

        public async Task<Comment> GetCommentByIdAsync(int commentId)
        {
            if (commentId <= 0)
            {
                throw new ArgumentNullException(nameof(commentId));
            }
            var comment = _dbContext.Db_Comment.Where(x=>x.uId==commentId);
            return await comment.FirstOrDefaultAsync();
        }
       



    }
}
