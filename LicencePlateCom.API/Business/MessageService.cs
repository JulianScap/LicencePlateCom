using System.Collections.Generic;
using System.Linq;
using LicencePlateCom.API.Database;
using LicencePlateCom.API.Database.Entities;
using LicencePlateCom.API.Utilities;

namespace LicencePlateCom.API.Business
{
    public interface IMessageService
    {
        Result SaveMessage(Message message);
    }

    public class MessageService : IMessageService
    {
        private readonly IMongoContext<Message> _mongoContext;

        public MessageService(IMongoContext<Message> mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public virtual Result SaveMessage(Message message)
        {
            return (Result) _mongoContext.Add(message);
        }

        public virtual IEnumerable<Message> GetMessages(Message message)
        {
            return Enumerable.Empty<Message>();
        }
    }
}