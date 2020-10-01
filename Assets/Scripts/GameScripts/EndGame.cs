using Assets.Scripts.GameScripts;
using Assets.Scripts.WeaponScripts;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;

public class EndGame : MonoBehaviour
{
    [SerializeField] public TMP_Text[] Team0Names = new TMP_Text[5];
    [SerializeField] public TMP_Text[] Team1Names = new TMP_Text[5];
    [SerializeField] public TMP_Text FinTeam0;
    [SerializeField] public TMP_Text FinTeam1;
    [SerializeField] public TMP_Text Puntaje;
    [SerializeField] public GameObject CanvasFin;
    [SerializeField] public Behaviour[] disableOnEnd;
    private GameModeCTF gameModeCTF;

    public void FinJuego(int Ganador)
    {
        gameModeCTF = FindObjectOfType<GameModeCTF>();
        gameModeCTF.canvasPuntaje.SetActive(false);
        for (int i = 0; i < disableOnEnd.Length; i++)
        {
            disableOnEnd[i].enabled = false;
        }
        CanvasFin.SetActive(true);
        List<string> Team;
        for (int x = 0; x < 2; x++)
        {
            Team = TeamManager.TraerTeam(x);
            for (int i = 0; i < Team.Count; i++)
            {
                if (x == 0)
                {
                    Team0Names[i].text = Team[i];
                }else if(x == 1)
                {
                    Team1Names[i].text = Team[i];
                }
            }
        }
        if(Ganador == 0)
        {
            FinTeam0.text = "Victoria";
            FinTeam1.text = "Derrota";
        }else if(Ganador == 1)
        {

            FinTeam1.text = "Victoria";
            FinTeam0.text = "Derrota";
        }
        Puntaje.text = gameModeCTF.ScoreTeam0.ToString() + " - " + gameModeCTF.ScoreTeam1.ToString();
    }
}
