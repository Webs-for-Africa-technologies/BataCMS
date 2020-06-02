using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Interfaces
{
    public interface IUnitItemRepository
    {
        IEnumerable<unitItem> unitItems { get; }

        unitItem GetItemById(int itemId);
    }
}
