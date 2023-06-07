using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [Header("Keyboard")]
    [SerializeField] private string KeyboardHorizontal = "Horizontal";
    [SerializeField] private string KeyboardVertical = "Vertical";
    [SerializeField] private KeyCode JumpKey = KeyCode.Space;
    [SerializeField] private KeyCode CrouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode walkKey = KeyCode.LeftAlt;
    [SerializeField] private KeyCode ResetKey = KeyCode.R;
    [SerializeField] private KeyCode PauseKey = KeyCode.Q;

    [Header("Mouse")]
    [SerializeField] private string MouseHorizontal = "Mouse X";
    [SerializeField] private string MouseVertical = "Mouse Y";
    [SerializeField] private float Sensitivity = 200f;

    [Header("Testing")]
    public Vector2 MoveDirection;
    public Vector2 LookDirection;

    public bool Jump;
    public bool Crouch;
    public bool Slide;
    public bool Walk;
    public bool Reset;
    public bool Pause;

    private void Start() {
        LockCursor();
    }

    private void Update()
    {
        MoveDirection = new Vector2(
            Input.GetAxisRaw(KeyboardHorizontal),
            Input.GetAxisRaw(KeyboardVertical)).normalized;
        
        LookDirection = new Vector2(
            Input.GetAxisRaw(MouseHorizontal),
            Input.GetAxisRaw(MouseVertical)) * Sensitivity * Time.smoothDeltaTime;

        Jump = getKeyState(JumpKey, Jump);
        Crouch = getKeyStateToggle(CrouchKey, Crouch);
        Slide = getKeyState(CrouchKey, Slide);
        Walk = getKeyStateToggle(walkKey, Walk);
        Reset = Input.GetKeyDown(ResetKey);
        Pause = Input.GetKeyDown(PauseKey);
    }

    public void resetInputs() {
        UnlockCursor();
        MoveDirection = Vector2.zero;
        LookDirection = Vector2.zero;
        Jump = false;
        Crouch = false;
        Slide = false;
        Walk = false;
        Reset = false;
    }

    public void LockCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private bool getKeyState(KeyCode key, bool state) {
        if (Input.GetKeyDown(key)) return true;
        if (Input.GetKeyUp(key)) return false;
   
        return state;
    }

    private bool getKeyStateToggle(KeyCode key, bool state) {
        if (Input.GetKeyDown(key)) return !state;

        return state;
    }

    private bool GetKeyPressed(KeyCode code) => Input.GetKeyDown(code); 
}
