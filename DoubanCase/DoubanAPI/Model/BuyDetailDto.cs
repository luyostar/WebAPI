using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DoubanAPI.Model
{
    public class BuyDetailDto
    {
        [Key]
        public int id { get; set; }
        public int uId { get; set; }
        public int movieId { get; set; }
        public string movieName { get; set; }
        public int buyNum { get; set; }
        public decimal price { get; set; }
        public string imgUrl { get; set; }
    }
}
