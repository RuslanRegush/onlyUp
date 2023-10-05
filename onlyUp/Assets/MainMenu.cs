using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame()
    {
        SceneManager.LoadScene("level1");
    }
    public void PlayGame2()
    {
        SceneManager.LoadScene("level2");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("quit");  
    }
}
