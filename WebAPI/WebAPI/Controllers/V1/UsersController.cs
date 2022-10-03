using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Application.Interfaces;
using WebAPI.Contract.Requests;
using WebAPI.Persistence.Domains;

namespace WebAPI.Controllers.V1
{
    
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [EnableCors("Policy")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserSevice _userSevice;

        public readonly IMapper _mapper;
        private IValidator<CreateUserRequest> _validator;
        public UsersController(IUserSevice userSevice,
                              ILogger<UsersController> logger,
                              IMapper mapper,
                              IValidator<CreateUserRequest> validator)
        {
            _userSevice = userSevice;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }
        [Authorize]
        [HttpGet]
        public IActionResult Index(int? pageNumber)
        {
            var user = _userSevice.GetUsers(pageNumber ?? 1);
            return Ok(user);
        }
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetUserById(String id)
        {
            var user = _userSevice.GetUserByID(id);
            return Ok(user);
        }
        [Authorize]
        [HttpPost]
        public IActionResult CreateUser(CreateUserRequest createUserRequest)
        {
            ValidationResult result = _validator.Validate(createUserRequest);
            if (!result.IsValid)
            {
                _logger.LogError("YOU HAVE NOT ENTERED ENOUGH INFORMATION");
                return BadRequest();
            }
            else
            {
                var user = _userSevice.CreateUser(createUserRequest);
                return Ok(user);
            }
        }
        [Authorize]
        [HttpPost("{id}")]
        public IActionResult EditUser(EditUserRequest editUserRequest, String id)
        {
            _userSevice.UpdateUser(editUserRequest, id);
            return Ok();
        }
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(String id)
        {
            _userSevice.DeleteUser(id);
            return Ok();
        }
        [HttpPost("login")]
        public IActionResult Login(LoginRequest loginRequest)
        {
            var login = _userSevice.LogIn(loginRequest);
            if(login != null)
            {
                return Ok(new { Token = login });
            }
            else
            {
                return Ok(new { error = "Login faill"});
            }
        }

    }
}
