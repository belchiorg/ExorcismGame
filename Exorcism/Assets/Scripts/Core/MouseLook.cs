using Unity.Netcode;
using UnityEngine;

namespace Game.Core
{
    public class MouseLook : NetworkBehaviour
    {
        [SerializeField] private float mouseSensitivity = 250f;
        [SerializeField] private Transform playerBody;
        private float _xRotation = 0f;

        private enum MouseState
        {
            Normal,
            Locked
        }

        MouseState _currentState = MouseState.Locked;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                Object.Destroy(this.gameObject); // remove the camera if its not owner
            };
        }

        private void Start()
        {
            HideAndLockCursor();
        }

        void Update()
        {
            Vector2 mouseInput = GetMouseInput();

            _xRotation -= mouseInput.y;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseInput.x);

            if (Input.GetKeyDown(KeyCode.J)) ToggleMouseState();
        }

        private Vector2 GetMouseInput()
        {
            float getMouseAxis(string axis)
            {
                return Input.GetAxis(axis) * mouseSensitivity * Time.deltaTime;
            }

            return new Vector2(getMouseAxis("Mouse X"), getMouseAxis("Mouse Y"));
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
