using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NillyProxy.Packets.DataObjects
{
    public class Location : IDataObject, ICloneable
    {
        public float X;

        public float Y;

        public static Location Empty
        {
            get
            {
                return new Location
                {
                    X = 0f,
                    Y = 0f
                };
            }
        }

        public virtual IDataObject Read(PacketReader r)
        {
            this.X = r.ReadSingle();
            this.Y = r.ReadSingle();
            return this;
        }

        public virtual void Write(PacketWriter w)
        {
            w.Write(this.X);
            w.Write(this.Y);
        }

        public float DistanceSquaredTo(Location location)
        {
            float num = location.X - this.X;
            float num2 = location.Y - this.Y;
            return num * num + num2 * num2;
        }

        public float DistanceTo(Location location)
        {
            return (float)Math.Sqrt((double)this.DistanceSquaredTo(location));
        }

        public virtual object Clone()
        {
            return new Location
            {
                X = this.X,
                Y = this.Y
            };
        }

        public override string ToString()
        {
            return string.Concat(new object[]
			{
				"{ X=",
				this.X,
				", Y=",
				this.Y,
				" }"
			});
        }
    }
}
