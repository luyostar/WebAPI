using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;
using DoubanData.Model;

namespace DoubanData
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public static readonly ILoggerFactory ConsoleLoggerFactory = LoggerFactory
           .Create(buidler =>
           {
               buidler.AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name
               && level == LogLevel.Information).AddConsole();  //如果AddConsole方法报错，需装Microsoft.Extensions.Logging.Console包
            }
           );

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(ConsoleLoggerFactory)
                          .EnableSensitiveDataLogging()
                          ;
        }

        public DbSet<Comment>  Db_Comment { get; set; }
        public DbSet<Users> Db_Users { get; set; }
        public DbSet<BuyDetail> Db_BuyDetail { get; set; }
        public DbSet<Movies> Db_Movies { get; set; }


    }
}
