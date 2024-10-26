using Balta.Domain.StoreContext.Entities;
using Balta.Domain.Test.Command.Interface;
using Flunt.Notifications;
using Flunt.Validations;
using System.Reflection.Emit;

namespace Balta.Domain.Test.Command
{
    public class CreateEmailCommand : Notifiable<Notification>, ICommand
    {
        public string email { get; set; }

        public CreateEmailCommand(string email)
        {
            this.email = email;
        }
        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
            .Requires()
                .IsNull(email, "The email cannot be null.")
                .IsEmail(email, "Email")
                .IsEmpty(email, "The email cannot be empty"));
        }

        public override string ToString()
        {
            return email;
        }
    }
}
