using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Game.Network
{
    public class MainMenu : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject findOpponentPanel;
        [SerializeField] private GameObject waitingStatusPanel;
        [SerializeField] private TextMeshProUGUI waitingStatusText;

        private bool _isConnecting;

        private const string GameVersion = "0.1";
        private const int MaxPlayersPerRoom = 4;

        private void Awake() => PhotonNetwork.AutomaticallySyncScene = true;

        public void FindOpponent()
        {
            _isConnecting = true;
            
            findOpponentPanel.SetActive(false);
            waitingStatusPanel.SetActive(true);

            waitingStatusText.text = "Searching...";

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = GameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Master");

            if (_isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            waitingStatusPanel.SetActive(false);
            findOpponentPanel.SetActive(true);
            
            Debug.Log($"Disconnected due to: {cause}");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("No Clients are waiting for an opponent, creating a new room");

            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Client successfully joined the room");

            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

            if (playerCount != MaxPlayersPerRoom) //Game can only start if lobby is full
            {
                waitingStatusText.text = "Waiting For Opponent";
                Debug.Log("Client is waiting for an opponent");
            }
            else
            {
                waitingStatusText.text = "Opponent Found";
                Debug.Log("Match is ready to begin");
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                
                waitingStatusText.text = "Opponent Found";
                Debug.Log("Match is ready to begin");
                
                PhotonNetwork.LoadLevel("Scene_Main");
            }
        }
    }
}
