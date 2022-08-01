using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 250f;
    [SerializeField] Transform playerBody;
    float xRotation = 0f;

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
    }

    private Vector2 getMouseInput()
    {
        float getMouseAxis(string axis)
        {
            return Input.GetAxis(axis) * mouseSensitivity * Time.deltaTime;
        }

        return new Vector2(getMouseAxis("Mouse X"), getMouseAxis("Mouse Y"));
    }

    private void hideAndLockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
