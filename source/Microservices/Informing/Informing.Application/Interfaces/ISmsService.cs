namespace Informing.Application.Interfaces
{
    public interface ISmsService
    {
        Task<bool> SendAsync();
    }
}
