using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NillyProxy.Packets.ClientPackets
{
    public class PlayerHitPacket : Packet
    {
        public byte BulletId;

        public int ObjectId;

        public override PacketType Type
        {
            get
            {
                return PacketType.PLAYERHIT;
            }
        }

        public override void Read(PacketReader r)
        {
            this.BulletId = r.ReadByte();
            this.ObjectId = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(this.BulletId);
            w.Write(this.ObjectId);
        }
    }
}
