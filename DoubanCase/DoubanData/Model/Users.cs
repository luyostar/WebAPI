using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DoubanData.Model
{
    public class Users
    {
        [Key]
        public int uId { get; set; }
        public string uName { get; set; }
        public string uPwd { get; set; }
        public DateTime? loginTime { get; set; }  // 要设置日期为可空类型，不然返回Json格式会爆错
    }
}
