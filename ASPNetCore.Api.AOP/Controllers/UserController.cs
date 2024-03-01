using ASPNetCore.Api.AOP.FilterAop;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ASPNetCore.Api.AOP.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    //[AopAuthorizationFilter("User")] //可以给指定的控制器加授权特性
    public class UserController : ControllerBase
    {
        public readonly List<User> users = new List<User>()
        {
         new User(){ Id=1,Name="王锐", Email="abcA",Created=DateTime.Now },
                  new User(){ Id=2,Name="王锐2" , Email="1abcA",Created=DateTime.Now},
                                    new User(){ Id=3,Name="王锐3", Email="ab2cA",Created=DateTime.Now }

        };

        public UserController() { }

        [HttpGet]
        [AopAuthorizationFilter("User")]  //给指定的方法加特性
        public IActionResult GetUsers()
        {
            return Ok(users);
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] User user)
        {
            users.Add(user);
            return GetUsers(); // 这里直接调用没有关系，不会限制
        }

        [HttpGet]
        public IActionResult Add()
        {

            int i = 0;
            var b = 1 / i;  //ExceptionFilter捕获异常成功
            return Ok();
        }

        [HttpGet]
        [AopResourceFilter(1)] //缓存有效期，秒
        public IActionResult GetByUserId(int id)
        {

            var user = users.Find(u => u.Id == id);
            if (user is null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public DateTime Created { get; set; }
    }
}
