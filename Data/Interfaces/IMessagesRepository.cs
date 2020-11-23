using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Interfaces
{
    public interface IMessagesRepository
    {
        IEnumerable<Message> Messages { get; }

        public Task<Message> AddMessageAsync(Message message);

        Task<Message> GetMessageByIdAsync(int id);

        Task UpdateMessageAsync(Message updateMessage);

        public Task DeleteMessageAsync(Message message);
    }
}
