using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NillyProxy.Packets
{
    public class PacketWriter : BinaryWriter
    {
        public PacketWriter(MemoryStream input)
            : base(input)
        {
        }

        public override void Write(short value)
        {
            base.Write(IPAddress.NetworkToHostOrder(value));
        }

        public override void Write(ushort value)
        {
            base.Write((ushort)IPAddress.HostToNetworkOrder((short)value));
        }

        public override void Write(int value)
        {
            base.Write(IPAddress.NetworkToHostOrder(value));
        }

        public override void Write(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            base.Write(bytes);
        }

        public override void Write(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            this.Write((short)bytes.Length);
            base.Write(bytes);
        }

        public void WriteUTF32(string value)
        {
            this.Write(value.Length);
            this.Write(Encoding.UTF8.GetBytes(value));
        }

        public static void BlockCopyInt32(byte[] data, int int32)
        {
            byte[] bytes = BitConverter.GetBytes(IPAddress.NetworkToHostOrder(int32));
            data[0] = bytes[0];
            data[1] = bytes[1];
            data[2] = bytes[2];
            data[3] = bytes[3];
        }
    }
}
