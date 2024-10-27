using Balta.Domain.Test.Repository.Interface;

namespace Balta.Domain.Test.Repository
{
    public class FakeEmailRepository : IFakeEmailRepository
    {
        private readonly List<string> _emails = new List<string>();

        public bool AddEmail(string email)
        {
            if (IsEmailNotRegistered(email))
            {
                _emails.Add(email);
                return true;
            }
            return false;
        }

        public string? Get(string email)
        {
            return _emails.FirstOrDefault(x => x.Equals(email));
        }

        public List<string> GetList()
        {
            return _emails;
        }

        private bool IsEmailNotRegistered(string email)
        {
            return !_emails.Select(x => x.ToLower()).ToList().Contains(email.ToLower());
        }
    }
}
