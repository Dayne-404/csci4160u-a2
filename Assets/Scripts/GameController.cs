using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private InputManager inputManager;

    [Header("Keybinds")]
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;

    private bool paused = false;

    private void Start() {
        paused = false;
        Time.timeScale = 1f;

        if (pauseMenu !=null) pauseMenu.SetActive(false);
    }

    private void Update() {
        if(Input.GetKeyDown(pauseKey)) {
            if(!paused) PauseGame();
            else if(paused) UnPauseGame();
        }
    }

    public void GotoMenu() {
        SceneManager.LoadScene("Main_Menu");
    }

    public void GotoTutorial() {
        SceneManager.LoadScene("SampleScene");
    }

    public void PauseGame() {
        Debug.Log("Pause");
        if(pauseMenu != null) pauseMenu.SetActive(true);
        if(inputManager != null) inputManager.UnlockCursor();
        Time.timeScale = 0f;
        paused = true;
    }

    public void UnPauseGame() {
        Debug.Log("UnPause");
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (inputManager != null) inputManager.LockCursor();
        Time.timeScale = 1f;
        paused = false;
    }

    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }


}
