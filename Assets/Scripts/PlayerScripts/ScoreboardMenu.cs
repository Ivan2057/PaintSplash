using Assets.Scripts.GameScripts;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.PlayerScripts
{
    class ScoreboardMenu : NetworkBehaviour
    {
        [SerializeField]
        GameObject playerScoreboardItem;

        [SerializeField]
        Transform playerScoreboardList;
        [SerializeField]
        Transform playerScoreboardList1;

        [SerializeField]
        private GameObject scoreboardUI;

        private static bool active = false;

        private void Start()
        {
            if (scoreboardUI == null) Debug.LogError("Scoreboard UI No existe!!!"); 
        }
        [Client]
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                on();
            }else if (Input.GetKeyUp(KeyCode.Tab)){
                off();
            }
           
        }

        public void on()
        {
            scoreboardUI.SetActive(true);
            List<Player> lst = new List<Player>(GameMode.GetPlayerList());
            foreach ( Player p in lst)
            {
                GameObject itemGO = new GameObject();
                if (p.Team == 0)
                {
                    itemGO = (GameObject)Instantiate(playerScoreboardItem, playerScoreboardList);
                }
                else if(p.Team == 1)
                {
                    itemGO = (GameObject)Instantiate(playerScoreboardItem, playerScoreboardList1);
                }
              PlayerScoreboardItem item = itemGO.GetComponent<PlayerScoreboardItem>();
              if(item != null)
                {
                    item.Setup(p.name, p.kills, p.muertes);
                }
            
            }
            
        }

        private void off()
        { 
            foreach (Transform child in playerScoreboardList)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in playerScoreboardList1)
            {
                Destroy(child.gameObject);
            }
            scoreboardUI.SetActive(false);
        }

        
    }
}
