using System.Collections.Generic;
using System.Linq;

namespace LicencePlateCom.API.Database.Entities
{
    public class Message : AbstractEntity, IValidable
    {
        public string Recipient { get; set; }
        public PredefinedMessage PredefinedMessage { get; set; }

        public bool Validate(out IEnumerable<string> messages)
        {
            var list = new List<string>();
            messages = list;

            if (PredefinedMessage == PredefinedMessage.None)
            {
                list.Add("No message specified (empty).");
            }

            if (string.IsNullOrWhiteSpace(Recipient))
            {
                list.Add("No recipient specified.");
            }
            else if (Recipient.Length > 10)
            {
                list.Add("The recipient licence plate is too long.");
            }

            return !list.Any();
        }
    }
}