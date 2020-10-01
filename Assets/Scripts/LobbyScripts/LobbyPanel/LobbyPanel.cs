using Assets.Scripts.NetworkScripts;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LobbyScripts.LobbyPanel
{
    class LobbyPanel : MonoBehaviour
    {




        public void CloseLobby()
        {


            NetworkManager.singleton.StopClient();
            NetworkManager.singleton.StopHost();

            

            NetworkServer.DisconnectAll();

            StartCoroutine(ExitDelay());
        }
        IEnumerator ExitDelay()
        {
            yield return new WaitForSeconds(0.1f);//attends un peu
          // Destroy(NetworkManager.singleton.gameObject);


        }
    }
}
