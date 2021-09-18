using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance;
    public GameObject loadingScreen;
    public bool connected = false;
    public DateTime connectSendTime;
    public GameObject loading;
    public GameObject message;
    public TMPro.TextMeshProUGUI messageText;
    private bool pressKeyToExit = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public void LoadGame()
    {
        loadingScreen.SetActive(false);
    }

    public void GameInProgress()
    {
        messageText.SetText("Unable to join. Game in progress.");
        loading.SetActive(false);
        message.SetActive(true);
        pressKeyToExit = true;
        Client.instance.Disconnect();
    }

    public void GameFull()
    {
        Debug.Log("Server full");
        messageText.SetText("Unable to join. Game is full.");
        loading.SetActive(false);
        message.SetActive(true);
        pressKeyToExit = true;
        Client.instance.Disconnect();
    }

    // Update is called once per frame
    void Update()
    {
        if (!connected && (DateTime.Now - connectSendTime).Seconds > 10)
        {
            messageText.SetText("Unable to connect to the server.");
            loading.SetActive(false);
            message.SetActive(true);
            pressKeyToExit = true;
            Client.instance.Disconnect();
        }
        
        if (Input.anyKey && pressKeyToExit)
        {
            SceneManager.LoadScene((int)Scenes.UI);
        }
    }
}
