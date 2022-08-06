/*using Mirror;
using UnityEngine;

namespace Game.Core
{
    public class PlayerMovement : NetworkBehaviour
    {
        [SerializeField] CharacterController controller;
        [SerializeField] float moveSpeed = 4f;
        
        void Update()
        {
            HandlePlayerMovement();
        }

        private void HandlePlayerMovement()
        {
            if (isLocalPlayer)
            {
                Vector2 keyboardInput = GetKeyboardInput();
                Vector3 move = transform.right * keyboardInput.x + transform.forward * keyboardInput.y;
                controller.Move(move * moveSpeed * Time.deltaTime);
            }
        }
        
        private Vector2 GetKeyboardInput()
        {
            float GetKeyboardAxis(string axis)
            {
                return Input.GetAxisRaw(axis);
            }

            return new Vector2(GetKeyboardAxis("Horizontal"), GetKeyboardAxis("Vertical"));
        }
    }
}*/

