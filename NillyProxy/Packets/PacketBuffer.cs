using NillyProxy.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NillyProxy.Packets
{
    public class PacketBuffer
    {
        private byte[] _buffer;

        private int _index;

        public PacketBuffer()
        {
            this.Flush();
        }

        public void Resize(int newSize)
        {
            Array.Resize<byte>(ref this._buffer, newSize);
        }

        public void Advance(int numBytes)
        {
            this._index += numBytes;
        }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < _buffer.Length; i++)
            {
                result += String.Format("{0:X2} ", _buffer[i]);
            }
            return result;
        }

        public void Flush()
        {
            this._buffer = new byte[4];
            this._index = 0;
        }

        public int BytesRemaining()
        {
            return this._buffer.Length - this._index;
        }

        public byte[] Buffer()
        {
            return this._buffer;
        }

        public int Length()
        {
            return this._buffer.Length;
        }

        public int Index()
        {
            return this._index;
        }
    }
}
