using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginClient : MonoBehaviour
{
    public static LoginClient instance;
    public int id;
    public delegate void PacketHandler(Packet _packet);
    public static Dictionary<int, PacketHandler> packetHandlers;
    public static int bufferSize = 4096;

    public TcpConnection tcp;

    public string ip = "127.0.0.1";
    public int port = 26032;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            ConnectToServer();
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    private void OnApplicationQuit()
    {
        if (tcp != null)
        {
            tcp.Disconnect();
        }
    }

    public void ConnectToServer()
    {
        Debug.Log("Trying to connect to server...");
        InitializeClientData();

        tcp = new TcpConnection(ip, port, bufferSize);
    }

    private void InitializeClientData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            { (int)ServerPackets.welcome, ClientResponse.WelcomeToGame},
            { (int)ServerPackets.successRegisterResponse, ClientResponse.WelcomeToGame},
            { (int)ServerPackets.successLoginResponse, ClientResponse.WelcomeToGame},
            { (int)ServerPackets.duplicateEmailRegisterResponse, ClientResponse.WelcomeToGame},
            { (int)ServerPackets.duplicateUsernameRegisterResponse, ClientResponse.WelcomeToGame},
            { (int)ServerPackets.wrongDetailsLoginResponse, ClientResponse.WelcomeToGame},
            { (int)ServerPackets.foundGame, ClientResponse.WelcomeToGame},
        };

        Debug.Log("Initialized packets.");
    }
}
