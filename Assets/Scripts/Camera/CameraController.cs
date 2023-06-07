using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _orientation;
    [SerializeField] private InputManager _inputManager;

    private Vector2 rotation;

    private void Start() {
        _orientation.rotation = transform.rotation;    
    }

    void LateUpdate()
    {
        rotation.x -= _inputManager.LookDirection.y;
        rotation.y += _inputManager.LookDirection.x;

        //print(rotationX);

        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);

        transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0);
        _orientation.localRotation = Quaternion.Euler(0, rotation.y, 0);
    }

    public void ClampCameraY(float camRot, float deg) {
        rotation.y = Mathf.Clamp(rotation.y, camRot - deg, camRot + deg);
    }

    public float getRotationY() => rotation.y;
}
