using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject EnemyBase;
    public GameObject FriendlyBase;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UnitSpawner[] Spawners = FindObjectsOfType<UnitSpawner>();
            for (int i = 0; i < Spawners.Length; i++)
            {
                Spawners[i].bSpawn = true;
            }
        }
    }
}
