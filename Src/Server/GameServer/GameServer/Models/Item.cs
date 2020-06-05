using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    
    class Item
    {
        public TCharacterItem dbItem;

        public int ItemID;

        public int Count;


        public  Item(TCharacterItem dbItem)
        {
            this.dbItem = dbItem;
            this.ItemID = (short)dbItem.ItemID;
            this.Count = (short)dbItem.ItemCount;
        }

        public void Add(int count)
        {
            this.Count += count;
            dbItem.ItemCount = this.Count;
        }


        public void Remove(int count)
        {
            this.Count -= count;
            dbItem.ItemCount = this.Count;
        }


        public bool Use(int count = 1)
        {
            return false;
        }

        public override string ToString()
        {
            return string.Format("ID: {0}, COUNT :{1}", this.ItemID, this.Count);
        }
    }
}
