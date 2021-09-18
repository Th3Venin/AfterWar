using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;
    public TMPro.TextMeshProUGUI health;
    public TMPro.TextMeshProUGUI armor;
    public TMPro.TextMeshProUGUI magazine;
    public TMPro.TextMeshProUGUI reserve;

    public GameObject spawnCamera;

    public SelectionMap map;

    public int selectedWeapon = 0;

    public List<WeaponTypes> weapons = new List<WeaponTypes>();

    public bool paused = false;

    public GameObject pauseMenu;
    public GameObject hud;
    public GameObject deathScreen;
    public GameObject chooseSpawnScreen;
    public GameObject loadingScreen;

    public List<Image> weaponImages;
    public List<Sprite> weaponSprites;
    public List<Image> weaponBackgrounds;


    public GameObject timer;
    public TMPro.TextMeshProUGUI timerText;
    public TMPro.TextMeshProUGUI chooseSpawnTimer;
    public GameObject waitingPlayersMsg;
    public GameObject warmupMsg;
    public TMPro.TextMeshProUGUI playerCount;

    public TMPro.TextMeshProUGUI stats;
    public TMPro.TextMeshProUGUI resultText;
    private bool gameEnded = false;

    public GameObject scope;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            for (int i = 0; i < 4; i++)
                weapons.Add(WeaponTypes.NoWeapon);
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        try
        {
            HUDManager.instance.health.SetText(GameManager.players[Client.instance.gameId].stats.health.ToString());
            HUDManager.instance.armor.SetText(GameManager.players[Client.instance.gameId].stats.armor.ToString());
            HUDManager.instance.magazine.SetText(GameManager.players[Client.instance.gameId].stats.magazine.ToString());
            HUDManager.instance.reserve.SetText(GameManager.players[Client.instance.gameId].stats.reserve.ToString());
        } catch (Exception)
        {
            //Debug.LogError("Player not spawned");
        }

        SelectCurrentWeapon();
        ShowWeapons();
        PauseMenuhandler();
        GameEndHandler();
        MatchUIHandler();
    }

    private void MatchUIHandler()
    {
        if (paused)
            return;

        MatchStage stage = GameManager.instance.matchStage;
        int time = GameManager.instance.serverTime;
        int minute = time / 60;
        int second = time % 60;
        string format = "00";

        String timeText = minute.ToString(format) + ":" + second.ToString(format);

        try
        {
            switch (stage)
            {
                case MatchStage.waitingForPlayers:
                    CameraController.instance.HideCursor();
                    timer.SetActive(false);
                    waitingPlayersMsg.SetActive(true);
                    warmupMsg.SetActive(false);
                    chooseSpawnScreen.SetActive(false);
                    hud.SetActive(true);
                    playerCount.transform.parent.gameObject.SetActive(true);
                    playerCount.SetText(GameManager.players.Count.ToString());
                    break;

                case MatchStage.warmup:
                    CameraController.instance.HideCursor();
                    timer.SetActive(true);
                    timerText.SetText(timeText);
                    waitingPlayersMsg.SetActive(false);
                    warmupMsg.SetActive(true);
                    chooseSpawnScreen.SetActive(false);
                    hud.SetActive(true);
                    playerCount.transform.parent.gameObject.SetActive(false);
                    break;

                case MatchStage.chooseSpawn:
                    CameraController.instance.ShowCursor();
                    chooseSpawnScreen.SetActive(true);
                    Transform camera = Utils.RecursiveFindChild(GameManager.players[Client.instance.gameId].transform.root, "MainCamera");
                    camera.gameObject.SetActive(false);
                    chooseSpawnTimer.SetText(timeText);
                    hud.SetActive(false);
                    playerCount.transform.parent.gameObject.SetActive(false);
                    break;

                case MatchStage.match:
                    if (!gameEnded)
                    {
                        camera = Utils.RecursiveFindChild(GameManager.players[Client.instance.gameId].transform.root, "MainCamera");
                        camera.gameObject.SetActive(true);
                        CameraController.instance.HideCursor();
                        timer.SetActive(false);
                        waitingPlayersMsg.SetActive(false);
                        warmupMsg.SetActive(false);
                        chooseSpawnScreen.SetActive(false);
                        hud.SetActive(true);
                        playerCount.transform.parent.gameObject.SetActive(true);
                        playerCount.SetText(GameManager.players.Count.ToString());
                    }
                    break;
            }
        } 
        catch (Exception)
        {
            Debug.LogError("HUD Got an error");
        }
       
    }

    private void GameEndHandler()
    {
        if (gameEnded && Input.anyKey)
        {
            Client.instance.Disconnect();
            SceneManager.LoadScene((int)Scenes.UI);
        }
    }

    private void PauseMenuhandler()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            pauseMenu.SetActive(paused);
            if (paused)
            {
                CameraController.instance.ShowCursor();
                pauseMenu.SetActive(true);
                hud.SetActive(false);
            }
            else
            {
                CameraController.instance.HideCursor();
                pauseMenu.SetActive(false);
                hud.SetActive(true);
            }
        }
    }

    private void SelectCurrentWeapon()
    {
        int i = 0;

        foreach(Image bg in weaponBackgrounds)
        {
            if (i == selectedWeapon)
            {
                Color tmp = bg.color;
                tmp.a = 0.192f;
                bg.color = tmp;
            }
            else
            {
                Color tmp = bg.color;
                tmp.a = 0;
                bg.color = tmp;
            }
            i++;
        }

    }

    private void ShowWeapons()
    {
        for(int i = 0; i < 4; i++)
        {
            if (weapons[i] == WeaponTypes.NoWeapon)
            {
                weaponImages[i].sprite = weaponSprites[4];
            }
            else
            {
                weaponImages[i].sprite = weaponSprites[(int)weapons[i]];
            }
        }
    }

    public void LeaveGame()
    {
        Client.instance.Disconnect();
        SceneManager.LoadScene((int)Scenes.UI);
    }

    public void Resume()
    {
        CameraController.instance.HideCursor();
        paused = false;
        pauseMenu.SetActive(false);
        hud.SetActive(true);
    }


    public void GameEnded(float damage, int kills, float hsPercent)
    {
        gameEnded = true;
        hud.SetActive(false);
        pauseMenu.SetActive(false);
        deathScreen.SetActive(true);
        resultText.SetText("YOU DIED");
        stats.SetText("Kills: " + kills +  "\n Damage: " + damage + "\n HeadShot%: " + (hsPercent * 100) + "% ");
        CameraController.instance.ShowCursor();
        Transform camera = Utils.RecursiveFindChild(GameManager.players[Client.instance.gameId].transform.root, "MainCamera");
        camera.gameObject.SetActive(false);
        Debug.Log("YOU LOST");
    }

    public void SpawnRemoteMark(Vector2 location)
    {
        map.SpawnRemotemark(location);
    }

    public void PlayerWon(float damage, int kills, float hsPercent)
    {
        gameEnded = true;
        hud.SetActive(false);
        pauseMenu.SetActive(false);
        resultText.SetText("YOU WON");
        deathScreen.SetActive(true);
        stats.SetText("Kills: " + kills + "\n Damage: " + damage + "\n HeadShot%: " + (hsPercent * 100) + "% ");
        CameraController.instance.ShowCursor();
        Transform camera = Utils.RecursiveFindChild(GameManager.players[Client.instance.gameId].transform.root, "MainCamera");
        camera.gameObject.SetActive(false);
        Debug.Log("YOU WON");
    }
}
