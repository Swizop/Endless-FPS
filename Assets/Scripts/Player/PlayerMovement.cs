using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSmoothTime;
    public float gravityStrength;
    public float jumpStrength;
    public float walkSpeed;
    public float runSpeed;

    private CharacterController controller;
    private Vector3 currentMoveVelocity;
    private Vector3 moveDampVelocity;

    private Vector3 currentForceVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        /* Movement Part */

        // Get the player's movement input on every frame
        Vector3 playerMovementInput = new Vector3
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = 0f,
            z = Input.GetAxisRaw("Vertical")
        };


        // Making sure that the player input doesn't overwrite the maximum speed
        if (playerMovementInput.magnitude > 1f)
        {
            playerMovementInput.Normalize();
        }

        // Rotating the movement vector so that forward movement is in front of the player
        Vector3 moveVector = transform.TransformDirection(playerMovementInput);
        // Checking if the sprint key is pressed
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // We move the player smoothly
        currentMoveVelocity = Vector3.SmoothDamp(currentMoveVelocity, moveVector * currentSpeed,
            ref moveDampVelocity, moveSmoothTime);
        controller.Move(currentMoveVelocity * Time.deltaTime);


        /* Jumping Part */

        // Using a ray under the player to check whether the player is grounded or not
        Ray groundCheckRay = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(groundCheckRay,1.5f))
        {
            // Keeping the player on the floor while climbing slopes and stairs
            currentForceVelocity.y = -2f;

            // Since the raycast hit, it means the player is on the ground and can jump
            if (Input.GetKey(KeyCode.Space))
            {
                currentForceVelocity.y = jumpStrength;
            }
        }
        // If the player is in the air, we apply gravity
        else
        {
            print("In the air");
            currentForceVelocity.y -= gravityStrength * Time.deltaTime;
        }

        controller.Move(currentForceVelocity * Time.deltaTime);
    }
}
