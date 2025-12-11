using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/* Johnathan Spangler
 * 12/09/25
 * Controls the player's Vehicle
 */

public enum MovementType
{
    Rotate, Free
}

public class RacerMovement : MonoBehaviour
{
    public float maxSpeed, acceleration, brakeSmoothTime, rotateInputTurnRate;

    public bool moveAlongLocalForward = true, preserveVerticalVelocity = true, braking, stopped;

    Rigidbody rb;

    public float currentSpeed = 0f, speedVelocityRef = 0f, currentYaw;

    public ControllerImplementation inputActions;

    public bool brakeLocked = true;

    void Start()
    {
        brakeLocked = true;
        braking = true;
        stopped = true;
        currentSpeed = 0f;
    }

    void Awake()
    {
        //Set rigidbody and input
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        if (!TryGetComponent<BoxCollider>(out _)) gameObject.AddComponent<BoxCollider>();

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        currentYaw = transform.eulerAngles.y;

        inputActions = new ControllerImplementation();

        if (inputActions == null) inputActions = new ControllerImplementation();
    }

    //Redundancy to ensure inputs are recieved
    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        //Handle braking logic
        bool brakePressed = false;

        if (inputActions != null)
        { 
            brakePressed = inputActions.Controller.Brake.IsPressed();
        }

        if (brakeLocked)
        {
            braking = true;
        }
        else
        {
            braking = brakePressed;
        }

        if (brakeLocked && brakePressed)
        {
            SetBrakeLock(false);
        }

        float targetSpeed = braking ? 0f : maxSpeed;

        if (!braking)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
            stopped = false;
        }
        else
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, 0f, ref speedVelocityRef, Mathf.Max(0.01f, brakeSmoothTime));
            if (Mathf.Abs(currentSpeed) < 0.01f) currentSpeed = 0f;
            stopped = Mathf.Abs(currentSpeed) < 0.05f;
        }

        if (!stopped)
        {
            HandleRotateMode();
        }

        transform.rotation = Quaternion.Euler(0f, currentYaw, 0f);
    }

    /// <summary>
    /// Enable braking when a level starts (Called in UI Manager)
    /// </summary>
    /// <param name="locked"></param>
    public void SetBrakeLock(bool locked)
    {
        brakeLocked = locked;
        if (locked)
        {
            braking = true;
            currentSpeed = 0f;
            stopped = true;
            if (rb != null && preserveVerticalVelocity)
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            else if (rb != null)
                rb.velocity = Vector3.zero;
        }
    }

    /// <summary>
    /// Constantly move forward
    /// </summary>
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

    /// <summary>
    /// Handles the player turning controls
    /// </summary>
    void HandleRotateMode()
    {
        float h = 0f;
        if (inputActions.Controller.Left.IsPressed()) h -= 1f;
        if (inputActions.Controller.Right.IsPressed()) h += 1f;

        float turnAmount = h * rotateInputTurnRate * Time.deltaTime;
        currentYaw += turnAmount;
    }

    /// <summary>
    /// Reset player to default settings
    /// </summary>
    public void ResetRotationImmediate()
    {
        currentYaw = 0f;
        transform.rotation = Quaternion.Euler(0f, currentYaw, 0f);
        if (rb != null)
        {
            rb.angularVelocity = Vector3.zero;
            rb.MoveRotation(Quaternion.Euler(0f, currentYaw, 0f));
        }
        currentSpeed = 0f;
        stopped = true;
        brakeLocked = true;
    }
}
