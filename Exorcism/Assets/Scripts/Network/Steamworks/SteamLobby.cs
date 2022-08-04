using Game.Network;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.UI;

namespace Game.Network.Steamworks
{

    public class SteamLobby : MonoBehaviour
    {

        public static SteamLobby instance;
        
        //Callbacks
        protected Callback<LobbyCreated_t> lobbyCreated;
        protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
        protected Callback<LobbyEnter_t> lobbyEntered;

        //Vars
        public ulong currentLobbyID;
        private const string HostAddressKey = "HostAddress";
        private CustomNetworkManager _manager;

        //GameObject
        public GameObject hostButton;
        public Text lobbyNameText;

        private void Start()
        {
            if (!SteamManager.Initialized)
            {
                return;
            }

            _manager = GetComponent<CustomNetworkManager>();

            lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
            lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

            if (instance == null)
            {
                instance = this; 
            }

            lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
            lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        }

        public void HostLobby(){

            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, _manager.maxConnections);
        }

        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            if (callback.m_eResult != EResult.k_EResultOK)
            {
                //buttons.SetActive(true);
                return;
            }
            
            _manager.StartHost();

            SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey,
                SteamUser.GetSteamID().ToString());
            SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name",
                SteamFriends.GetPersonaName() + "'s Lobby");
        }

        private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
        {
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }

        private void OnLobbyEntered(LobbyEnter_t callback)
        {
            //Everyone
            hostButton.SetActive(false);
            currentLobbyID = callback.m_ulSteamIDLobby;
            lobbyNameText.gameObject.SetActive(true);
            lobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name");
        
            //Clients
            if (NetworkServer.active)
            {
                return;
            }

            _manager.networkAddress =
                SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);
            
            _manager.StartClient();
        }
        
    }
}
