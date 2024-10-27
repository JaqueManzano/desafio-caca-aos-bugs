namespace Balta.Domain.Test.Repository.Interface
{
    public interface IFakeEmailRepository
    {
        bool AddEmail(string email);
        string? Get(string email);
        List<string> GetList();

    }
}
