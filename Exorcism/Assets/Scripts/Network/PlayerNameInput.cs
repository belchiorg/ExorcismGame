using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Network
{
    public class PlayerNameInput : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private Button continueButton;

        private const string PlayerPrefSNameKey = "PlayerName";

        private void Start() => SetUpInputField();

        private void SetUpInputField()
        {
            if (!PlayerPrefs.HasKey(PlayerPrefSNameKey))
            {
                return;
            }

            string defaultName = PlayerPrefs.GetString(PlayerPrefSNameKey);

            nameInputField.text = defaultName;

            SetPlayerName(defaultName);
        }

        public void SetPlayerName(string defaultName)
        {
            continueButton.interactable = !string.IsNullOrEmpty(name);
        }

        public void SavePlayerName()
        {
            string playerName = nameInputField.text;

            PhotonNetwork.NickName = playerName;
            
            PlayerPrefs.SetString(PlayerPrefSNameKey, playerName);
        }
    }
}