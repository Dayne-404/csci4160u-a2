using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioClip runningSound;

    [SerializeField] private AudioSource source;
    private BaseMovement bm;

    private void Awake() {
        bm = GetComponent<BaseMovement>();
    }

    private void Update() {
        AudioHandler();
    }

    private void AudioHandler() {
        if(bm.state == BaseMovement.MovementState.sprinting) {
            setSourceClip(runningSound);
            playAudio();
        } else {
            stopAudio();
        }
    }

    private void stopAudio() {
        source.volume = 0;
        source.enabled = false;
    }

    private void setSourceClip(AudioClip clip) {
        if(source.clip != clip) {
            stopAudio();
            source.clip = clip;
        }
    }

    private void playAudio() {
        source.volume = 0.4f;
        source.enabled = true;
    }
}
