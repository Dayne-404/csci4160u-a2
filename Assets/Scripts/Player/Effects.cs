using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Effects : MonoBehaviour
{
    [SerializeField] private ParticleSystem speedLines;
    private ParticleSystem.MainModule main;
    private Rigidbody rb;
    [SerializeField] float effectStartSpeed = 6f;
    private float alpha = 0f;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        main = speedLines.main;
    }

    void Start()
    {
        if(speedLines != null)
            speedLines.Stop();
    }

    void Update()
    {
        if(speedLines != null)
            SpeedLines();
    }

    private void SpeedLines() {
        bool atMaxSpeed = (rb.velocity.magnitude >= effectStartSpeed);

        if(speedLines.isPlaying && atMaxSpeed) {
            main.startColor = new Color(1, 1, 1, alpha);
            if (alpha < 1) alpha += Time.deltaTime * 0.2f;
        } else if (speedLines.isStopped && atMaxSpeed) {
            alpha = 0f;
            speedLines.Play();
        } else if (speedLines.isPlaying) {
            speedLines.Stop();
        }
    }
}
