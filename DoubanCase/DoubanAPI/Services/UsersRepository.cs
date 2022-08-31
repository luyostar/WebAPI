using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoubanData;
using DoubanData.Model;
using Microsoft.EntityFrameworkCore;

namespace DoubanAPI.Services
{
    public class UsersRepository
    {
        private readonly DataContext _dbContext;
        public UsersRepository(DataContext dataContext)
        {
            _dbContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));

        }

        public async Task<List<Users>> GetAllUsersAsync()
        {
            // if (comment is null) throw new ArgumentNullException(nameof(comment));

            var items = _dbContext.Db_Users as IQueryable<Users>;

            // var items = _dbContext.Db_Users.FromSqlRaw("select * from db_users");

            return await items.ToListAsync();
        }

        public async Task<Users> GetUserByIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }
            var user = _dbContext.Db_Users.Where(x => x.uId == userId);
            return await user.FirstOrDefaultAsync();
        }

        public async Task<Users> GetUserByIdAndPwdAsync(int userId,string userPwd)
        {
            if (userId <= 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }
            var user = _dbContext.Db_Users.Where(x => x.uId == userId && x.uPwd==userPwd);
            return await user.FirstOrDefaultAsync();
        }



        public void AddUser(Users user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            _dbContext.Entry(user).State = EntityState.Added;
            _dbContext.Db_Users.Add(user);
        }

        public void UpdateUserPwdOrLoginTime(Users user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            _dbContext.Entry(user).State = EntityState.Modified;
            _dbContext.Db_Users.Update(user);

        }

        public void DeleteUser(Users user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            _dbContext.Entry(user).State = EntityState.Deleted;
            _dbContext.Db_Users.Remove(user);
        }

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() >= 0;
        }

        public async Task<bool> UserExistAsync(int uId)
        {
            if (uId <= 0) throw new ArgumentNullException(nameof(uId));
            return await _dbContext.Db_Users.AnyAsync(x => x.uId == uId);
        }

        

    }
}
