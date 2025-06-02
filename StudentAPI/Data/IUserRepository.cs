using StudentAPI.Models;

public interface IUserRepository
{
    void AddUser(UserRegistration user);
    UserRegistration? GetUserByRoleEmailAndPassword(string role, string email, string password);
    //void UpdateUser(UserRegistration user);
    //void DeleteUser(string email);

    IEnumerable<UserRegistration> GetUsersByRoleOrEmail(string email, string role);

    IEnumerable<UserRegistration> GetAllUsers();

    UserRegistration? GetUserByEmail(string email);


}
