using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip victoryMusic;
    private SmoothVolume smoothVolume;

    private void Start() {
        smoothVolume = GetComponent<SmoothVolume>();
        audioSource.Stop();
        smoothVolume.enabled = false;
    }

    public void playMusic() {
        audioSource.clip = backgroundMusic;
        audioSource.Play();
        smoothVolume.enabled = true;
    }

    public void stopMusic() {
        audioSource.Stop();
        smoothVolume.enabled = false;
    }

    public void pauseMusic() {
        audioSource.Pause();
    }

    public void resumeMusic() {
        audioSource.Play();
    }

    public void playVictoryMusic() {
        audioSource.clip = victoryMusic;
        audioSource.volume = 0.2f;
        audioSource.Play();
    }
}
