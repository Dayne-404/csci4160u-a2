using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public CameraController cc;
    private Rigidbody rb;
    private BaseMovement pm;
    private InputManager im;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    public float slideCoolDown;
    private float slideCoolCounter = 0f;
    private float slideTimer;

    [Header("FOV")]
    [SerializeField] private float FOVlimit = 60f;

    private Vector3 inputDirection;
    public float slideScale = 0.5f;
    private Vector3 startScale;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<BaseMovement>();
        im = GetComponent<InputManager>();
    }

    private float cameraRotation;

    void Update() {
        if (im.Slide && pm.moveSpeed > pm.walkSpeed && !pm.isSliding && slideCoolCounter <= 0) {
            StartSlide();
        }

        if (pm.isSliding) {
            cc.ClampCameraY(cameraRotation, FOVlimit);
        }

        if (!im.Slide && pm.isSliding) {
            StopSlide();
        }
    }

    private void FixedUpdate() {
        if (slideCoolCounter > 0)
            slideCoolCounter -= Time.deltaTime;

        if (pm.isSliding) {
            SlidingMovement();
        }
    }

    private void SlidingMovement() {
        if (!pm.OnSlope() || rb.velocity.y > 0) {
            rb.AddForce(inputDirection.normalized * slideForce * slideTimer, ForceMode.Force);

            slideTimer -= Time.deltaTime;

            if (slideTimer < 0) {
                StopSlide();
            }
        } else {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection.normalized) * slideForce, ForceMode.Force);
        }

    }

    private void StartSlide() {
        cameraRotation = cc.getRotationY();
        inputDirection = orientation.forward * im.MoveDirection.y + orientation.right * im.MoveDirection.x;
        pm.isSliding = true;

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 0.5f, transform.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    //Im dumb and for some reason could not figure out a better way to write this lmao
    private void StopSlide() {
        if (!pm.objAbove()) {
            pm.isSliding = false;
            im.Slide = false;
            im.Crouch = false;
            transform.localScale = pm.startScale;
            slideCoolCounter = slideCoolDown;
        } else if (slideTimer < 0) {
            pm.isSliding = false;
            im.Slide = false;
            pm.isCrouching = true;
        }
    }
}