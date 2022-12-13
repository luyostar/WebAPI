using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DoubanAPI.Model
{
    public class CommentDto
    {
    
        [Key]
        public int uId { get; set; }
        public string uName { get; set; }
        public string subjectUrl { get; set; }
        public string avatorUrl { get; set; }
        public string subTitle { get; set; }
        public string subContent { get; set; }

    }

    
}
