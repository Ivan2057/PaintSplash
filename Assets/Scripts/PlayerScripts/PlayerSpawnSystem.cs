using Assets.Scripts.LobbyScripts;
using Assets.Scripts.NetworkScripts;
using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.GameScripts;

namespace Assets.Scripts.PlayerScripts
{
    public class PlayerSpawnSystem : NetworkBehaviour
    {
        [SerializeField] private GameObject playerPrefab = null;
        [SerializeField] private GameObject playerPrefab1 = null;
        private int i = 0;
        private List<NetworkGamePlayerLobby> ListJugadores = new List<NetworkGamePlayerLobby>();
        private List<NetworkConnection> listConexiones = new List<NetworkConnection>();
        public static List<Transform> spawnPoints = new List<Transform>();
        public static List<Transform> spawnPoints1 = new List<Transform>();

        private int nextIndex = 0;
        private int nextIndex1 = 0;
        private int pos = 0;

        public static void AddSpawnPoint(Transform transform)
        {
            spawnPoints.Add(transform);

            spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
        }
        public static void RemoveSpawnPoint(Transform transform) => spawnPoints.Remove(transform);

        public static void AddSpawnPoint1(Transform transform)
        {
            spawnPoints1.Add(transform);

            spawnPoints1 = spawnPoints1.OrderBy(x => x.GetSiblingIndex()).ToList();
        }
        public static void RemoveSpawnPoint1(Transform transform) => spawnPoints1.Remove(transform);

        public override void OnStartServer() => NetworkManagerLobby.OnServerReadied += SpawnPlayer;

        public override void OnStartClient()
        {
            InputManager.Add(ActionMapNames.Player);
            InputManager.Controls.Player.Look.Enable();
        }

        [ServerCallback]
        private void OnDestroy() => NetworkManagerLobby.OnServerReadied -= SpawnPlayer;

       
        [Server]
        public void SpawnPlayer(NetworkConnection conn)
        {
            Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex);
            ListJugadores = TeamManager.TraerLista();
            GameObject playerInstance = null;

                if (spawnPoint == null)
                {
                    Debug.LogError($"Missing spawn point for player {nextIndex}");
                    return;
                }

                if (ListJugadores[i].Team == 0)
                {
                    playerInstance = Instantiate(playerPrefab, spawnPoints[nextIndex].position, spawnPoints[nextIndex].rotation);
                Player.posInicial = spawnPoints[nextIndex];
            }
                else if (ListJugadores[i].Team == 1)
                {
                    playerInstance = Instantiate(playerPrefab1, spawnPoints1[nextIndex1].position, spawnPoints1[nextIndex1].rotation);
                Player.posInicial = spawnPoints1[nextIndex1];
            }
                NetworkServer.Spawn(playerInstance, conn);
                nextIndex++;
                nextIndex1++;
                i++;
            

        }
    }
}
