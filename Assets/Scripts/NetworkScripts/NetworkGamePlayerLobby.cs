using Assets.Scripts.GameScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Assets.Scripts.NetworkScripts
{
    public class NetworkGamePlayerLobby : NetworkBehaviour
    {
        [SyncVar]
        public string displayName = "Loading...";

        public int Team;
        public Vector3 SpawnPosition;
        private NetworkManagerLobby room;
        private NetworkManagerLobby Room
        {
            get
            {
                if (room != null) { return room; }
                return room = NetworkManager.singleton as NetworkManagerLobby;
            }
        }

        public override void OnStartClient()
        {
            DontDestroyOnLoad(gameObject);

            Room.GamePlayers.Add(this);

            TeamManager.AñadirJugador(this);
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
        }


        public override void OnNetworkDestroy()
        {
            Room.GamePlayers.Remove(this);
        }

        [Server]
        public void SetDisplayName(string displayName)
        {
            this.displayName = displayName;
        }


    }
}
