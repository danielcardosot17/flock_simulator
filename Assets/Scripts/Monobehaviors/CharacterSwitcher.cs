using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class CharacterSwitcher : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    PlayerInputManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<PlayerInputManager>();
        Debug.Log("BBBBBBBBBBBBB");
        
    }

    public void SpawnPlayer()
    {

    }
    
    public void SpawnEnemy()
    {
        
    }
}
