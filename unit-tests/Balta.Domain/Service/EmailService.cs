using Balta.Domain.AccountContext.ValueObjects;
using Balta.Domain.Repository;

public class EmailService
{
    private readonly IEmailRepository _emailRepository;

    public EmailService(IEmailRepository emailRepository)
    {
        _emailRepository = emailRepository;
    }

    public bool AddEmail(Email email)
    {
        if (IsEmailNotRegistered(email))
        {
            _emailRepository.AddEmail(email);
            return true;
        }
        return false;

    }

    public Email? Get(Email email)
    {
        return _emailRepository.Get(email);
    }

    private bool IsEmailNotRegistered(Email emailAddress)
    {
        Email? register = _emailRepository.Get(emailAddress);
        if (register is null)
            return true;
        return false;
    }
}
