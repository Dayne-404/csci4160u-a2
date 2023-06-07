using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _orientation;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4500f;
    [SerializeField] private float maxSpeed = 20f;
    
    [Header("Slopes")]
    [SerializeField] private float maxSlopeAngle = 30f;

    [Header("GroundCheck")]
    [SerializeField] private float _height;
    [SerializeField] private bool _grounded;
    [SerializeField] private LayerMask _groundLayer;


    private bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, _height * 0.5f + 0.1f, _groundLayer);
        Debug.DrawRay(transform.position, Vector3.down * (_height * 0.5f + 0.1f), Color.red);
    }

    private void FixedUpdate() {
        Movement();
    }

    private void Movement() {
        _rigidbody.AddForce(Vector3.down * Time.deltaTime * 10);

        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        float maxSpeed = this.maxSpeed;

        if (_inputManager.MoveDirection.x > 0 && xMag > maxSpeed) _inputManager.MoveDirection.x = 0;
        if (_inputManager.MoveDirection.x < 0 && xMag < -maxSpeed) _inputManager.MoveDirection.x = 0;
        if (_inputManager.MoveDirection.y > 0 && yMag > maxSpeed) _inputManager.MoveDirection.y = 0;
        if (_inputManager.MoveDirection.y < 0 && yMag < -maxSpeed) _inputManager.MoveDirection.y = 0;

        Vector3 moveDirection = _orientation.forward * _inputManager.MoveDirection.y + _orientation.right * _inputManager.MoveDirection.x;
        
        _rigidbody.AddForce(moveDirection * moveSpeed * Time.deltaTime);
    }

    public Vector2 FindVelRelativeToLook() {
        float lookAngle = _orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(_rigidbody.velocity.x, _rigidbody.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = _rigidbody.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private bool IsFloor(Vector3 v) {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }
}
