using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBackup : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float runSpeed = 8.0f;
    [SerializeField] public float walkSpeed = 4.0f;
    [SerializeField] private float crouchSpeed = 2.0f;

    [SerializeField] private float dragForce = 5.0f;
    [SerializeField] private float airMultiplier = 0.5f;
    
    private Vector3 moveDirection;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5.0f;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight = 2.0f;
    public bool grounded;

    [Header("Slope Movement")]
    [SerializeField] public float maxSlopeAngle = 30.0f;
    private RaycastHit slopeHit;

    [Header("References")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform orientation;
    private InputManager _inputManager;
    private Rigidbody rb;

    public Vector3 startScale;
    public bool isSprinting = false;
    public bool isSliding = false;
    public bool isCrouching = false;

    public float moveSpeed;
    public float maxSpeed;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    void Awake()
    {
        _inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Start() {
        startScale = transform.localScale;
        maxSpeed = walkSpeed;
        moveSpeed = maxSpeed;
    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, groundLayer);
        speedControl();
    }

    void FixedUpdate() {
        MovePlayer();
        CrouchManager();

        if(grounded && _inputManager.Jump) Jump();
    }

    private void MovePlayer() {
        if (!isSliding) {
            moveDirection = orientation.forward * _inputManager.MoveDirection.y + orientation.right * _inputManager.MoveDirection.x;
            
            if(!isCrouching)
                DynamicMovement();

            if (OnSlope())
                rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 8f, ForceMode.Force);
            else if (grounded) {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            }
                
            else if (!grounded) {
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

    private void DynamicMovement() {
        if(!_inputManager.Walk && moveDirection.magnitude > 0 && checkMovingForward()) {
            maxSpeed = runSpeed;
            if (moveSpeed < maxSpeed * 0.9f)
                moveSpeed = Mathf.Lerp(moveSpeed, maxSpeed, Time.smoothDeltaTime * 0.8f);
            else if (moveSpeed > maxSpeed * 0.9f)
                moveSpeed = maxSpeed;
        } else {
            maxSpeed = walkSpeed;
            moveSpeed = walkSpeed;
        }
    }

    private bool checkMovingForward() {
        Vector2 velocityRelativeToLook = FindVelRelativeToLook();

        if(velocityRelativeToLook.y > 0.2f) {
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
        maxSpeed = crouchSpeed;
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
        if(grounded) {
            rb.drag = dragForce;
        } else {
            rb.drag = 0f;
        }


        if (OnSlope()) {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        } else {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            if (flatVel.magnitude > maxSpeed) {
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
            
            if(angle > maxSlopeAngle) {
                isSprinting = false;
            }

            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector2 FindVelRelativeToLook() {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitude = rb.velocity.magnitude;
        float yMag = magnitude * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitude * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag).normalized;
    }

    private IEnumerator SmoothlyLerpMoveSpeed() {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference) {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope()) {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            } else
                time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }
}
