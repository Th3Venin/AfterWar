using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject mainUI;
    public GameObject loadingScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        mainUI.SetActive(false);
        loadingScreen.SetActive(true);
        SceneManager.LoadScene((int)Scenes.GAME);
    }

    public void Exit()
    {
        Application.Quit();
    }

}
