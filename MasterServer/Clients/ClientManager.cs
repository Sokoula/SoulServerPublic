﻿using System;
using System.Collections.Generic;
using MasterServer.Properties;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Tools;

namespace MasterServer.Clients
{
    internal static class ClientManager
    {
        public static Dictionary<ShortGuid, Client> _connectedClients;

        public static void Initialize()
        {
            Console.WriteLine(Resources.ClientManager_Initialize_Load);
            _connectedClients = new Dictionary<ShortGuid, Client>();
            Console.WriteLine(Resources.ClientManager_Initialize_Success);
        }

        public static void UpdateOrAddClient(Connection connection, out Client client)
        {
            
            var guid = connection.ConnectionInfo.NetworkIdentifier;
            Console.WriteLine(Resources.ClientManager_UpdateOrAddClient_Load + guid);

            if (_connectedClients.ContainsKey(guid))
            {
                client = _connectedClients[guid];
                switch (client.getAtZone())
                {
                    case atZone.afterLogin:
                        client.ServerConnection = connection;
                        client.setAtZone(atZone.atCharSelect);
                        return;
                    case atZone.atCharSelect:
                        client.ChannelConnection = connection;
                        return;
                }
                _connectedClients[guid].AuthConnection = connection;
                _connectedClients[guid].setAtZone(atZone.connectedToServer);
                Console.WriteLine(Resources.ClientManager_UpdateOrAddClient_UpdateSuccess, guid);
                return;
            }
            client = new Client(connection,guid);
            _connectedClients.Add(guid, client);
            Console.WriteLine(Resources.ClientManager_UpdateOrAddClient_ClientAddSuccess,guid);
        }
        

        public static void RemoveClient(Connection connection)
        {
            var guid = connection.ConnectionInfo.NetworkIdentifier;
            Console.WriteLine(Resources.ClientManager_RemoveClient_Load_+guid);

            if (_connectedClients.TryGetValue(guid, out var client))
            {
                _connectedClients.Remove(guid);
                Console.WriteLine(Resources.ClientManager_RemoveClient_Success+guid);
                return;

            }
            Console.WriteLine(Resources.ClientManager_RemoveClient_Error);

        }

        public static Client GetClient(Connection connection)
        {
            return _connectedClients.TryGetValue(connection.ConnectionInfo.NetworkIdentifier, out var client) ? client : null;
        }
    }
}
