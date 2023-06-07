using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothVolume : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float endValue = 1f;
    [SerializeField] private float speedMultiplier = 1f;
    
    void Update()
    {
        audioSource.volume = Mathf.Lerp(audioSource.volume, endValue, Time.fixedDeltaTime * speedMultiplier);
    }
}
