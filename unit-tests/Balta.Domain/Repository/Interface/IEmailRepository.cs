using Balta.Domain.AccountContext.ValueObjects;

namespace Balta.Domain.Repository
{
    public interface IEmailRepository
    {
        void AddEmail(Email email);
        Email? Get(Email email);
        List<Email> GetList();
    }
}
