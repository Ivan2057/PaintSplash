using Assets.Scripts.GameScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.NetworkScripts;
using Mirror;
using Assets.Scripts.PlayerScripts;
using TMPro;

public class GameMode : NetworkBehaviour
{
    //set up
    public int teamAmmount = 2;

    [SyncVar] public int ScoreTeam0 = 0;
    [SyncVar] public int ScoreTeam1 = 0;
    [SerializeField] public GameObject canvasPuntaje;
    public int Meta = 500;
    public List<Transform> spawnPoints;

    protected void Start()
    {
        Debug.Log("Setting up game");
    }

    /*
    [Client]
    public void AddScore(int teamID, int score)
    {
        Debug.Log("Soy Un Cliente");
        CmdServerAddPoint(teamID, score);
    }
    */

    [Command]
    public void CmdServerAddPoint(int teamID, int score)
    {
        Debug.Log("Soy el servidor");
        if (teamID == 0)
        {
            ScoreTeam0 += score;
        }
        else
        {
            ScoreTeam1 += score;
        }
        RpcAddScoreToCanvas(teamID, score);
    }
    [ClientRpc]
    void RpcAddScoreToCanvas(int teamID, int score)
    {
        Debug.Log("RpcAddScoreCanvas");
        Debug.Log(teamID);
        if (teamID == 0)
        {
            GameObject.Find("time0").GetComponent<TextMeshProUGUI>().text = score.ToString();
        }
        else
        {
            GameObject.Find("time1").GetComponent<TextMeshProUGUI>().text = score.ToString();
        }
        
    }

    [Server]
    public void PlayerWasKilled(GameObject player, Transform pos)
    {
        player.transform.position = pos.position;
        player.transform.rotation = pos.rotation;
        player.GetComponent<Player>().SetDefaults();

    }
}



