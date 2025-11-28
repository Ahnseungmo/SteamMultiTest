using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using TMPro;

public class LobbyListItem : MonoBehaviour
{
    public Text lobbyNameText;

    public TextMeshProUGUI lobbyText;
    private CSteamID lobbyId;

    public void SetLobby(CSteamID id)
    {
        lobbyId = id;
        string name = SteamMatchmaking.GetLobbyData(id, "name");
//        lobbyNameText.text = string.IsNullOrEmpty(name) ? "No Name" : name;
        lobbyText.text = string.IsNullOrEmpty(name) ? "No Name" : name;
    }

    public void OnClickJoin()
    {
        SteamLobby.Instance.JoinLobby(lobbyId);
    }
}
