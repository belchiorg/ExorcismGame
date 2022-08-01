using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] float moveSpeed = 4f;

    void Update()
    {
        Vector2 keyboardInput = getKeyboardInput();

        Vector3 move = transform.right * keyboardInput.x + transform.forward * keyboardInput.y;

        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    private Vector2 getKeyboardInput()
    {
        float getKeyboardAxis(string axis)
        {
            return Input.GetAxisRaw(axis);
        }

        return new Vector2(getKeyboardAxis("Horizontal"), getKeyboardAxis("Vertical"));
    }
}
