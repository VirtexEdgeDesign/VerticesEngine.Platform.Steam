using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.Net.Messages;
using Facepunch.Steamworks;
using VerticesEngine.Net;

namespace VerticesEngine.Platforms.Steam.Networking
{
    /// <summary>
    /// The Steam Server wrapper handles the Steam P2P Sessions. It isn't so much a server as it is a lobby and 'host' for a netwworked session.
    /// It also allows for a single user to be the 'Authority' on game events.
    /// </summary>
    internal class vxNetworkSteamServerWrapper : vxINetworkServerBackend
    {
        vxNetworkServer _netServerManager;

        private bool disposedValue;

        public string ServerName
        {
            get { return _serverName; }
        }
        private string _serverName = "steamP2Pserver";

        public int Port
        {
            get { return _port; }
        }
        private int _port = 0;

        public bool IsAcceptingIncomingConnections { get => _isAcceptingIncomingConnections; set => _isAcceptingIncomingConnections = value; }
        private bool _isAcceptingIncomingConnections = false;

        public vxNetworkSteamServerWrapper(vxNetworkServer netServerManager, string serverName, int port)
        {
            _netServerManager = netServerManager;
            _serverName = serverName;
            _port = port;
        }

        public void Start()
        {

        }

        public void Shutdown()
        {

        }

        public vxINetMessageOutgoing CreateMessage()
        {
            return new vxNetMessageOutgoingSteamWrapper();
        }

        public void DebugDraw()
        {
            //throw new NotImplementedException();
        }

        public double GetCurrentNetTime()
        {
            return DateTime.Now.Ticks;
        }

        public void Initialise()
        {

        }

        public vxINetMessageIncoming ReadMessage()
        {
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
        // ~vxNetworkSteamServerWrapper()
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
    }
}