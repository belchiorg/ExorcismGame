using UnityEngine;
using Mirror;
using Steamworks;

namespace Game.Steamworks
{

    public class SteamLobby : MonoBehaviour
    {

        public static SteamLobby instance;
        
        protected Callback<LobbyCreated_t> lobbyCreated;
        protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
        protected Callback<LobbyEnter_t> lobbyEntered;

        private const string HostAddressKey = "HostAddress";

        [SerializeField] private GameObject buttons;

        private NetworkManager _networkManager;

        private void Start()
        {
            _networkManager = GetComponent<NetworkManager>();

            if (!SteamManager.Initialized)
            {
                return;
            }

            if (instance == null)
            {
                instance = this; 
            }

            lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
            lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        }

        public void HostLobby(){
                buttons.SetActive(false);

                SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, _networkManager.maxConnections);
        }

        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            if (callback.m_eResult != EResult.k_EResultOK)
            {
                buttons.SetActive(true);
                return;
            }
            
            _networkManager.StartHost();

            SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey,
                SteamUser.GetSteamID().ToString());
        }

        private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
        {
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }

        private void OnLobbyEntered(LobbyEnter_t callback)
        {
            if (NetworkServer.active)
            {
                return;
            }

            string hostAddress = 
                SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);

            _networkManager.networkAddress = hostAddress;
            _networkManager.StartClient();
            
            buttons.SetActive(false);
        }
    }
}
