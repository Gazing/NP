using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NillyProxy.Packets.ClientPackets
{
    public class CheatingPacket : Packet
    {
        public override PacketType Type
        {
            get
            {
                return PacketType.CHEATING;
            }
        }
    }
}
