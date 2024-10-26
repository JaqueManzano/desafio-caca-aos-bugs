using Balta.Domain.StoreContext.Entities;

namespace Balta.Domain.Test.Repository.Interface
{
    public interface IEmailRepository
    {
        bool AddEmail(string email);
        string? Get(string email);
        List<string> GetList();

    }
}
