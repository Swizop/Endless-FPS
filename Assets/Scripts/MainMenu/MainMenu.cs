using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string firstLevel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Bound to the NewGame button in the MainMenu
    public void NewGame()
    {
        SceneManager.LoadScene(firstLevel);
    }

    // Bound to the Exit button in the MainMenu
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
    }
}
