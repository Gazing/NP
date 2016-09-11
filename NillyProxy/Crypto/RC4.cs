using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NillyProxy.Crypto
{
    public class RC4
    {
        private static readonly int STATE_LENGTH = 256;

        private byte[] engineState;

        private int x;

        private int y;

        private byte[] workingKey;

        public RC4(byte[] key)
        {
            this.workingKey = key;
            this.SetKey(this.workingKey);
        }

        public RC4(string hexString)
        {
            this.workingKey = RC4.HexStringToBytes(hexString);
            this.SetKey(this.workingKey);
        }

        public void Cipher(byte[] packet)
        {
            this.ProcessBytes(packet, 5, packet.Length - 5, packet, 5);
        }

        public void Reset()
        {
            this.SetKey(this.workingKey);
        }

        private void ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff)
        {
            int num;
            for (int i = 0; i < length; i = num + 1)
            {
                this.x = (this.x + 1 & 255);
                this.y = ((int)this.engineState[this.x] + this.y & 255);
                byte b = this.engineState[this.x];
                this.engineState[this.x] = this.engineState[this.y];
                this.engineState[this.y] = b;
                output[i + outOff] = (byte)(input[i + inOff] ^ this.engineState[(int)(this.engineState[this.x] + this.engineState[this.y] & 255)]);
                num = i;
            }
        }

        private void SetKey(byte[] keyBytes)
        {
            this.workingKey = keyBytes;
            this.x = (this.y = 0);
            bool flag = this.engineState == null;
            if (flag)
            {
                this.engineState = new byte[RC4.STATE_LENGTH];
            }
            int num;
            for (int i = 0; i < RC4.STATE_LENGTH; i = num + 1)
            {
                this.engineState[i] = (byte)i;
                num = i;
            }
            int num2 = 0;
            int num3 = 0;
            for (int j = 0; j < RC4.STATE_LENGTH; j = num + 1)
            {
                num3 = ((int)((keyBytes[num2] & 255) + this.engineState[j]) + num3 & 255);
                byte b = this.engineState[j];
                this.engineState[j] = this.engineState[num3];
                this.engineState[num3] = b;
                num2 = (num2 + 1) % keyBytes.Length;
                num = j;
            }
        }

        public static byte[] HexStringToBytes(string key)
        {
            bool flag = key.Length % 2 != 0;
            if (flag)
            {
                throw new ArgumentException("Invalid hex string!");
            }
            byte[] array = new byte[key.Length / 2];
            char[] array2 = key.ToCharArray();
            for (int i = 0; i < array2.Length; i += 2)
            {
                StringBuilder stringBuilder = new StringBuilder(2).Append(array2[i]).Append(array2[i + 1]);
                int num = Convert.ToInt32(stringBuilder.ToString(), 16);
                array[i / 2] = (byte)num;
            }
            return array;
        }
    }
}
