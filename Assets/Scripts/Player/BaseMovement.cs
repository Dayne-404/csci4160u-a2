using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;
    private InputManager _inputManager;
    private Rigidbody rb;

    [Header("Speed")]
    [SerializeField] private float runSpeed = 8.0f;
    [SerializeField] public float walkSpeed = 4.0f;
    [SerializeField] private float slideSpeed = 9f;
    [SerializeField] private float crouchSpeed = 2.0f;
    [SerializeField] private float wallRunSpeed = 6.0f;
    public float moveSpeed;
    private Vector3 moveDirection;

    [Header("Acceleration")]
    [SerializeField] private float acceleration = 4f;
    [SerializeField] private float slopeAcceleration = 2.5f;

    [Header("Forces")]
    [SerializeField] private float dragForce = 5.0f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float airMultiplier = 0.5f;
    

    [Header("Ground Check")]
    [SerializeField] private float playerHeight = 2.0f;
    public bool grounded;

    [Header("Slope Movement")]
    [SerializeField] private float maxSlopeAngle = 45.0f;
    public Vector3 startScale;
    private RaycastHit slopeHit;
    
    [Header("State Control")]
    public bool isSprinting = false;
    public bool isSliding = false;
    public bool isCrouching = false;
    public bool isWallRunning = false;

    

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        wallrunning,
        crouching,
        sliding,
        air
    }

    void Awake() {
        _inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Start() {
        startScale = transform.localScale;
    }

    void Update() {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, groundLayer);
        speedControl();
        StateHandler();
    }

    void FixedUpdate() {
        MovePlayer();
        CrouchManager();

        if (grounded && _inputManager.Jump) Jump();
    }

    private void MovePlayer() {
        if (!isSliding) {
            moveDirection = transform.forward * _inputManager.MoveDirection.y + transform.right * _inputManager.MoveDirection.x;

            if (OnSlope())
                rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 8f, ForceMode.Force);
            else if (grounded) {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            } else if (!grounded) {
                rb.AddForce(Vector3.down * Time.deltaTime * 20);
                rb.AddForce((moveDirection.normalized * 0.5f) * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            }
        }

        rb.useGravity = !OnSlope();
    }

    private void CrouchManager() {
        if (rb.velocity.magnitude <= walkSpeed) {
            if (_inputManager.Crouch && !isCrouching && grounded) {
                crouch();
            } else if ((!_inputManager.Crouch || _inputManager.Jump) && isCrouching) {
                exitCrouch();
            }
        }
    }

    private bool checkMovingForward() {
        Vector2 velocityRelativeToLook = FindVelRelativeToLook();

        if (velocityRelativeToLook.y > 0.2f) {
            return true;
        }

        return false;
    }

    private void Jump() {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void crouch() {
        isCrouching = true;
        moveSpeed = crouchSpeed;

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 0.5f, transform.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }

    private void exitCrouch() {
        if (!objAbove()) {
            isCrouching = false;
            _inputManager.Crouch = false;
            transform.localScale = startScale;
        } else {
            _inputManager.Crouch = true;
        }
    }

    private void speedControl() {
        if (grounded) {
            rb.drag = dragForce;
        } else {
            rb.drag = 0f;
        }


        if (OnSlope()) {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        } else {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed) {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction) {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    public bool objAbove() {
        return Physics.Raycast(transform.position, Vector3.up, playerHeight, ~playerLayer);
    }

    public bool OnSlope() {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f)) {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

            if (angle > maxSlopeAngle) {
                isSprinting = false;
            }

            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private void StateHandler() {
        if (isWallRunning) {
            state = MovementState.wallrunning;
            desiredMoveSpeed = wallRunSpeed;
        } 
        else if (isSliding) {
            state = MovementState.sliding;

            if (OnSlope() && rb.velocity.y < 0.1f)
                desiredMoveSpeed = slideSpeed;
            else desiredMoveSpeed = runSpeed;
        } 
        else if (isCrouching) {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }
        else if (grounded && moveDirection.magnitude > 0 && !_inputManager.Walk && checkMovingForward()) {
            state = MovementState.sprinting;
            desiredMoveSpeed = runSpeed;
        }
        else if (grounded) {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }
        else {
            state = MovementState.air;
        }

        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) >= 4f && moveSpeed != 0) {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        } else {
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private Vector2 FindVelRelativeToLook() {
        float lookAngle = transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitude = rb.velocity.magnitude;
        float yMag = magnitude * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitude * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag).normalized;
    }

    private IEnumerator SmoothlyLerpMoveSpeed() {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference) {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope()) {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * acceleration * slopeAcceleration * slopeAngleIncrease;
            } else
                time += Time.deltaTime * slopeAcceleration;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }
}