using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Video used: https://youtu.be/JivuXdrIHK0
 * eu eram la https://youtu.be/JivuXdrIHK0?t=593
 */
public class PauseMenu : MonoBehaviour
{
    // boolean variable that stores the state of the game (if paused or not)
    public static bool GameIsPaused = false;

    [SerializeField]
    private GameObject pauseMenuUI;
    [SerializeField]
    private GameObject playerHUD;
    [SerializeField]
    private GameObject player;
    [SerializeField] 
    private string mainMenuScript;

    void Update()
    {
        // every time the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {

            Debug.Log(GameIsPaused);
            if (GameIsPaused)
            {
                ResumeGame();
            } 
            else
            {
                PauseGame();
            }
        }
    }

    // method also called when "RESUME" button is clicked
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        playerHUD.SetActive(true);

        // reenable the player view when the game resumes
        player.GetComponent<PlayerLook>().enabled = true;

        // hide the cursor when the game resumes
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1.0f;
        GameIsPaused = false;
    }

    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        playerHUD.SetActive(false);

        // disable the player view (camera souldn't move when paused)
        player.GetComponent<PlayerLook>().enabled = false;

        // show the cursor when the game is paused
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    // method called when "SETTINGS" button is pressed
    public void OpenSettings()
    {

    }

    public void Exit()
    {
        SceneManager.LoadScene(mainMenuScript);
    }
}
