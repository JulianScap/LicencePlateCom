using System.Collections.Generic;
using System.Linq;
using LicencePlateCom.API.Database;
using LicencePlateCom.API.Database.Entities;

namespace LicencePlateCom.API.Business
{
    public interface IMessageService
    {
        bool SaveMessage(Message message);
    }

    public class MessageService : IMessageService
    {
        private readonly IMongoContext _mongoContext;

        public MessageService(IMongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public virtual bool SaveMessage(Message message)
        {
            _mongoContext.Add(message);
            return true;
        }

        public virtual IEnumerable<Message> GetMessages(Message message)
        {
            return Enumerable.Empty<Message>();
        }
    }
}