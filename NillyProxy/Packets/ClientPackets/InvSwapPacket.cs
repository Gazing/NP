using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NillyProxy.Packets.DataObjects;

namespace NillyProxy.Packets.ClientPackets
{
    public class InvSwapPacket : Packet
    {
        public int Time;

        public Location Position;

        public SlotObject SlotObject1;

        public SlotObject SlotObject2;

        public override PacketType Type
        {
            get
            {
                return PacketType.INVSWAP;
            }
        }

        public override void Read(PacketReader r)
        {
            this.Time = r.ReadInt32();
            this.Position = (Location)new Location().Read(r);
            this.SlotObject1 = (SlotObject)new SlotObject().Read(r);
            this.SlotObject2 = (SlotObject)new SlotObject().Read(r);
        }

        public override void Write(PacketWriter w)
        {
            w.Write(this.Time);
            this.Position.Write(w);
            this.SlotObject1.Write(w);
            this.SlotObject2.Write(w);
        }
    }
}
