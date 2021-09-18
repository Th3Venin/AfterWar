using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, ItemSpawner> spawners = new Dictionary<int, ItemSpawner>();
    public List<GameObject> spawnerPrefabs;
    public List<GameObject> weaponPrefabs;
    public List<Vector3> weaponPosLocal;
    public List<Quaternion> weaponRotLocal;

    public List<Vector3> weaponPosRemote;
    public List<Quaternion> weaponRotRemote;

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public GameObject itemSpawnerPrefab;

    public MatchStage matchStage;
    public int serverTime;

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

        // Weapon Order: AK, G28, M4, Revolver

        weaponPosLocal.Add(new Vector3(0.012f, 0.274f, 0.186f));
        weaponPosLocal.Add(new Vector3(0.002f, 0.22f, -0.081f));
        weaponPosLocal.Add(new Vector3(0.004f, 0.11f, 0.376f));
        weaponPosLocal.Add(new Vector3(0.002f, 0.22f, -0.081f));

        weaponRotLocal.Add(Quaternion.Euler(5.6f, 88.5f, -93f));
        weaponRotLocal.Add(Quaternion.Euler(-84, -241, 240));
        weaponRotLocal.Add(Quaternion.Euler(176f, -90f, 88.5f));
        weaponRotLocal.Add(Quaternion.Euler(-84, -241, 240));

        weaponPosRemote.Add(new Vector3(-0.035f, -0.023f, -0.267f));
        weaponPosRemote.Add(new Vector3(-0.079f, 0.058f, 0.001f));
        weaponPosRemote.Add(new Vector3(-0.082f, -0.081f, -0.43f));
        weaponPosRemote.Add(new Vector3(0.08f, 0.103f, 0.092f));

        weaponRotRemote.Add(Quaternion.Euler(-51f, -118f, -63));
        weaponRotRemote.Add(Quaternion.Euler(-36f, 86f, 72));
        weaponRotRemote.Add(Quaternion.Euler(-63.948f, -128.177f, -63.527f));
        weaponRotRemote.Add(Quaternion.Euler(-73f, -192f, 95));

    }

    private void Start()
    {
    }

    private void Update()
    {

    }

    public void SpawnPlayer(int id, string username, Vector3 position, Quaternion rotation, int deaths, int score, int kills)
    {
        GameObject player;

        if (id == Client.instance.gameId)
        {
            Debug.Log("Spawning Local Player");
            player = Instantiate(localPlayerPrefab, position, rotation);
        }
        else
        {
            player = Instantiate(playerPrefab, position, rotation);
        }

        player.GetComponent<PlayerManager>().Initialize(id, username, deaths, kills, score);

        players.Add(id, player.GetComponent<PlayerManager>());
    }

    public void CreateItemSpawner(int spawnerId, bool hasItem, Vector3 position, WeaponTypes type)
    {
        GameObject spawner = Instantiate(spawnerPrefabs[(int)type], position, itemSpawnerPrefab.transform.rotation);
        spawner.GetComponent<ItemSpawner>().Intitialize(spawnerId, hasItem, position, type);
        spawners.Add(spawnerId, spawner.GetComponent<ItemSpawner>());

        //Debug.Log("Created Item spawner of type " + (int)type);
    }
}
