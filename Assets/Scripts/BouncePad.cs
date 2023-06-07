using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [SerializeField] private float bounceForce;

    public void OnCollisionEnter(Collision collision) {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        
        if(rb != null) {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * bounceForce, ForceMode.Impulse); 
        }
    }
}
