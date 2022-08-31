using DoubanAPI.Services;
using DoubanData.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoubanAPI.Attributes;



namespace DoubanAPI.Controllers
{
    [AllowCrossSiteJson]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly CommentRepository _commentRepository;
      
        public CommentController(CommentRepository commentRepository)
        {
            _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
        
        }
        // GET: api/<CommentController>
        [HttpGet]       
        public async Task<ActionResult<List<Comment>>> GetAllComment()
        {
            var items = await _commentRepository.GetCommentsAsync();

            //Response.Headers.Add("Access-Control-Allow-Origin","*");

            // return NotFound(); // 返回404
            // return Ok(items);  // 返回200状态码
            return new JsonResult(items); // 返回Json 格式
        }

        // GET api/<CommentController>/5
        [HttpGet("{uid}")] 
        public async Task<ActionResult<Comment>> GetCommentById(int uid)
        {
            var item = await _commentRepository.GetCommentByIdAsync(uid);
            if (item == null)
            {
                return NotFound();
            }
            return new JsonResult(item);
        }

        // POST api/<CommentController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CommentController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CommentController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
