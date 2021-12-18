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
    public Canvas endScreen;

    public InputActionAsset controls; 
    [Range(1,10)]  public float spawnRadius = 7;
    
    public Text playerCountText;
    public Text winnerText;
    public InputField nameInput;
    private List<string> controlSchemeList;
    private List<GameObject> players;
    public List<GameObject> Players { get => players; private set => players = value; }
    private List<string> playersNames;
    private int numberOfPlayers = 0;
    private bool isEndgame = false;
    private bool hasStarted = false;


    // Start is called before the first frame update
    void Awake()
    {
        playerCountText.text = "0 Players: ";
        isEndgame = false;
        hasStarted = false;
        mainMenu.enabled = true;
        flock.SetActive(false);
        Players = new List<GameObject>();
        playersNames = new List<string>();
        controlSchemeList = new List<string>();
        foreach(var control in controls.controlSchemes)
        {
            controlSchemeList.Add(control.name);
        }
    }

    private void Update() {
        if(hasStarted && !isEndgame)
        {
            if(WinCondition()) EndGame();
        }    
    }

    private bool WinCondition()
    {
        return players.Count <= 1;
    }

    private void EndGame()
    {
        isEndgame = true;
        endScreen.enabled = true;
        if(players.Count == 1)
        {
            winnerText.text = GetWinnerName() + " Boid Won!!";
        }
    }

    private string GetWinnerName()
    {
        if(players.Count == 1)
        {
            return players[0].name;
        }
        else return "No";
    }

    public void IncreasePlayerCount()
    {
        if(numberOfPlayers < maxNumberOfPlayers)
        {
            numberOfPlayers++;
            if(string.IsNullOrEmpty(nameInput.text))
            {
                nameInput.text = "Player_" + numberOfPlayers;
            }
            playersNames.Add(nameInput.text);
            ShowPlayersNames();
        }
    }

    public void DecreasePlayerCount()
    {
        if(numberOfPlayers > 0)
        {
            numberOfPlayers--;
            playersNames.RemoveAt(playersNames.Count - 1);
            ShowPlayersNames();
        }
    }

    private void ShowPlayersNames()
    {
        var names = numberOfPlayers + " Players: ";
        foreach(var name in playersNames)
        {
            names += name + " ";
        }
        playerCountText.text = names;
    }

    public void StartGame()
    {
        if(numberOfPlayers > 1)
        {
            SpawnPlayers();
            mainMenu.enabled = false;
            flock.SetActive(true);
            flock.GetComponent<Flock>().InitializeFlock();
            hasStarted = true;
            isEndgame = false;
        }
    }

    public void RestartGame()
    {
        isEndgame = false;
        endScreen.enabled = false;
        hasStarted = false;
        mainMenu.enabled = true;
        flock.GetComponent<Flock>().ResetFlock();
        flock.SetActive(false);
        DestroyAllPlayersAndClearList();
    }

    private void DestroyAllPlayersAndClearList()
    {
        for(int i = players.Count - 1; i >= 0; i--)
        {
            DestroySinglePlayerAndRemoveFromList(players[i]);
        }
    }

    public void DestroySinglePlayerAndRemoveFromList(GameObject player)
    {
        DecreasePlayerCount();
        players.Remove(player);
        Destroy(player);
    }

    public void SpawnPlayers()
    {
        for(int i = 0; i < numberOfPlayers; i++)
        {
            var newPlayer = PlayerInput.Instantiate(playerPrefab, controlScheme: controlSchemeList[i], pairWithDevice: Keyboard.current).gameObject;
            newPlayer.name = playersNames[i];
            Players.Add(newPlayer);
        }
        foreach(var player in Players)
        {
            player.transform.position = Random.insideUnitCircle * spawnRadius;
            player.transform.rotation = Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f));
        }
    }
}
