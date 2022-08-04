using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.UI;
using System.Linq;
using Game.Network;
using Unity.Services.Lobbies.Models;

namespace Game.Network.Steamworks
{
    public class LobbyController : NetworkBehaviour
    {
        public SteamLobby steamLobby;
        public static LobbyController instance;
        
        //UI elements
        public Text lobbyNameText;
        
        //Player Data
        public GameObject playerListViewContent;
        public GameObject playerListItemPrefab;
        public GameObject localPlayerObject;  
        
        //Other Data
        public ulong currentLobbyID;
        public bool playerItemCreated = false;
        private List<PlayerListItem> _playerListItems = new List<PlayerListItem>();
        public PlayerObjectController localPlayerController;
        
        //Manager
        private CustomNetworkManager _manager;

        private CustomNetworkManager Manager
        {
            get
            {
                if (_manager != null)
                {
                    return _manager;
                }

                return _manager = CustomNetworkManager.singleton as CustomNetworkManager;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                steamLobby = Manager.GetComponent<SteamLobby>();
            }
        }

        public void UpdateLobbyName()
        {
            currentLobbyID = steamLobby.currentLobbyID;
            lobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(currentLobbyID), "name");
        }

        public void UpdatePlayerList()
        {
            if (!playerItemCreated)
            {
                CreateHostPlayerItem(); //Host
            }
            
            if(_playerListItems.Count < Manager.GamePlayers.Count) { CreateClientPlayerItem();}
            if(_playerListItems.Count > Manager.GamePlayers.Count) { RemovePlayerItem();}
            if(_playerListItems.Count == Manager.GamePlayers.Count) { UpdatePlayerItem();}
        }

        public void FindLocalPlayer()
        {
            localPlayerObject = GameObject.Find("LocalGamePlayer");
            localPlayerController = localPlayerObject.GetComponent<PlayerObjectController>();
        }
        
        public void CreateHostPlayerItem()
        {
            foreach (PlayerObjectController player in Manager.GamePlayers)
            {
                GameObject newPlayerItem = Instantiate(playerListItemPrefab) as GameObject;
                PlayerListItem newPlayerItemScript = newPlayerItem.GetComponent<PlayerListItem>();

                newPlayerItemScript.playerName = player.playerName;
                newPlayerItemScript.connectionID = player.connectionID;
                newPlayerItemScript.playerSteamID = player.playerSteamID;
                newPlayerItemScript.SetPlayerValues();
                
                newPlayerItem.transform.SetParent(playerListViewContent.transform);
                newPlayerItem.transform.localScale = Vector3.one;

                _playerListItems.Add(newPlayerItemScript);
            }

            playerItemCreated = true;
        }
        
        
        
        public void CreateClientPlayerItem()
        {
            foreach (PlayerObjectController player in Manager.GamePlayers)
            {
                if (!_playerListItems.Any(b => b.connectionID == player.connectionID))
                {
                    GameObject newPlayerItem = Instantiate(playerListItemPrefab) as GameObject;
                    PlayerListItem newPlayerItemScript = newPlayerItem.GetComponent<PlayerListItem>();

                    newPlayerItemScript.playerName = player.playerName;
                    newPlayerItemScript.connectionID = player.connectionID;
                    newPlayerItemScript.playerSteamID = player.playerSteamID;
                    newPlayerItemScript.SetPlayerValues();
                
                    newPlayerItem.transform.SetParent(playerListViewContent.transform);
                    newPlayerItem.transform.localScale = Vector3.one;

                    _playerListItems.Add(newPlayerItemScript);
                }
            }
        }

        public void UpdatePlayerItem()
        {
            foreach (PlayerObjectController player in Manager.GamePlayers)
            {
                foreach (PlayerListItem playerListItemScript in _playerListItems)
                {
                    if (playerListItemScript.connectionID == player.connectionID)
                    {
                        playerListItemScript.playerName = player.playerName;
                        playerListItemScript.SetPlayerValues();
                    }
                }
            }
        }

        public void RemovePlayerItem()
        {
            List<PlayerListItem> playerListItemsToRemove = new List<PlayerListItem>();

            foreach (PlayerListItem playerListItem in _playerListItems)
            {
                if (!Manager.GamePlayers.Any(b => b.connectionID == playerListItem.connectionID))
                {
                    playerListItemsToRemove.Add(playerListItem);
                }
            }

            if (playerListItemsToRemove.Count > 0)
            {
                foreach (PlayerListItem playerlistItemToRemove in playerListItemsToRemove)
                {
                    GameObject objectToRemove = playerlistItemToRemove.gameObject;
                    _playerListItems.Remove(playerlistItemToRemove);
                    Destroy(objectToRemove);
                    objectToRemove = null;
                }
            }
        }
    }
}
