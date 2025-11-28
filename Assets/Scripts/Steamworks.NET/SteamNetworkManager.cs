using UnityEngine;
using Steamworks;

public class SteamNetworkManager : MonoBehaviour
{
    public static SteamNetworkManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SendMessage(string message, CSteamID to)
    {
        byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
        SteamNetworking.SendP2PPacket(to, data, (uint)data.Length, EP2PSend.k_EP2PSendReliable);
    }

    private void Update()
    {
        uint size;

        if (SteamNetworking.IsP2PPacketAvailable(out size))
        {
            byte[] buffer = new byte[size];
            uint bytesRead;
            CSteamID sender;

            if (SteamNetworking.ReadP2PPacket(buffer, size, out bytesRead, out sender))
            {
                string msg = System.Text.Encoding.UTF8.GetString(buffer);
                Debug.Log($"[Recv] {sender}: {msg}");
            }
        }
    }

    public void CloseConnection(CSteamID user)
    {
        SteamNetworking.CloseP2PSessionWithUser(user);
    }
}
