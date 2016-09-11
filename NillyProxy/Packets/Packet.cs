using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NillyProxy.Packets
{
    public class Packet
    {
        public byte id;
        private byte[] data;
        public bool send = true;

        public virtual PacketType Type
        {
            get
            {
                return PacketType.UNKNOWN;
            }
        }

        public virtual void Read(PacketReader r)
        {
            this.data = r.ReadBytes((int)r.BaseStream.Length - 5);
        }

        public virtual void Write(PacketWriter w)
        {
            w.Write(this.data);
        }

        public static Packet Create(PacketType type)
        {
            Packet packet = (Packet)Activator.CreateInstance(Serializer.getPacketByType(type));
            packet.id = Serializer.getPacketId(Serializer.getPacketByType(type));
            return packet;
        }

        public static Packet Create(byte[] data)
        {
            Packet result;
            using (PacketReader packetReader = new PacketReader(new MemoryStream(data)))
            {
                packetReader.ReadInt32();
                byte id = packetReader.ReadByte();
                Type packetType = Serializer.getPacketById(id);
                Packet packet = (Packet)Activator.CreateInstance(packetType);
                packet.id = id;
                packet.Read(packetReader);
                result = packet;
            }
            return result;
        }
    }
}
