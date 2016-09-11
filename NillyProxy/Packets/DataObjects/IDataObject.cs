using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NillyProxy.Packets.DataObjects
{
    public interface IDataObject : ICloneable
    {
        IDataObject Read(PacketReader r);

        void Write(PacketWriter w);
    }
}
