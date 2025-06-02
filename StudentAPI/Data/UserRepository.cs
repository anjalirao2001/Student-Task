using StudentAPI.Models;
using System.Data;
using Dapper;
using StudentAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

public class UserRepository : IUserRepository
{
    private readonly DapperContext _context;

    public UserRepository(DapperContext context)
    {
        _context = context;
    }

    public void AddUser(UserRegistration user)
    {
        var query = "AddUser";

        var parameters = new DynamicParameters();
        parameters.Add("Role", user.Role);
        parameters.Add("Username", user.Username);
        parameters.Add("Age", user.Age);
        parameters.Add("Gender", user.Gender);
        parameters.Add("Mobile", user.Mobile);
        parameters.Add("Email", user.Email);
        parameters.Add("Password", user.Password);

        using var connection = _context.CreateConnection();
        connection.Execute(query, parameters, commandType: CommandType.StoredProcedure);
    }

    public UserRegistration? GetUserByEmail(string email)
    {
        var query = "GetUsersByRoleOrEmail";

        var parameters = new DynamicParameters();
        parameters.Add("Email", email);

        using var connection = _context.CreateConnection();
        return connection.QueryFirstOrDefault<UserRegistration>(query, parameters, commandType: CommandType.StoredProcedure);
    }

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




    //public void UpdateUser(UserRegistration user)
    //{
    //    var query = "UpdateUser";  

    //    var parameters = new DynamicParameters();
    //    parameters.Add("Role", user.Role);
    //    parameters.Add("Username", user.Username);
    //    parameters.Add("Age", user.Age);
    //    parameters.Add("Gender", user.Gender);
    //    parameters.Add("Mobile", user.Mobile);
    //    parameters.Add("Email", user.Email);
    //    parameters.Add("Password", user.Password);

    //    using var connection = _context.CreateConnection();
    //    connection.Execute(query, parameters, commandType: CommandType.StoredProcedure);
    //}

    //public void DeleteUser(string email)
    //{
    //    var query = "DeleteUser";  

    //    var parameters = new DynamicParameters();
    //    parameters.Add("Email", email);

    //    using var connection = _context.CreateConnection();
    //    connection.Execute(query, parameters, commandType: CommandType.StoredProcedure);
    //}


    public IEnumerable<UserRegistration> GetUsersByRoleOrEmail(string email, string role)
    {
        var query = "GetUsersByRoleOrEmail";

        var parameters = new DynamicParameters();
        parameters.Add("Email", email);
        parameters.Add("Role", role);

        using var connection = _context.CreateConnection();
        return connection.Query<UserRegistration>(query, parameters, commandType: CommandType.StoredProcedure);
    }



    public IEnumerable<UserRegistration> GetAllUsers()
    {
        var query = "GetAllUsers";

        using var connection = _context.CreateConnection();
        return connection.Query<UserRegistration>(query, commandType: CommandType.StoredProcedure).ToList();
    }


}
