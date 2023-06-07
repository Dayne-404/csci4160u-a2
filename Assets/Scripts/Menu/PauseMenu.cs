using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private AudioManager audioManager;

    private bool paused = false;

    private void Start() {
        paused = false;
        Time.timeScale = 1f;

        if (pauseMenu != null) pauseMenu.SetActive(false);
    }

    private void Update() {
        if (inputManager.Pause) {
            if (!paused) PauseGame();
            else if (paused) UnPauseGame();
        }
    }

    public void GotoMenu() {
        if (inputManager != null) inputManager.UnlockCursor();
        SceneManager.LoadScene("Main_Menu");
    }

    public void PauseGame() {
        Debug.Log("Pause");
        if (pauseMenu != null) pauseMenu.SetActive(true);
        if (inputManager != null) inputManager.UnlockCursor();
        audioManager.pauseMusic();
        Time.timeScale = 0f;
        paused = true;
    }

    public void UnPauseGame() {
        Debug.Log("UnPause");
        if (pauseMenu != null) {
            Debug.Log("Yeah");
            pauseMenu.SetActive(false);
        }
        if (inputManager != null) inputManager.LockCursor();
        audioManager.resumeMusic();
        Time.timeScale = 1f;
        paused = false;
    }

    public void ExitGame() {
        Debug.Log("Quit");
        if (inputManager != null) inputManager.UnlockCursor();
        Application.Quit();
    }
}
