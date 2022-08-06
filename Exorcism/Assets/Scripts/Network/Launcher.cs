using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Game.Network
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields
        
        // The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField] private byte maxPlayersPerRoom = 4;
        
        #endregion
        
        
        #region Private Fields

        // This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        private string gameVersion = "1";
        
        #endregion

        
        #region MonoBehaviour Callbacks

        // MonoBehaviour method called on GameObject by Unity during early initialization phase.
        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        
        void Start()
        {
            Connect();
        }
        
        #endregion
        
        
        #region Public Methods
        
        
        // Tries to connect. 
        // - If already connected, tries to join a random room
        // - If not, tries to connect to the Photon Cloud
        public void Connect()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings(); //Connects to photon cloud "deus"
                PhotonNetwork.GameVersion = gameVersion;
            }
        }
        
        #endregion
        
        
        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            
            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            PhotonNetwork.CreateRoom(null, new RoomOptions  { MaxPlayers = maxPlayersPerRoom } );
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");

        }
        
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }
        
        #endregion
    }
}
