using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputManager im;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LevelManager lm;

    [Header("Position")]
    [SerializeField] private Vector3 checkPoint = Vector3.zero;
    [SerializeField] private float maxYDepth = -50f;

    void Start() {
        if(lm.startPosition != null)
            checkPoint = lm.startPosition.position;
    }

    void Update() {
        if (im.Reset || transform.position.y <= maxYDepth) {
            im.Reset = false;
            ReturnToCheckpoint();
        }
    }

    public void ResetCheckpoint(Transform newPosition) {
        checkPoint = newPosition.position;
    }

    public void ReturnToCheckpoint() {
        rb.velocity = new Vector3(0, 0, 0);
        transform.position = checkPoint;
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 8) {
            checkPoint = other.gameObject.transform.GetChild(0).position;
        } 
        
        if (other.gameObject.tag == "Play" && lm != null) {
            lm.currentState = LevelManager.LevelState.Started;
        } else if (other.gameObject.tag == "Exit" && lm != null) {
            lm.currentState = LevelManager.LevelState.Ended;
        }
    }
}
