using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.Net;
using VerticesEngine.Net.Messages;


namespace VerticesEngine.Platforms.Steam.Networking
{
    internal class vxNetworkSteamClientWrapper : vxINetworkClientBackend
    {
        private bool disposedValue;

        public void Connect(string Address, int Port)
        {
            throw new NotImplementedException();
        }

        public void Connect(string Address, int Port, vxINetMessageOutgoing hail)
        {
            throw new NotImplementedException();
        }

        public vxINetMessageOutgoing CreateMessage()
        {
            throw new NotImplementedException();
        }

        public void DebugDraw()
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public double GetCurrentNetTime()
        {
            throw new NotImplementedException();
        }

        public void Initialise()
        {
            throw new NotImplementedException();
        }

        public vxINetMessageIncoming ReadMessage()
        {
            throw new NotImplementedException();
        }

        public void Recycle(vxINetMessageIncoming im)
        {
            throw new NotImplementedException();
        }

        public void SendDiscoverySignal(int port)
        {
            // Discover any steam related 
            using (var client = new Facepunch.Steamworks.Client(100))
            {
                client.Lobby.Create(Facepunch.Steamworks.Lobby.Type.Public, 4);
                
            }


            throw new NotImplementedException();
        }

        public void SendMessage(vxINetworkMessage gameMessage)
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~vxNetworkSteamClientWrapper()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void SendLocalDiscoverySignal(int port)
        {
            throw new NotImplementedException();
        }

        public void SendDiscoverySignal(string ip, int port)
        {
            throw new NotImplementedException();
        }
    }
}