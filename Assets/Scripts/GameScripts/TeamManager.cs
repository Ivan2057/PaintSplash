using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Assets.Scripts.NetworkScripts;
using Assets.Scripts.PlayerScripts;

namespace Assets.Scripts.GameScripts
{
    public class TeamManager : NetworkBehaviour
    {
        private static List<NetworkGamePlayerLobby> ListJugadores = new List<NetworkGamePlayerLobby>();
        public static int i = 0;
        public static List<Vector3> SpawnTeam1 = new List<Vector3>();
        public static List<Vector3> SpawnTeam0 = new List<Vector3>();

        //marcos -- used to sync all scores
        private static List<GameObject> LstPlayerPrefabs = new List<GameObject>(); 

        public static void AddPlayerPrefab(GameObject miPlayer)
        {
            
            LstPlayerPrefabs.Add(miPlayer);
        }
        public static List<GameObject> GetPlayerPrefab()
        {
            return LstPlayerPrefabs;
        }
        //
        public static void AñadirJugador(NetworkGamePlayerLobby player)
        {
            player.Team = SetTeam();
            ListJugadores.Add(player);
        }
        
        public static List<NetworkGamePlayerLobby> TraerLista()
        {
            return ListJugadores;
        }

        public static int SetTeam()
        { int team = -1;
            if (ListJugadores.Count % 2 == 0)
            {
                team = 0;
            }
            else if (ListJugadores.Count % 2 != 0)
            {
                team = 1;
            }
            return team;
        }
        public static NetworkGamePlayerLobby TraerJugador(NetworkConnection conn)
        { int pos = 0;

            while(pos < ListJugadores.Count && ListJugadores[pos].connectionToClient != conn)
            {
                pos++;
            }
            return ListJugadores[pos];
        }

        public static int ReturnTeam(int conn)
        {
            int team = -1;
            int i = 0;
           
            while(i < ListJugadores.Count && conn != ListJugadores[i].connectionToClient.connectionId)
            {
                i++;
            }
            if (ListJugadores[i].Team == 0)
            {
                team = 0;
            }
            else if(ListJugadores[i].Team == 1)
            {
                team = 1;
            }
            return team;
                
        }

       public static List<string> TraerTeam(int TeamId)
        { List<string> Team = new List<string>();

            for(int i = 0; i < ListJugadores.Count;i++)
            {
                if(ListJugadores[i].Team == TeamId)
                {
                    Team.Add(ListJugadores[i].displayName);
                }
            }
            return Team;
        }

        // Devuelve todos los jugadores conectados 
        public static List<Player> GetAllPlayers()
        {
            List<Player> listaPlayers = new List<Player>();

            




            return listaPlayers;
        }
    }
}