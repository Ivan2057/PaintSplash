﻿
using Assets.Scripts.NetworkScripts;
using Mirror;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.LobbyScripts
{
    public class RoundSystem : NetworkBehaviour
    {
        [SerializeField] private Animator animator = null;

        private NetworkManagerLobby room;
        private NetworkManagerLobby Room
        {
            get
            {
                if (room != null) { return room; }
                return room = NetworkManager.singleton as NetworkManagerLobby;
            }
        }


        #region Server

        public override void OnStartServer()
        {
            NetworkManagerLobby.OnServerStopped += CleanUpServer;
            NetworkManagerLobby.OnServerReadied += CheckToStartRound;
        }

        [ServerCallback]
        private void OnDestroy() => CleanUpServer();

        [Server]
        private void CleanUpServer()
        {
            NetworkManagerLobby.OnServerStopped -= CleanUpServer;
            NetworkManagerLobby.OnServerReadied -= CheckToStartRound;
        }

        [ServerCallback]
        public void StartRound()
        {
            RpcStartRound();
        }

        [Server]
        private void CheckToStartRound(NetworkConnection conn)
        {
            if (Room.GamePlayers.Count(x => x.connectionToClient.isReady) != Room.GamePlayers.Count) { return; }

        }

        #endregion



        [ClientRpc]
        private void RpcStartRound()
        {
            InputManager.Remove(ActionMapNames.Player);
        }

    }
}
