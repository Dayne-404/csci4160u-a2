using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator anim;
    private BaseMovement baseMovement;
    private Rigidbody rb;

    public float maxSpeed = 8f;

    private void Awake() {
        baseMovement= GetComponent<BaseMovement>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        anim.SetFloat("VelocityMag", new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude);
        anim.SetBool("Grounded", baseMovement.grounded);
        
        if(baseMovement.isSliding) {
            anim.speed = 0;
        } else {
            anim.speed = 1;
        }
    }
}
