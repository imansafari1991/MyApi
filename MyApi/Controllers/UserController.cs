using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebFramework.Api;
using WebFramework.Filters;

namespace MyApi.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    [ApiResultFilter]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet]
        
        public async Task<List<User>> Get(CancellationToken cancellationToken)
        {
            var users = await userRepository.TableNoTracking.ToListAsync(cancellationToken);

            return users;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> Get(int id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            if (user == null)
                return NotFound();
            return user;
        }

        //[HttpPost]
        //public async Task<ApiResult<User>> Create(UserDto userDto, CancellationToken cancellationToken)
        //{
        //    //var exists = await userRepository.TableNoTracking.AnyAsync(p => p.UserName == userDto.UserName);
        //    //if (exists)
        //    //    return BadRequest("نام کاربری تکراری است");

        //    var user = new User
        //    {
        //        Age = userDto.Age,
        //        FullName = userDto.FullName,
        //        Gender = userDto.Gender,
        //        UserName = userDto.UserName
        //    };
        //    await userRepository.AddAsync(user, userDto.Password, cancellationToken);
        //    return user;
        //}

        [HttpPut]
        public async Task<ActionResult> Update(int id, User user, CancellationToken cancellationToken)
        {
            var updateUser = await userRepository.GetByIdAsync(cancellationToken, id);

            updateUser.UserName = user.UserName;
            updateUser.PasswordHash = user.PasswordHash;
            updateUser.FullName = user.FullName;
            updateUser.Age = user.Age;
            updateUser.Gender = user.Gender;
            updateUser.IsActive = user.IsActive;
            updateUser.LastLoginDate = user.LastLoginDate;

            await userRepository.UpdateAsync(updateUser, cancellationToken);

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            await userRepository.DeleteAsync(user, cancellationToken);

            return Ok();
        }
    }
}
