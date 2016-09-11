using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Reflection;
using System.IO;
using NillyProxy.Packets;
using NillyProxy.Proxy;

namespace NillyProxy
{
    public static class Serializer
    {
        public static Dictionary<byte, Type> PacketIdTypeMap = new Dictionary<byte, Type>();
        public static Dictionary<Type, byte> PacketTypeIdMap = new Dictionary<Type, byte>();
        public static Dictionary<PacketType, Type> PacketTypeTypeMap = new Dictionary<PacketType, Type>();

        public static void SerializePacketIds()
        {
            string text = Serializer.DEBUGGetSolutionRoot() + "/XML/packets.xml";
            bool flag = File.Exists(text);
            if (flag)
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(text);
                foreach (XmlNode xmlNode in xmlDocument.DocumentElement.ChildNodes)
                {
                    string text2 = "";
                    byte b = 255;
                    foreach (XmlNode xmlNode2 in xmlNode.ChildNodes)
                    {
                        string a = xmlNode2.Name.ToLower();
                        if (!(a == "packetname"))
                        {
                            if (a == "packetid")
                            {
                                b = byte.Parse(xmlNode2.InnerText);
                            }
                        }
                        else
                        {
                            text2 = xmlNode2.InnerText;
                        }
                    }
                    bool flag2 = text2 != "" && b != 255;
                    if (flag2)
                    {
                        PacketType packetType;
                        bool flag3 = Enum.TryParse<PacketType>(text2, true, out packetType);
                        Type p = Serializer.getPacketByType(packetType);
                        bool flag4 = (p != typeof(Packet));
                        if (flag3 && flag4)
                        {
                            Serializer.PacketIdTypeMap.Add(b, PacketTypeTypeMap[packetType]);
                            Serializer.PacketTypeIdMap.Add(PacketTypeTypeMap[packetType], b);
                        }
                    }
                }
                Console.WriteLine("[Serializer] Serialized {0} packet ids successfully.", Serializer.PacketTypeIdMap.Count);
                return;
            }
            throw new FileNotFoundException("Unable to find file.", text);
        }

        public static void SerializePacketTypes()
        {
            Type tPacket = typeof(Packet);
            Type[] array = (from t in Assembly.GetAssembly(typeof(ProxyServer)).GetTypes()
                            where tPacket.IsAssignableFrom(t)
                            select t).ToArray<Type>();
            Type[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                Type type = array2[i];
                PacketType type2 = (Activator.CreateInstance(type) as Packet).Type;
                Serializer.PacketTypeTypeMap.Add(type2, type);
            }
            Console.WriteLine("[Serializer] Mapped {0} packet structures successfully.", Serializer.PacketTypeTypeMap.Count);
        }

        public static Type getPacketById(byte id)
        {
            if (Serializer.PacketIdTypeMap.ContainsKey(id))
            {
                return Serializer.PacketIdTypeMap[id];
            }
            return typeof(Packet);
        }

        public static Type getPacketByType(PacketType type)
        {
            if (Serializer.PacketTypeTypeMap.ContainsKey(type))
            {
                return Serializer.PacketTypeTypeMap[type];
            }
            return typeof(Packet);
        }

        public static byte getPacketId(Type type)
        {
            if (Serializer.PacketTypeIdMap.ContainsKey(type))
            {
                return Serializer.PacketTypeIdMap[type];
            }
            return 255;
        }

        public static string DEBUGGetSolutionRoot()
        {
            DirectoryInfo parent = Directory.GetParent(Directory.GetParent(Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString()).ToString());
            bool flag = Directory.Exists(parent.ToString() + "/XML/");
            string result;
            if (flag)
            {
                result = parent.ToString();
            }
            else
            {
                result = System.Reflection.Assembly.GetExecutingAssembly().Location;
            }
            return result;
        }
    }
}
