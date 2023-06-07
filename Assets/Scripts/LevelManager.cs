using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private TMP_Text finalTimeText;
    [SerializeField] private AudioManager audioManager;
    
    private Timer timer;
    private PauseMenu pauseMenu;
    private Transition transition;
    private Checkpoint playerCheckpoint;
    private InputManager inputManager;

    [Header("Extra")]
    public Transform startPosition;
    public LevelState currentState = LevelState.None;
    public enum LevelState {
        None,
        Started,
        Ongoing,
        Ended
    }

    private void Awake() {
        playerCheckpoint = player.GetComponent<Checkpoint>();
        inputManager = player.GetComponent<InputManager>();
        timer = GetComponent<Timer>();
        transition = GetComponent<Transition>();
        pauseMenu = GetComponent<PauseMenu>();
    }

    void Update()
    {
        switch (currentState) {
            case LevelState.Started:
                StartGame();
                break;
            case LevelState.Ended:
                EndGame();
                break;
        }
    }

    private void StartGame() {
        currentState = LevelState.Ongoing;
        audioManager.playMusic();
        timer.enabled = true;
    }

    public void RestartGame() {
        currentState = LevelState.None;
        audioManager.stopMusic();
        StopAllCoroutines();
        StartCoroutine(transition.FadeOut());

        timer.ResetTimer();
        inputManager.LockCursor();
        inputManager.enabled = true;
        
        playerCheckpoint.ResetCheckpoint(startPosition);
        playerCheckpoint.ReturnToCheckpoint();
        pauseMenu.enabled = true;
    }

    public void LoadNewLevel() {
        audioManager.stopMusic();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void EndGame() {
        currentState = LevelState.None;
        audioManager.stopMusic();
        audioManager.playVictoryMusic();
        StopAllCoroutines();
        StartCoroutine(transition.FadeIn());

        inputManager.resetInputs();
        inputManager.enabled = false;

        pauseMenu.enabled = false;
        timer.enabled = false;
        finalTimeText.text = timer.GetString();
    }
}
