using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WallRun : MonoBehaviour
{
    [Header("Wallrunning")]
    [SerializeField] public LayerMask whatIsWall;
    [SerializeField] public LayerMask whatIsGround;
    public float wallRunForce;
    
    public float maxWallRunTime;
    public float wallRunTimer;

    public bool canWallRun = true;

    [Header("Detaction")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit rightWallhit;
    private RaycastHit leftWallhit;
    private bool wallLeft;
    private bool wallRight;

    [Header("References")]
    [SerializeField] private InputManager im;
    [SerializeField] private BaseMovement pc;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform orientation;

    public float wallJumpSideForce;
    public float wallJumpUpForce;

    private void Start() {
        wallRunTimer = maxWallRunTime;
    }

    private void Update() {
        CheckForWall();

        if (!canWallRun && pc.grounded) canWallRun = true;

        if(canWallRun) WallRunHandler();
    }

    private void FixedUpdate() {        
        if(pc.isWallRunning) {
            WallRunMovement();
        }
    }

    private void CheckForWall() {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, whatIsWall);
        
    }

    private bool checkAboveGround() {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void WallRunHandler() {
        if((wallRight || wallLeft)  && checkAboveGround()) {
            if (!pc.isWallRunning && im.MoveDirection.y > 0) StartWallRun();
            else if (wallRunTimer <= 0) EndWallRun();
            else if (im.Jump && wallRunTimer < maxWallRunTime * 0.7f) WallJump();
        } else {
            if (pc.isWallRunning) EndWallRun();
        }
    }

    private void StartWallRun() {
        pc.isWallRunning= true;
        wallRunTimer = maxWallRunTime;
    }

    private void WallRunMovement() {
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward + wallForward).magnitude)
            wallForward = -wallForward;

        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        if(wallRunTimer > 0) wallRunTimer -= Time.deltaTime;
    }

    private void EndWallRun() {
        pc.isWallRunning = false;
        canWallRun = false;
    }

    private void WallJump() {
        // enter exiting wall state
        Debug.Log("WALLJUMP");
        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        // reset y velocity and add force
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }
}
