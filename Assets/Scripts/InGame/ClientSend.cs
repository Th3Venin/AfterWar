using UnityEngine;

class ClientSend
{
    private static void SendUDPData(Packet packet)
    {
        packet.WriteLength();
        Client.instance.udp.SendData(packet);
    }

    private static void SendGameServerTCPData(Packet packet)
    {
        packet.WriteLength();
        Client.instance.gameTcp.SendData(packet);
    }

    private static void SendLoginServerData(Packet packet)
    {
        packet.WriteLength();
        LoginClient.instance.tcp.SendData(packet);
    }

    public static void Register(string name, string username, string email, string password)
    {
        using (Packet packet = new Packet((int)ClientPackets.registerRequest))
        {
            packet.Write(Client.instance.id);
            packet.Write(name);
            packet.Write(email);
            packet.Write(username);
            packet.Write(password);

            SendLoginServerData(packet);
        }
    }

    public static void Login(string username, string password)
    {
        using (Packet packet = new Packet((int)ClientPackets.loginRequest))
        {
            packet.Write(Client.instance.id);
            packet.Write(username);
            packet.Write(password);

            SendLoginServerData(packet);
        }
    }

    public static void FindGame()
    {
        using (Packet packet = new Packet((int)ClientPackets.playGame))
        {
            packet.Write(Client.instance.id);
            SendLoginServerData(packet);
        }
    }

    public static void WelcomeReceived(int gameId, string username)
    {
        using (Packet packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            packet.Write(gameId);
            packet.Write(username);
            SendGameServerTCPData(packet);
        }
    }

    public static void PlayerMovement(PlayerMovement playerMovement)
    {
        using (Packet packet = new Packet((int)ClientPackets.playerMovement))
        {
            Vector3 pos = playerMovement.transform.position;
            pos.y -= 1.6f;
            packet.Write(pos);
            packet.Write(playerMovement.transform.rotation);
            packet.Write(playerMovement.camTransform.rotation);
            if (playerMovement.spine == null)
                packet.Write(0);
            else
                packet.Write(playerMovement.spine.xRotation);
            packet.Write(playerMovement.keysPressed.Length);
            packet.Write(playerMovement.isGrounded);

            foreach (bool key in playerMovement.keysPressed)
            {
                packet.Write(key);
            }

            SendUDPData(packet);
        }
    }

    public static void EquipWeapon(int slot)
    {
        using (Packet packet = new Packet((int)ClientPackets.equipWeapon))
        {
            packet.Write(slot);

            SendGameServerTCPData(packet);
        }
    }

    public static void WeaponDropped(WeaponTypes type)
    { 
        using (Packet packet = new Packet((int)ClientPackets.dropWeapon))
        {
            packet.Write((int)type);

            SendGameServerTCPData(packet);
        }
    }

    public static void Interact()
    {
        using (Packet packet = new Packet((int)ClientPackets.interact))
        {
            SendGameServerTCPData(packet);
        }
    }

    public static void Reload()
    {
        using (Packet packet = new Packet((int)ClientPackets.reload))
        {
            SendGameServerTCPData(packet);
        }
    }

    public static void ChooseSpawn(Vector2 spawn)
    {
        using (Packet packet = new Packet((int)ClientPackets.chooseSpawn))
        {
            packet.Write(spawn);
            SendGameServerTCPData(packet);
        }
    }

}