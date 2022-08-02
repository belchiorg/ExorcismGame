using System.Collections;
using System.Collections.Generic;
using Game.Network;
using UnityEngine;
using Mirror;
using Steamworks;

namespace Game
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
                if (_manager != null)
                {
                    return _manager;
                }

                return _manager = CustomNetworkManager.singleton as CustomNetworkManager;
            }
        }
        public void PlayerNameUpdate(string oldValue, string newValue)
        {
            
        }
    }
    
}
