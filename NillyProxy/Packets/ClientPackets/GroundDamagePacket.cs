using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NillyProxy.Packets.DataObjects;

namespace NillyProxy.Packets.ClientPackets
{
    public class GroundDamagePacket : Packet
    {
        public int Time;

        public Location Position;

        public override PacketType Type
        {
            get
            {
                return PacketType.GROUNDDAMAGE;
            }
        }

        public override void Read(PacketReader r)
        {
            this.Time = r.ReadInt32();
            this.Position = (Location)new Location().Read(r);
        }

        public override void Write(PacketWriter w)
        {
            w.Write(this.Time);
            this.Position.Write(w);
        }
    }
}
