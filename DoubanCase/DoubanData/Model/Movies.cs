using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DoubanData.Model
{
    public class Movies
    {
        [Key]
        public int Cid { get; set; }
        public string Rate { get; set; }
        public int Cover_x { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Cover { get; set; }
        public string Mid { get; set; }
        public int Cover_y { get; set; }
        public string Is_New { get; set; }
        public int TypeId { get; set; }
        public string Directors { get; set; }
        public string Actors { get; set; }
        public string Types { get; set; }
        public string Region { get; set; }
        public string Release_Year { get; set; }
        public string Duration { get; set; }        
        public decimal Star { get; set; }
        public string Short_Comment_Content { get; set; }
        public string SubTitle { get; set; }
    }
}
