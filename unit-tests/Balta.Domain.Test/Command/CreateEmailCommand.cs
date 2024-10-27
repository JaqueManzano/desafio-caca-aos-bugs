using Balta.Domain.AccountContext.ValueObjects;
using Balta.Domain.SharedContext.Abstractions;
using Balta.Domain.StoreContext.Entities;
using Balta.Domain.Test.Command.Interface;
using Flunt.Notifications;
using Flunt.Validations;

namespace Balta.Domain.Test.Command
{
    public class CreateEmailCommand : Notifiable<Notification>, ICommand
    {
        public Email Email { get; private set; }

        public CreateEmailCommand(string emailAddress, IDateTimeProvider dateTimeProvider)
        {
            if (emailAddress is null)
            {
                throw new ArgumentNullException();
            }

            Email = Email.ShouldCreate(emailAddress, dateTimeProvider);
        }
        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
            .Requires()
                .IsNotNullOrEmpty(Email.Address, "The email address cannot be empty")
                .IsEmail(Email.Address, "Email"));
        }
    }
}
