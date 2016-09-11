using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NillyProxy.Packets.DataObjects
{
    public class SlotObject : IDataObject, ICloneable
    {
        public int ObjectId;

        public byte SlotId;

        public short ObjectType;

        public IDataObject Read(PacketReader r)
        {
            this.ObjectId = r.ReadInt32();
            this.SlotId = r.ReadByte();
            this.ObjectType = r.ReadInt16();
            return this;
        }

        public void Write(PacketWriter w)
        {
            w.Write(this.ObjectId);
            w.Write(this.SlotId);
            w.Write(this.ObjectType);
        }

        public object Clone()
        {
            return new SlotObject
            {
                ObjectId = this.ObjectId,
                ObjectType = this.ObjectType,
                SlotId = this.SlotId
            };
        }

        public override string ToString()
        {
            return string.Concat(new object[]
			{
				"{ ObjectId=",
				this.ObjectId,
				", SlotId=",
				this.SlotId,
				", ObjectType=",
				this.ObjectType,
				" }"
			});
        }
    }
}
