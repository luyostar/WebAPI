using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DoubanData.Model
{
    public class BuyDetail
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

    public class AlipayHeader
    {
        [Key]
        public int PayId { get; set; }
        public string PayNo { get; set; }
        public DateTime PayDate { get; set; }
        public int UserId { get; set; }
        public string PayComment { get; set; }
        public string InvoiceAddress { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class AlipayDetail
    {
        [Key]
        public int PayDetailId { get; set; }
        public int PayId { get; set; }
        public int MoiveId { get; set; }
        public int OrderNum { get; set; }
        public decimal OrderPrice { get; set; }
        public decimal OrderDiscount { get; set; }
        public string DetailComment { get; set; }
    }
}
