using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
public class TcpConnection
{
    public TcpClient socket;

    private Packet receivedData;
    private byte[] receiveBuffer;
    private NetworkStream stream;
    private int bufferSize;

    public TcpConnection(string ip, int port, int bufferSize)
    {
        socket = new TcpClient
        {
            ReceiveBufferSize = bufferSize,
            SendBufferSize = bufferSize
        };

        receiveBuffer = new byte[bufferSize];
        socket.BeginConnect(ip, port, ConnectCallback, socket);
        LoadingManager.instance.connectSendTime = DateTime.Now;
        this.bufferSize = bufferSize;
    }

    public void SendData(Packet packet)
    {
        try
        {
            if (socket != null)
            {
                stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null); // Send data to appropriate client
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending data {ex}");
        }
    }

    private void ConnectCallback(IAsyncResult _result)
    {
        socket.EndConnect(_result);

        if (!socket.Connected)
        {
            return;
        }

        stream = socket.GetStream();

        receivedData = new Packet();
        stream.BeginRead(receiveBuffer, 0, bufferSize, ReceiveCallback, null);
        Debug.Log("Connected to server!");
        LoadingManager.instance.connected = true;
    }

    private void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            int byteLength = stream.EndRead(result);
            if (byteLength < 0)
            {
                Debug.LogError($"Error receiving TCP data.");
                Disconnect();
                return;
            }

            byte[] data = new byte[byteLength];
            Array.Copy(receiveBuffer, data, byteLength);
            receivedData.Reset(HandleData(data));

            stream.BeginRead(receiveBuffer, 0, bufferSize, ReceiveCallback, null);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error receiving TCP data: {ex}");
            Disconnect();
        }
    }

    private bool HandleData(byte[] data)
    {
        int packetLength = 0;

        receivedData.SetBytes(data);

        if (receivedData.UnreadLength() >= 4)
        {
            // If client's received data contains a packet
            packetLength = receivedData.ReadInt();
            if (packetLength <= 0)
            {
                // If packet contains no data
                return true; // Reset receivedData instance to allow it to be reused
            }
        }

        while (packetLength > 0 && packetLength <= receivedData.UnreadLength())
        {
            // While packet contains data AND packet data length doesn't exceed the length of the packet we're reading
            byte[] packetBytes = receivedData.ReadBytes(packetLength);
            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet packet = new Packet(packetBytes))
                {
                    int packetId = packet.ReadInt();
                    Client.packetHandlers[packetId](packet); // Call appropriate method to handle the packet
                }
            });

            packetLength = 0; // Reset packet length
            if (receivedData.UnreadLength() >= 4)
            {
                // If client's received data contains another packet
                packetLength = receivedData.ReadInt();
                if (packetLength <= 0)
                {
                    // If packet contains no data
                    return true; // Reset receivedData instance to allow it to be reused
                }
            }
        }

        if (packetLength <= 1)
        {
            return true; // Reset receivedData instance to allow it to be reused
        }

        return false;
    }

    public void Disconnect()
    {
        socket.Close();
    }
}
