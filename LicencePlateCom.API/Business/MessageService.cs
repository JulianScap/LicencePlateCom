using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LicencePlateCom.API.Database;
using LicencePlateCom.API.Database.Entities;
using LicencePlateCom.API.Utilities;
using Microsoft.Extensions.Logging;

namespace LicencePlateCom.API.Business
{
    public interface IMessageService
    {
        Task<Result<IEnumerable<Message>>> GetMessagesAsync(Expression<Func<Message, bool>> expression);
        Task<Result> SaveMessageAsync(Message message);
    }

    public class MessageService : IMessageService
    {
        private readonly IMongoContext<Message> _mongoContext;
        private readonly ILogger<MessageService> _logger;

        public MessageService(IMongoContext<Message> mongoContext, ILogger<MessageService> logger)
        {
            _mongoContext = mongoContext;
            _logger = logger;
        }

        public virtual async Task<Result> SaveMessageAsync(Message message)
        {
            try
            {
                var result = await _mongoContext.AddAsync(message).ConfigureAwait(false);
                return Return.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SaveMessageAsync Failed");
                return Return.Failed<Result>();
            }
        }

        public virtual async Task<Result<IEnumerable<Message>>> GetMessagesAsync(
            Expression<Func<Message, bool>> expression)
        {
            try
            {
                var result = await _mongoContext.GetAsync(expression).ConfigureAwait(false);
                return Return.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetMessagesAsync Failed");
                return Return.Failed<IEnumerable<Message>>();
            }
        }
    }
}