using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoubanAPI.Attributes;
using DoubanData.Model;
using DoubanAPI.Services;
using AutoMapper;
using DoubanAPI.Model;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DoubanAPI.Controllers
{
    // [EnableCors("Domain")]
    //[AllowCrossSiteJson]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersRepository _usersRepository;
        private readonly IMapper _mapper;
        public UsersController(UsersRepository usersRepository,IMapper mapper)
        {
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        // GET: api/<UsersController>
        [HttpGet]
        [AllowCrossSiteJson]
        public async Task<ActionResult<List<UsersDto>>> GetAllUsers()
        {
            var items = await _usersRepository.GetAllUsersAsync();

            var usersDto = _mapper.Map<IEnumerable<UsersDto>>(items);

            foreach (var item in usersDto)
            {
                item.isSuccess = true;
                item.curDescription = "get all users";
            }

            return new JsonResult(usersDto);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}",Name =nameof(GetUserByIdAndPwd))]
        [AllowCrossSiteJson]
        public async Task<ActionResult<UsersDto>> GetUserByIdAndPwd(int id,string pwd)
        {
            var item = await _usersRepository.GetUserByIdAndPwdAsync(id,pwd);

            if (item == null) return new JsonResult(new {isSuccess=false,curDescription="No found User" });

            var usersDto = _mapper.Map<UsersDto>(item);

            usersDto.isSuccess = true;
            usersDto.curDescription = "get one user";
            
            return new JsonResult(usersDto);
        }

        [HttpGet("exist/{id}")]
        [AllowCrossSiteJson]
        public async Task<ActionResult<bool>> UserExist(int id)
        {
            var item = await _usersRepository.UserExistAsync(id);

            return new JsonResult(item);
        }

        // POST api/<UsersController>
        [HttpPost]
        [AllowCrossSiteJson]
        public async Task<ActionResult<UsersDto>> AddUser(Users user)
        {
            var item = await _usersRepository.GetUserByIdAsync(user.uId);

            if (item != null) return new JsonResult(new { isSuccess = false, curDescription = "add fail:the UserId exist already" });

            _usersRepository.AddUser(user);
            await _usersRepository.SaveAsync();

            var usersDto = _mapper.Map<UsersDto>(user);

            usersDto.isSuccess = true;
            usersDto.curDescription = "Add Success";

            return new JsonResult(usersDto);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        [AllowCrossSiteJson]
        public async Task<ActionResult<UsersDto>> UpdateUser(Users user)
        {
            _usersRepository.UpdateUserPwdOrLoginTime(user);
            await _usersRepository.SaveAsync();

            var usersDto = _mapper.Map<UsersDto>(user);

            usersDto.isSuccess = true;
            usersDto.curDescription = "Updating Success";

            return new JsonResult(usersDto);
        }

        // PUT api/<UsersController>/5/login
        [HttpPut("login/{id}",Name =nameof(UserLogin))]
        [AllowCrossSiteJson]
        public async Task<ActionResult<UsersDto>> UserLogin(int id,string pwd)
        {
            var user = await _usersRepository.GetUserByIdAndPwdAsync(id,pwd);

            if (user == null) return new JsonResult(new { isSuccess = false, curDescription = "login fail:No found User" });
            
            user.loginTime =DateTime.Now;
            _usersRepository.UpdateUserPwdOrLoginTime(user);
            await _usersRepository.SaveAsync();

            var usersDto = _mapper.Map<UsersDto>(user);

            usersDto.isSuccess = true;
            usersDto.curDescription = "Login Success";

            return new JsonResult(usersDto);
        }


        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        [AllowCrossSiteJson]
        public async Task<ActionResult> DeleteUser(int id)
        {
            if (!await _usersRepository.UserExistAsync(id))
            {
                return NotFound();
            }

            var user = await _usersRepository.GetUserByIdAsync(id);
            
            _usersRepository.DeleteUser(user);
            await _usersRepository.SaveAsync();

            return new JsonResult(new {isSuccess=true,curDescription="Deleting Success"});
        }

        //以下添加对各个Action的访问权限(跨域访问),像POST，PUT，DELETE在动作前都会有个预检(preflight)options请求，
        //所以要加允许跨域attribute，否则会失败
        [HttpOptions("login/{id}",Name =nameof(UserLoginOptions))]
        [AllowCrossSiteJson]
        public IActionResult UserLoginOptions()
        {
            //Response.Headers.Add("Access-Control-Allow-Origin", "*");
            //Response.Headers.Add("Access-Control-Allow-Methods", "GET,PUT,POST,OPTIONS,DELETE,PATCH,HEAD");
            //Response.Headers.Add("Access-Control-Allow-Headers", "*");
            //Response.Headers.Add("Allow", "GET,POST,PUT,OPTIONS,DELETE");


            return Ok();
        }

        [HttpOptions("{id}", Name = nameof(AllOptions))]
        //[HttpOptions("{id}")]
        [AllowCrossSiteJson]
        public IActionResult AllOptions()
        {
            return Ok();
        }


        [HttpOptions("",Name =nameof(UserOptions))]
        [AllowCrossSiteJson]
        public IActionResult UserOptions()
        {
            return Ok();
        }



    }
}
