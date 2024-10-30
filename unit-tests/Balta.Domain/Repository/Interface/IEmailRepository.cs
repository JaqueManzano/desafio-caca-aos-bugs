using Balta.Domain.AccountContext.ValueObjects;

namespace Balta.Domain.Repository
{
    public interface IEmailRepository
    {
        void AddEmail(Email email);
        Email? Get(string emailAddress);
        List<Email> GetList();
    }
}
