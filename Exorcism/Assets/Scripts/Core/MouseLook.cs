using Unity.Netcode;
using UnityEngine;

namespace Game.Core
{
    public class MouseLook : NetworkBehaviour
    {
        [SerializeField] float mouseSensitivity = 250f;
        [SerializeField] Transform playerBody;
        float xRotation = 0f;

        private enum MouseState
        {
            NORMAL,
            LOCKED
        }

        MouseState currentState = MouseState.LOCKED;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                Object.Destroy(this.gameObject); // remove the camera if its not owner
            };
        }

        private void Start()
        {
            hideAndLockCursor();
        }

        void Update()
        {
            Vector2 mouseInput = getMouseInput();

            xRotation -= mouseInput.y;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseInput.x);

            if (Input.GetKeyDown(KeyCode.J)) toggleMouseState();
        }

        private Vector2 getMouseInput()
        {
            float getMouseAxis(string axis)
            {
                return Input.GetAxis(axis) * mouseSensitivity * Time.deltaTime;
            }

            return new Vector2(getMouseAxis("Mouse X"), getMouseAxis("Mouse Y"));
        }

        private void toggleMouseState()
        {
            if (currentState == MouseState.NORMAL)
            {
                hideAndLockCursor();
                currentState = MouseState.LOCKED;
            }
            else
            {
                unlockCursor();
                currentState = MouseState.NORMAL;
            }
        }

        private void hideAndLockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void unlockCursor()
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

}
