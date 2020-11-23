using BataCMS.Data;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Repositories
{
    public class MessageRepository : IMessagesRepository
    {
        private readonly AppDbContext _appDbContext;

        public MessageRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Message> Messages => _appDbContext.Messages;

        public async Task<Message> AddMessageAsync(Message message)
        {
            await _appDbContext.Messages.AddAsync(message);
            await _appDbContext.SaveChangesAsync();
            return message;
        }

        public async Task DeleteMessageAsync(Message message)
        {
            _appDbContext.Messages.Remove(message);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Message> GetMessageByIdAsync(int id)
        {
            return await _appDbContext.Messages.FirstOrDefaultAsync(p => p.MessageId == id);
        }

        public async Task UpdateMessageAsync(Message updateMessage)
        {
            _appDbContext.Messages.Update(updateMessage);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
