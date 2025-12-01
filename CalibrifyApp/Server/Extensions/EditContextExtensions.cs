using Microsoft.AspNetCore.Components.Forms;

namespace CalibrifyApp.Server.Extensions
{
    public static class EditContextExtensions
    {
        public static void AddValidationMessage(this EditContext editContext, FieldIdentifier fieldIdentifier, string message)
        {
            var messages = new ValidationMessageStore(editContext);
            messages.Add(fieldIdentifier, message);
            editContext.NotifyValidationStateChanged();
        }

        public static void AddValidationMessage(this EditContext editContext, in FieldIdentifier fieldIdentifier, string message)
        {
            var messages = new ValidationMessageStore(editContext);
            messages.Add(fieldIdentifier, message);
            editContext.NotifyValidationStateChanged();
        }
    }
}
