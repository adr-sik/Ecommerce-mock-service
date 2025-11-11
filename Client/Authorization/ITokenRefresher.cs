namespace Client.Authorization
{
    public interface ITokenRefresher
    {
        Task <bool> RefreshTokenA();
    }
}
