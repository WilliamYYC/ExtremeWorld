using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BagItem
    {
        public ushort itemId;
        public ushort Count;

        public static BagItem Zero = new BagItem{ itemId = 0, Count =0};

        public BagItem(int itemId,int  Count)
        {
            this.itemId = (ushort)itemId;
            this.Count = (ushort)Count;
        }

        public static bool operator ==(BagItem lhs, BagItem rhs)
        {
            return lhs.itemId == rhs.itemId && lhs.Count == rhs.Count;
        }

        public static bool operator !=(BagItem lhs, BagItem rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object other)
        {
            if (other is BagItem)
            {
                return Equals((BagItem)other);
            }
            return false;
        }

        public bool Equals(BagItem other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return itemId.GetHashCode() ^ (Count.GetHashCode() << 2);
        }
    }  
}
