using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DoubanAPI.Model
{
    public class UsersDto
    {
        [Key]
        public int uId { get; set; }
        public string uName { get; set; }
        public string uPwd { get; set; }
        public DateTime? loginTime { get; set; }

        public bool? isSuccess { get; set; }

        public string curDescription { get; set; }


    }
}
