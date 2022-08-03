using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.SceneManagement;

namespace Game.Network
{
    public class CustomNetworkManager : NetworkManager
    {
        [SerializeField] private PlayerObjectController GamePlayerPrefab;
        public List<PlayerObjectController> GamePlayers { get; } = new List<PlayerObjectController>();

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            if (SceneManager.GetActiveScene().name == "Lobby")
            {
                PlayerObjectController gamePlayerInstance = Instantiate(GamePlayerPrefab);
                gamePlayerInstance.connectionID = conn.connectionId;
                gamePlayerInstance.playerIdNumber = GamePlayers.Count + 1;
                gamePlayerInstance.playerSteamID =
                    (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.instance.currentLobbyID, GamePlayers.Count);

                NetworkServer.AddPlayerForConnection(conn, gamePlayerInstance.gameObject);
            }
        }
        
        
        
    }
}
