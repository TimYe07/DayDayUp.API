namespace DayDayUp.AccountContext
{
    public interface IUserService
    {
        JwtToken Authenticate(string email, string password);
        UserDto GetUser(string name);
    }
}