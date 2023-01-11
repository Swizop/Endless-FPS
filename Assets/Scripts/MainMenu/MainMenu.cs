using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public string firstLevel;
    public string secondLevel;
    private AudioSource gameStartedAudio;

    void Start()
    {

    // this part plays the start up sound (the BOOM)

        gameStartedAudio= GetComponent<AudioSource>();
        gameStartedAudio.volume = 0.1f;
        gameStartedAudio.Play();
    }

    void Update()
    {
        
    }

    // Bound to the NewGame button in the MainMenu
    public void ForestLevel()
    {
        SceneManager.LoadScene(firstLevel);
    }

    public void DungeonLevel()
    {
        SceneManager.LoadScene(secondLevel);
    }

    public void Settings()
    {

    }

    // Bound to the Exit button in the MainMenu
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
    }

    public void playButtonSound(AudioSource btnSound)
    {
        btnSound.volume = 0.5f;
        btnSound.Play();
    }
}
