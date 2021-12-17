using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour
{
    [Range(1,5)] public int maxNumberOfPlayers = 5;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject flock;
    public Canvas mainMenu;

    public InputActionAsset controls; 
    [Range(1,10)]  public float spawnRadius = 7;
    
    public Text playerCountText;
    private List<string> controlSchemeList;
    private List<GameObject> players;
    private int numberOfPlayers = 0;

    // Start is called before the first frame update
    void Awake()
    {
        mainMenu.enabled = true;
        flock.SetActive(false);
        players = new List<GameObject>();
        controlSchemeList = new List<string>();
        foreach(var control in controls.controlSchemes)
        {
            controlSchemeList.Add(control.name);
        }
    }

    public void IncreasePlayerCount()
    {
        if(numberOfPlayers < maxNumberOfPlayers) numberOfPlayers++;
        playerCountText.text = "Players: " + numberOfPlayers;
    }
    
    public void DecreasePlayerCount()
    {
        if(numberOfPlayers > 0) numberOfPlayers--;
        playerCountText.text = "Players: " + numberOfPlayers;
    }

    public void StartGame()
    {
        SpawnPlayers();
        mainMenu.enabled = false;
        // flock.SetActive(true);
    }

    public void SpawnPlayers()
    {
        for(int i = 0; i < numberOfPlayers; i++)
        {
            var newPlayer = PlayerInput.Instantiate(playerPrefab, controlScheme: controlSchemeList[i], pairWithDevice: Keyboard.current).gameObject;
            players.Add(newPlayer);
        }
        foreach(var player in players)
        {
            player.transform.position = Random.insideUnitCircle * spawnRadius;
            player.transform.rotation = Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f));
        }
    }
}
