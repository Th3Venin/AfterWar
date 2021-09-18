using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Client : MonoBehaviour
{
    public static Client instance;

    public static int bufferSize = 4096;
    public int id;
    public int gameId;

    public delegate void PacketHandler(Packet _packet);
    public static Dictionary<int, PacketHandler> packetHandlers;

    public TcpConnection gameTcp;
    public UdpConnection udp;
    public string username;

    public string gameIp = "192.168.0.20";
    public int gamePort = 26033;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        ConnectToGame();
    }

    private void OnApplicationQuit()
    {
        if (gameTcp != null)    
        {
            gameTcp.Disconnect();
            udp.Disconnect();
        }
    }

    public void ConnectToGame()
    {
        Debug.Log("Trying to connect to game...");
        InitializeClientData();

        gameTcp = new TcpConnection(gameIp, gamePort, bufferSize);
        udp = new UdpConnection();
    }

    private void InitializeClientData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            { (int)ServerPackets.welcomeToGame, ClientResponse.WelcomeToGame},
            { (int)ServerPackets.spawnPlayer, ClientResponse.SpawnPlayer},
            { (int)ServerPackets.playerMovement, ClientResponse.PlayerMovement},
            { (int)ServerPackets.playerDisconnected, ClientResponse.PlayerDisconnected},
            { (int)ServerPackets.createItemSpawner, ClientResponse.CreateItemSpawner},
            { (int)ServerPackets.itemSpawned, ClientResponse.ItemSpawned},
            { (int)ServerPackets.itemPickedUp, ClientResponse.ItemPickedUp},
            { (int)ServerPackets.playerHealth, ClientResponse.PlayerHealth},
            { (int)ServerPackets.equippedWeapon, ClientResponse.EquippedWeapon},
            { (int)ServerPackets.unEquippedWeapon, ClientResponse.UnEquippedWeapon},
            { (int)ServerPackets.playerArmor, ClientResponse.PlayerArmor},
            { (int)ServerPackets.playerAmmo, ClientResponse.PlayerAmmo},
            { (int)ServerPackets.playerWeapons, ClientResponse.PlayerWeapons},
            { (int)ServerPackets.playerEliminated, ClientResponse.PlayerEliminated},
            { (int)ServerPackets.playerChosenSpawn, ClientResponse.PlayerChosenSpawn},
            { (int)ServerPackets.matchStage, ClientResponse.MatchStage},
            { (int)ServerPackets.serverTime, ClientResponse.ServerTime},
            { (int)ServerPackets.gameInProgress, ClientResponse.GameInProgress},
            { (int)ServerPackets.gameFull, ClientResponse.GameFull},
            { (int)ServerPackets.playerWon, ClientResponse.PlayerWon}

        };

        Debug.Log("Initialized packets.");
    }

    public void Disconnect()
    {
        gameTcp.Disconnect();
        udp.Disconnect();
    }
}
