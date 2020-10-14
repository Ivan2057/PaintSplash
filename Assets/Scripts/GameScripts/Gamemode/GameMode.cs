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
    
    public int teamAmmount = 2;

    [SyncVar] 
    public int ScoreTeam0 = 0;

    [SyncVar] 
    public int ScoreTeam1 = 0;

    [SerializeField]
    public GameObject canvasPuntaje;

    public int Meta = 500;

    public List<Transform> spawnPoints;

    public static List<Player> _lstPlayers = new List<Player>();




    protected void Start()
    {
        Debug.Log("Setting up game");
    }


    public static void LoadPlayer(Player p)
    {
        _lstPlayers.Add(p);
    }

    public static List<Player> GetPlayerList()
    {
        return _lstPlayers;
    }
    public static void UpdatePlayer(Player p)
    {
        int i = 0;
        while(i < _lstPlayers.Count  && _lstPlayers[i] != p) 
        {
            i++;
        }
        if(_lstPlayers[i].jugador.name.Equals(p.jugador.name))
        {
            _lstPlayers[i] = p;
        }
    }

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



