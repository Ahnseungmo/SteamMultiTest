using UnityEngine;
using System.Collections.Generic;
using Steamworks;

public class LobbyUIManager : MonoBehaviour
{
    public static LobbyUIManager Instance;

    public GameObject lobbyItemPrefab;
    public Transform lobbyListParent;

    private List<GameObject> currentItems = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateLobbyList(List<CSteamID> list)
    {
        foreach (var item in currentItems)
            Destroy(item);
        currentItems.Clear();

        foreach (var lobbyId in list)
        {
            GameObject obj = Instantiate(lobbyItemPrefab, lobbyListParent);
            currentItems.Add(obj);

            var item = obj.GetComponent<LobbyListItem>();
            item.SetLobby(lobbyId);
        }
    }

    public void OnClickRefresh()
    {
        SteamLobby.Instance.RequestLobbyList();
    }
}
