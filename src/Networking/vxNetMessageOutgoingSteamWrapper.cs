#if __STEAM__ //  Only supported in the steam implementation

using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.Net.Messages;

namespace VerticesEngine.Platforms.Steam.Networking
{
    internal struct vxNetMessageOutgoingSteamWrapper : vxINetMessageOutgoing
    {
        /// <summary>
        /// The underlying Lidren Message
        /// </summary>
        internal NetOutgoingMessage dataWriter;

        public vxNetMessageOutgoingSteamWrapper(NetOutgoingMessage om)
        {
            dataWriter = om;
        }

        public void Write(byte source)
        {
            dataWriter.Write(source);
        }
        public void Write(byte[] bytes)
        {
            dataWriter.Write(bytes);
        }

        public void Write(string source)
        {
            dataWriter.Write(source);
        }
        public void Write(int source)
        {
            dataWriter.Write(source);
        }

        public void Write(double source)
        {
            dataWriter.Write(source);
        }

        public void Write(float source)
        {
            dataWriter.Write(source);
        }

        public void Write(long source)
        {
            dataWriter.Write(source);
        }
        public void Write(bool source)
        {
            dataWriter.Write(source);
        }

        public void Write(Vector2 source)
        {
            dataWriter.Write(source);
        }

        public void Write(Vector3 source)
        {
            dataWriter.Write(source);
        }

        public void Write(Quaternion source)
        {
            dataWriter.WriteRotation(source, 24);
        }

        public void WriteAllFields(object source)
        {
            dataWriter.WriteAllFields(source);
        }

        public void WriteTime()
        {
            throw new NotImplementedException();
        }
    }
}

#endif