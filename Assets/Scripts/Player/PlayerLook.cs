using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour, ISaveable
{
    public Transform playerCamera;
    public Vector2 sensitivity;

    private Vector2 XYRotation;
<<<<<<< Updated upstream
    Vector2 XYRotationUpdate;
    bool updated;
=======
    private Vector2 XYRotationUpdate;
    private bool updated;

>>>>>>> Stashed changes
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

        if (updated)
        {
            XYRotation = XYRotationUpdate;
            if (XYRotation.x > 270)
            {
                XYRotation.x -= 360;
            }
            updated = false;
        }

        // We rotate the player for horizontal rotation
        // and the camera for vertical rotation
        transform.eulerAngles = new Vector3(0f, XYRotation.y, 0f);
        playerCamera.localEulerAngles = new Vector3(XYRotation.x, 0f, 0f);
   
    }

    public object CaptureState()
    {
        return new SaveData()
        {
            xRot = playerCamera.localEulerAngles.x,
            yRot = transform.eulerAngles.y,
        };
    }

    public void RestoreState(object state)
    {
        Debug.Log("Loading state rotation");
        var saveData = (SaveData)state;

        updated = true;
        XYRotationUpdate = new Vector2(saveData.xRot, saveData.yRot);
        /*Debug.Log("Look vector:");
        Debug.Log(XYRotationUpdate);*/
    }

    [Serializable]
    private struct SaveData
    {
        public float xRot;
        public float yRot;
    }

    public object CaptureState()
    {
        return new SaveData()
        {
            xRot = playerCamera.localEulerAngles.x,
            yRot = transform.eulerAngles.y,
        };
    }

    public void RestoreState(object state)
    {
        Debug.Log("Loading state rotation");
        var saveData = (SaveData)state;

        updated = true;
        XYRotationUpdate = new Vector2(saveData.xRot, saveData.yRot);
        /*Debug.Log("Look vector:");
        Debug.Log(XYRotationUpdate);*/
    }

    [Serializable]
    private struct SaveData
    {
        public float xRot;
        public float yRot;
    }
}
