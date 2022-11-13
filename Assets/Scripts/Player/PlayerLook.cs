using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Transform playerCamera;
    public Vector2 sensitivity;

    private Vector2 XYRotation;

    private void Start()
    {
        // We don't want a cursor running around the screen while playing
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // We get the mouse input from the player
        Vector2 mouseInput = new Vector2
        {
            x = Input.GetAxis("Mouse X"),
            y = Input.GetAxis("Mouse Y")
        };


        // Rotating the x axis invokes vertical rotation
        XYRotation.x -= mouseInput.y * sensitivity.y;
        // Rotating the y axis invokes horizontal rotation
        XYRotation.y += mouseInput.x * sensitivity.x;

        // We limit the vertical rotation
        XYRotation.x = Mathf.Clamp(XYRotation.x, -90f, 90f);

        // We rotate the player for horizontal rotation
        // and the camera for vertical rotation
        transform.eulerAngles = new Vector3(0f, XYRotation.y, 0f);
        playerCamera.localEulerAngles = new Vector3(XYRotation.x, 0f, 0f);
    }
}
