using System.Collections;
using System.Collections.Generic;
using Game.Network;
using UnityEngine;
using Mirror;
using Steamworks;
using Game.Network.Steamworks;

namespace Game.Network
{
    public class PlayerObjectController : NetworkBehaviour
    {
        //Player Data
        [SyncVar] public int connectionID;
        [SyncVar] public int playerIdNumber;
        [SyncVar] public ulong playerSteamID;

        [SyncVar(hook = nameof(PlayerNameUpdate))]
        public string playerName;

        private CustomNetworkManager _manager;

        private CustomNetworkManager Manager 
        {
            get
            {
                if (_manager)
                {
                    return _manager;
                }

                return _manager = CustomNetworkManager.singleton as CustomNetworkManager;
            }
        }

        public override void OnStartAuthority()
        {
            CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
            gameObject.name = "LocalGamePlayer"; //verificar se é o mm do outro script
            LobbyController.instance.FindLocalPlayer();
            LobbyController.instance.UpdateLobbyName();
        }

        public override void OnStartClient()
        {
            Manager.GamePlayers.Add(this);
            LobbyController.instance.UpdateLobbyName();
            LobbyController.instance.UpdatePlayerList();
        }

        public override void OnStopClient()
        {
            Manager.GamePlayers.Remove(this);
            LobbyController.instance.UpdatePlayerList();
        }

        [Command]
        private void CmdSetPlayerName(string playeName)
        {
            this.PlayerNameUpdate(this.playerName, playeName);
        }
        
        public void PlayerNameUpdate(string oldValue, string newValue)
        {
            if (isServer)
            {
                this.playerName = newValue;
            }

            if (isClient)
            {
                LobbyController.instance.UpdatePlayerList();
            }
        }
    }
    
}
