using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Wallet
{
    public int Gold;
}
public class GameManager : Singleton<GameManager>
{
    public GameObject EnemyBase;
    public GameObject FriendlyBase;

    private Wallet PlayerWallet;
    private Wallet EnemyWallet;

    public void ChangeMoney(UnitAllegiance WalletAllegiance , int MoneyDifference)
    {
        switch (WalletAllegiance)
        {
            case UnitAllegiance.friendly:
                PlayerWallet.Gold += MoneyDifference;
                break;
            case UnitAllegiance.enemy:
                EnemyWallet.Gold += MoneyDifference;
                break;
            default:
                break;
        }
    }
    public void ChangeMoney(Vector3 WorldPos,UnitAllegiance WalletAllegiance , int MoneyDifference)
    {
        switch (WalletAllegiance)
        {
            case UnitAllegiance.friendly:
                PlayerWallet.Gold += MoneyDifference;
                UIManager.Instance.DisplayTextOnScreen(WorldPos , MoneyDifference.ToString() , MoneyDifference >0 ? Color.yellow : Color.red);
                break;
            case UnitAllegiance.enemy:
                EnemyWallet.Gold += MoneyDifference;
                break;
            default:
                break;
        }
    }
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
