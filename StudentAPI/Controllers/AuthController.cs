using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentAPI.Data;
using StudentAPI.Models;
using StudentAPI.Services;
using System.Data;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly JwtService _jwtService;
    private readonly DapperContext _context;

    public AuthController(IUserRepository userRepository, JwtService jwtService, DapperContext context)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _context = context;
    }

    [HttpPost("register")]
    public IActionResult Register(UserRegistration user)
    {
        _userRepository.AddUser(user);
        return Ok(new { message = "User registered successfully" });
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        var user = _userRepository.GetUserByRoleEmailAndPassword(
            loginRequest.Role,
            loginRequest.Email,
            loginRequest.Password
        );

        if (user == null)
            return Unauthorized(new { message = "Invalid email or password" });

        // Generate JWT token
        var token = _jwtService.GenerateToken(user.Email, user.Role);

        // Based on role, return data
        if (user.Role.Equals("admin", StringComparison.OrdinalIgnoreCase))
        {
            var allUsers = _userRepository.GetAllUsers();
            return Ok(new
            {
                message = "Admin login successful",
                token,
                users = allUsers
            });
        }
        else if (user.Role.Equals("student", StringComparison.OrdinalIgnoreCase))
        {
            return Ok(new
            {
                message = "Student login successful",
                token,
                user
            });
        }

        return BadRequest(new { message = "Invalid role" });
    }


    [HttpGet("GetUserByRoleEmailAndPassword/{role}/{email}/{password}")]
    public UserRegistration? GetUserByRoleEmailAndPassword(string role, string email, string password)
    {
        var query = "GetUserByRoleEmailAndPassword";

        var parameters = new DynamicParameters();
        parameters.Add("Role", role);
        parameters.Add("Email", email);
        parameters.Add("Password", password);

        using var connection = _context.CreateConnection();
        var user = connection.QueryFirstOrDefault<UserRegistration>(query, parameters, commandType: CommandType.StoredProcedure);
        return user;
    }


    [HttpGet("GetAllUsers/{email}/{role}")]
    public IActionResult GetUsers(string role, string email)
    {
        if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(email))
        {
            return BadRequest(new { message = "Role and Email are required" });
        }

        if (role.Equals("admin", StringComparison.OrdinalIgnoreCase))
         {
             // Admin gets all users
             var users = _userRepository.GetUsersByRoleOrEmail(email, role);
              return Ok(users);
         }
           else if (role.Equals("student", StringComparison.OrdinalIgnoreCase))
            {
             // Student gets only their own record
            var users = _userRepository.GetUsersByRoleOrEmail(email, role);
            return Ok(users);
           }
            else
            {
                return BadRequest(new { message = "Invalid role" });
            }
          }





}
