namespace MyTaskManager.Services.Interfaces
{
    public interface IGetUserIdentity
    {
        IEnumerable<string> GetClaims();
    }
}
