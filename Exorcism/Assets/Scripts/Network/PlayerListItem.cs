using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;

namespace Game.Network
{
    public class PlayerListItem : MonoBehaviour
    {
        public string playerName;
        public int connectionID;
        public ulong playerSteamID;
        private bool _avatarReceived;

        public Text PlayerNameText;
        public RawImage PlayerIcon;
        
        protected Callback<AvatarImageLoaded_t> imageLoaded;

        private void Start()
        {
            imageLoaded = Callback<AvatarImageLoaded_t>.Create(OnImageLoaded);
        }

        private void OnImageLoaded(AvatarImageLoaded_t callback)
        {
            if (callback.m_steamID.m_SteamID == playerSteamID)
            {
                PlayerIcon.texture = GetSteamImageAsTexture(callback.m_iImage);
            }
            else
            {
                return;
            }
        }
        
        private Texture2D GetSteamImageAsTexture(int iImage)
        {
            Texture2D texture = null;

            bool isValid = SteamUtils.GetImageSize(iImage, out uint width, out uint height);
            if (isValid)
            {
                byte[] image = new byte[width * height * 4];

                isValid = SteamUtils.GetImageRGBA(iImage, image, (int)(width * height * 4));

                if (isValid)
                {
                    texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                    texture.LoadRawTextureData(image);
                    texture.Apply();
                }
            }
            _avatarReceived = true;
            return texture;
        }

        public void SetPlayerValues()
        {
            PlayerNameText.text = playerName;
            if(!_avatarReceived) { GetPlayerIcon();}
        }
        private void GetPlayerIcon()
        {
            int imageID = SteamFriends.GetLargeFriendAvatar((CSteamID)playerSteamID);
            if(imageID == -1) { return;}

            PlayerIcon.texture = GetSteamImageAsTexture(imageID);
        }
        
    }
}
