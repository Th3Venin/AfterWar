using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UdpConnection
{
    public IPEndPoint endPoint;
    public UdpClient socket;

    public UdpConnection()
    {
        endPoint = new IPEndPoint(IPAddress.Parse(Client.instance.gameIp), Client.instance.gamePort);
    }

    public void Connect(int localport)
    {
        socket = new UdpClient(localport);

        socket.Connect(endPoint);
        socket.BeginReceive(ReceiveCallback, null);

        using (Packet _packet = new Packet())
        {
            SendData(_packet);
        }

        Debug.Log("Connected via UDP to game server");
    }

    private void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            byte[] data = socket.EndReceive(result, ref endPoint);
            socket.BeginReceive(ReceiveCallback, null);

            if (data.Length < 4)
            {
                Debug.LogError("Error handling UDP, disconnecting");
                Disconnect();
                return;
            }

            HandleData(data);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error handling UDP, disconnecting " + ex);
            Disconnect();
        }
    }

    public void SendData(Packet packet)
    {
        try
        {
            packet.InsertInt(Client.instance.gameId); // Insert the client's ID at the start of the packet
            if (socket != null)
            {
                socket.BeginSend(packet.ToArray(), packet.Length(), null, null);
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Error sending data to server via UDP: {ex}");
        }
    }

    /// <summary>Prepares received data to be used by the appropriate packet handler methods.</summary>
    /// <param name="_packetData">The packet containing the recieved data.</param>
    public void HandleData(byte[] data)
    {

        using (Packet packet = new Packet(data))
        {
            int packetLength = packet.ReadInt();
            data = packet.ReadBytes(packetLength);
        }

        ThreadManager.ExecuteOnMainThread(() =>
        {
            using (Packet packet = new Packet(data))
            {
                int packetId = packet.ReadInt();
                Client.packetHandlers[packetId](packet); // Call appropriate method to handle the packet
            }
        });
    }

    /// <summary>Cleans up the UDP connection.</summary>
    public void Disconnect()
    {
        if (socket != null)
            socket.Close();
        endPoint = null;
        socket = null;
    }
}