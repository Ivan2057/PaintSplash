
using Assets.Scripts.PlayerScripts;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GameScripts
{
    public class CaptureZone : NetworkBehaviour
    {
        [SerializeField] public int teamID;
        [SerializeField] GameModeCTF gameModeCTF;

        private void Start()
        {
            gameModeCTF = FindObjectOfType<GameModeCTF>();
            if (gameModeCTF == null && GameObject.Find("GameModeCTF").GetComponent<GameModeCTF>())
            {
                Debug.LogError("Could not find GameModeCTF");
            }
        }

        private void OnTriggerEnter(Collider objectCollider)
        {
            Player player = objectCollider.GetComponent<Player>();
            
            if (player != null && gameModeCTF != null)
            {
                
                if (player.GetWeaponTeamID() != teamID)
                {
                    return;
                }

               
            }
        }

        
    }
}
