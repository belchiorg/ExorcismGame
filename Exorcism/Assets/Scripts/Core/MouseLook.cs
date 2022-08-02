using System;
using Mirror;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Core
{
    public class MouseLook : NetworkBehaviour
    {
        [SerializeField] private float mouseSensitivity = 250f;
        [SerializeField] private Transform playerBody;
        [SerializeField] private Camera playerCam;
        [SerializeField] private AudioListener audioListener;
        private float _xRotation = 0f;

        private enum MouseState
        {
            Normal,
            Locked
        }

        MouseState _currentState = MouseState.Locked;

        private void Start()
        {
            if (isLocalPlayer)
            {
                HideAndLockCursor();
            }
            else
            {
                playerCam.enabled = false;
                audioListener.enabled = false;
            }
        }

        void Update()
        {
            if (isLocalPlayer)
            {
                Vector2 mouseInput = GetMouseInput();

                _xRotation -= mouseInput.y;
                _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

                playerCam.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
                playerBody.Rotate(Vector3.up * mouseInput.x);

                if (Input.GetKeyDown(KeyCode.J)) ToggleMouseState();
            }
        }

        private Vector2 GetMouseInput()
        {
            float GetMouseAxis(string axis)
            {
                return Input.GetAxis(axis) * mouseSensitivity * Time.deltaTime;
            }

            return new Vector2(GetMouseAxis("Mouse X"), GetMouseAxis("Mouse Y"));
        }

        private void ToggleMouseState()
        {
            if (_currentState == MouseState.Normal)
            {
                HideAndLockCursor();
                _currentState = MouseState.Locked;
            }
            else
            {
                UnlockCursor();
                _currentState = MouseState.Normal;
            }
        }

        private void HideAndLockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

}
