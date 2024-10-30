
using Balta.Domain.AccountContext.ValueObjects;
using Balta.Domain.Repository;

namespace Balta.Domain.Test.Repository
{
    public class FakeEmailRepository : IEmailRepository
    {
        private readonly List<Email> _emails = new List<Email>();

        public void AddEmail(Email email)
        {
            _emails.Add(email);
        }

        public Email? Get(string emailAddress)
        {
            return  _emails.FirstOrDefault(x => x.Address.Equals(emailAddress));
        }

        public List<Email> GetList()
        {
            return _emails;
        }

    }
}
