using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Johnathan Spangler
 * 11/18/25
 * Controls the player's Vehicle
 */

public enum MovementType
{
    Rotate, Free
}

public class RacerMovement : MonoBehaviour
{
    public MovementType movementType = MovementType.Rotate;

    public float maxSpeed, acceleration, brakeSmoothTime, turnSpeedDegreesPerSecond, rotateInputTurnRate;

    public bool moveAlongLocalForward = true, preserveVerticalVelocity = true, braking, stopped;

    Rigidbody rb;

    float currentSpeed = 0f, speedVelocityRef = 0f, currentYaw;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        if (!TryGetComponent<BoxCollider>(out _)) gameObject.AddComponent<BoxCollider>();

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        currentYaw = transform.eulerAngles.y;
    }

    void Update()
    {
        braking = Input.GetKey(KeyCode.Space);

        float targetSpeed = braking ? 0f : maxSpeed;

        if (!braking)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
            stopped = false;
        }
        else
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, 0f, ref speedVelocityRef, brakeSmoothTime);
            if (Mathf.Abs(currentSpeed) < 0.01f)
            {
                currentSpeed = 0f;
            }
            if (Mathf.Abs(currentSpeed) < 1f)
            {
                stopped = true;
            }
        }

        if (!stopped)
        {
            if (movementType == MovementType.Rotate)
                HandleRotateMode();
            else
                HandleFreeMode();
        }
        transform.rotation = Quaternion.Euler(0f, currentYaw, 0f);
    }

    void FixedUpdate()
    {
        Vector3 forward = moveAlongLocalForward ? transform.forward : Vector3.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 horizontalVelocity = forward * currentSpeed;

        if (preserveVerticalVelocity)
            rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);
        else
            rb.velocity = horizontalVelocity;
    }

    void HandleRotateMode()
    {
        float h = 0f;
        if (Input.GetKey(KeyCode.A)) h -= 1f;
        if (Input.GetKey(KeyCode.D)) h += 1f;

        float turnAmount = h * rotateInputTurnRate * Time.deltaTime;
        currentYaw += turnAmount;
    }

    void HandleFreeMode()
    {
        Vector2 input = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) input.y += 1f;
        if (Input.GetKey(KeyCode.S)) input.y -= 1f;
        if (Input.GetKey(KeyCode.D)) input.x += 1f;
        if (Input.GetKey(KeyCode.A)) input.x -= 1f;

        if (input.sqrMagnitude > 0.0001f)
        {
            Vector3 desired = new Vector3(input.x, 0f, input.y).normalized;
            float desiredYaw = Mathf.Atan2(desired.x, desired.z) * Mathf.Rad2Deg;
            currentYaw = Mathf.MoveTowardsAngle(currentYaw, desiredYaw, turnSpeedDegreesPerSecond * Time.deltaTime);
        }
    }

    /*
     * Use either of these later if desired to control via code
    public void ToggleMovementMode()
    {
        movementType = movementType == MovementType.Rotate ? MovementType.Free : MovementType.Rotate;
    }

    public void SetMovementMode(MovementType mode)
    {
        movementType = mode;
    }*/
}
