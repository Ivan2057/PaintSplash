﻿using Assets.Scripts.NetworkScripts;
using UnityEngine;

namespace Assets.Scripts.LobbyScripts
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private NetworkManagerLobby networkManager = null;

        [Header("UI")]
        [SerializeField] private GameObject landingPagePanel = null;

        public void HostLobby()
        {
            
            networkManager.StartHost();

          //  landingPagePanel.SetActive(false);
        }
    }
}
