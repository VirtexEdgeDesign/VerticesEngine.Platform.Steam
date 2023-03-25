using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using VerticesEngine.Net.Messages;


namespace VerticesEngine.Platforms.Steam.Networking
{
    internal struct vxNetMessageIncomingSteamWrapper : vxINetMessageIncoming
    {
        public IPEndPoint SenderEndPoint => throw new NotImplementedException();

        public void ReadAllFields(object target)
        {
            throw new NotImplementedException();
        }

        public bool ReadBoolean()
        {
            throw new NotImplementedException();
        }

        public byte ReadByte()
        {
            throw new NotImplementedException();
        }

        public byte[] ReadByteArray(int length)
        {
            throw new NotImplementedException();
        }

        public double ReadDouble()
        {
            throw new NotImplementedException();
        }

        public float ReadFloat()
        {
            throw new NotImplementedException();
        }

        public short ReadInt16()
        {
            throw new NotImplementedException();
        }

        public int ReadInt32()
        {
            throw new NotImplementedException();
        }

        public long ReadInt64()
        {
            throw new NotImplementedException();
        }

        public Quaternion ReadQuaternion()
        {
            throw new NotImplementedException();
        }

        public string ReadString()
        {
            throw new NotImplementedException();
        }

        public double ReadTime()
        {
            throw new NotImplementedException();
        }

        public Vector2 ReadVector2()
        {
            throw new NotImplementedException();
        }

        public Vector3 ReadVector3()
        {
            throw new NotImplementedException();
        }
    }
}