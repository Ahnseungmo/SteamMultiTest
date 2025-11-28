using Steamworks;
using System.Collections.Generic;
using UnityEngine;

public class SteamLobby : MonoBehaviour
{
    public static SteamLobby Instance;

    public CSteamID CurrentLobbyID;
    public const string LOBBY_NAME_KEY = "name";
    public const string LOBBY_HOST_STEAMID = "host";

    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<LobbyEnter_t> LobbyEntered;
    protected Callback<LobbyMatchList_t> LobbyMatchList;
    protected Callback<LobbyDataUpdate_t> LobbyDataUpdated;

    public List<CSteamID> LobbyList = new List<CSteamID>();
    private void Awake()
    {
        Instance = this;

        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        LobbyMatchList = Callback<LobbyMatchList_t>.Create(OnLobbyMatchList);
        LobbyDataUpdated = Callback<LobbyDataUpdate_t>.Create(OnLobbyDataUpdated);
    }

    public void CreateLobby(int maxPlayers = 4)
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, maxPlayers);
    }

    private void OnLobbyCreated(LobbyCreated_t result)
    {
        if (result.m_eResult == EResult.k_EResultOK)
        {
            CurrentLobbyID = new CSteamID(result.m_ulSteamIDLobby);
            Debug.Log("Lobby created: " + CurrentLobbyID);
            Debug.Log(SteamFriends.GetPersonaName() + "'s Room");

            SteamMatchmaking.SetLobbyData(CurrentLobbyID, LOBBY_NAME_KEY, SteamFriends.GetPersonaName() + "'s Room");
            SteamMatchmaking.SetLobbyData(CurrentLobbyID, LOBBY_HOST_STEAMID, SteamUser.GetSteamID().ToString());
    
        }
    }

    public void RequestLobbyList()
    {
        LobbyList.Clear();
        SteamMatchmaking.RequestLobbyList();   // Start searching
        Debug.Log("Requesting lobby list...");
    }

    private void OnLobbyMatchList(LobbyMatchList_t result)
    {
        Debug.Log($"Lobby Found Count: {result.m_nLobbiesMatching}");

        for (int i = 0; i < result.m_nLobbiesMatching; i++)
        {
            CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
            LobbyList.Add(lobbyID);

            // Request data load
            SteamMatchmaking.RequestLobbyData(lobbyID);
        }

        LobbyUIManager.Instance.UpdateLobbyList(LobbyList);
    }

    private void OnLobbyDataUpdated(LobbyDataUpdate_t result)
    {
        LobbyUIManager.Instance.UpdateLobbyList(LobbyList);
    }

    public void JoinLobby(CSteamID id)
    {
        SteamMatchmaking.JoinLobby(id);
    }



    private void OnLobbyEntered(LobbyEnter_t result)
    {
        CurrentLobbyID = new CSteamID(result.m_ulSteamIDLobby);

        Debug.Log("Entered Lobby: " + CurrentLobbyID);

        if (SteamMatchmaking.GetLobbyOwner(CurrentLobbyID) == SteamUser.GetSteamID())
            Debug.Log("You are Host");
        else
            Debug.Log("You are Client");
    }

    public void LeaveLobby()
    {
        SteamMatchmaking.LeaveLobby(CurrentLobbyID);
        Debug.Log("Lobby Left");
        CurrentLobbyID = CSteamID.Nil;
    }

    public bool IsHost()
    {
        return SteamMatchmaking.GetLobbyOwner(CurrentLobbyID) == SteamUser.GetSteamID();
    }
}
