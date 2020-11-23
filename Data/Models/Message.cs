using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Models
{
    public class Message
    {
        public int MessageId { get; set; }

        public string MessageTitle { get; set; }

        public string MessageContent { get; set; }

        public DateTime DateModified { get; set; }
    }
}
