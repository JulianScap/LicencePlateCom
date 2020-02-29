using System.Collections.Generic;
using System.Linq;

namespace LicencePlateCom.API.Database.Entities
{
    public class Message : AbstractEntity, IValidatable
    {
        internal const string NoMessage = "No message specified (empty).";
        internal const string NoRecipient = "No recipient specified.";
        internal const string RecipientTooLong = "The recipient licence plate is too long.";

        public string Recipient { get; set; }
        public PredefinedMessage PredefinedMessage { get; set; }

        public bool Validate(out List<string> messages)
        {
            var list = new List<string>();
            messages = list;

            if (PredefinedMessage == PredefinedMessage.None)
            {
                list.Add(NoMessage);
            }

            if (string.IsNullOrWhiteSpace(Recipient))
            {
                list.Add(NoRecipient);
            }
            else if (Recipient.Length > 10)
            {
                list.Add(RecipientTooLong);
            }

            return !list.Any();
        }
    }
}