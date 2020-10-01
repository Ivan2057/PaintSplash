using Assets.Scripts.GameScripts;
using Assets.Scripts.NetworkScripts;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

namespace Assets.Scripts.LobbyScripts.OnlinePanel
{
    class OnlinePanel : MonoBehaviour
    {
        [SerializeField] private NetworkManagerLobby networkManagerLobby = null;

        DatabaseReference mDatabaseRef;

        [Header("JoinLobby Thinks")]
        [SerializeField] private LUI_MenuCamControl CamControll = null;
        [SerializeField] private Transform LobbyMount = null;
        [SerializeField] private InputField ipAddressInputField = null;
        [SerializeField] private Button joinButton = null;

        protected Callback<LobbyCreated_t> LobbyCreated;
        protected Callback<LobbyEnter_t> lobbyEnter;
        protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
        private const string HostAddressKey = "HostAddress";
        private bool isFirebaseAppInstanciated = false;

        private string LobbySteamASD = "0";


        private void Start()
        {
            string conn = "https://paintsplash.firebaseio.com/";
            try
            {

                FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(conn);
                mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;

                isFirebaseAppInstanciated = true;
            }
            catch (Exception e)
            {
                Debug.LogError("Error al instanciar FirebaseApp: " + e.Message);

                isFirebaseAppInstanciated = false;
                RoomFirebaseSteam.isFirebaseAppInstanciated = isFirebaseAppInstanciated;
            }
            
            if(!SteamManager.Initialized)
            {
                Debug.LogError("Necesitas de Steam para poder jugar");
                return;
            }
            gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);

        }

        private void Update()
        {
            if(LobbySteamASD != "0")
            {
                ConnectToLobby(LobbySteamASD);
                LobbySteamASD = "0";
            }
        }

        private void CreateRoomIdInFirebase()
        {

            Dictionary<string, System.Object> entryValues = RoomFirebaseSteam.ToDictionary();
            Dictionary<string, System.Object> childUpdates = new Dictionary<string, Object>();

            childUpdates["/Rooms/" + RoomFirebaseSteam.GetRoomId()] = entryValues;
            mDatabaseRef.UpdateChildrenAsync(childUpdates);

        }

        private void GetSteamIdFromRoomidAsync(string roomId)
        {
            string steamId = "";

              FirebaseDatabase.DefaultInstance
                      .GetReference("Rooms/" + roomId)
                      .GetValueAsync().ContinueWith(task =>
                      {
                          if (task.IsFaulted)
                          {
                              Debug.LogError("ERROR al encontrar la RoomId");
                              steamId = "";
                          }
                          else if (task.IsCompleted)
                          {
                              DataSnapshot snapshot = task.Result;

                              Dictionary<string, System.Object> dictionary = new Dictionary<string, Object>();
                              dictionary = (Dictionary<string, System.Object>)snapshot.Value;
                              foreach (KeyValuePair<string, System.Object> miRoom in dictionary)
                              {
                                      //recorre el diccionario de [0] elementos y agarra el value de la room;

                                      steamId = miRoom.Value.ToString();
                                  LobbySteamASD = steamId;
                                      Debug.Log("Lobby Encontrada:" + steamId);

                               
                              }

                          }
                      });
        }

        public void HostLobby()
        {
            if (isFirebaseAppInstanciated)
            {
                CreateRoomIdInFirebase();
            }

            LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            lobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
            networkManagerLobby.StartHost();
        }

        private bool IsValidSteamId(string SteamId)
        {
            bool isValid = false;
            try
            {
                CSteamID stimid = new CSteamID(ulong.Parse(SteamId));

                if (SteamFriends.GetFriendPersonaName(stimid) != null)
                {
                    isValid = true;
                }
            }
            catch
            {
                isValid = false;
            }


            return isValid;
        }

        private bool ValidateInputLobby(string ipAddress)
        {
            bool isValid = false;
 


            if (ipAddress.Length == 4)
            {
                GetSteamIdFromRoomidAsync(ipAddress);
                
                isValid = true;
            }
            else if (!string.IsNullOrEmpty(ipAddress) && IsValidSteamId(ipAddress))
            {

                ConnectToLobby(ipAddress);
                isValid = true;
            }
            else if (ipAddress == "localhost")
            {
                ConnectToLobby(ipAddress);
                isValid = true;
            }


            return isValid;
        }

        public void JoinLobby()
        {
            string ipAddress = ipAddressInputField.text;
            if (!ValidateInputLobby(ipAddress)) Debug.LogError("La IP Ingresada no es Valida");
            
        }

        private void ConnectToLobby(string ipAddress)
        {
            networkManagerLobby.networkAddress = ipAddress;
            networkManagerLobby.StartClient();

            joinButton.interactable = false;
            CamControll.setMount(LobbyMount);
        }

        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            if(callback.m_eResult != EResult.k_EResultOK)
            {
                //btns.setActive(true);
                return;
            }

            SteamMatchmaking.SetLobbyData(
                new CSteamID(callback.m_ulSteamIDLobby),
                HostAddressKey,
                SteamUser.GetSteamID().ToString());
        }

        private void OnLobbyEntered(LobbyEnter_t callback)
        {

            string hostAddress = SteamMatchmaking.GetLobbyData(
                new CSteamID(callback.m_ulSteamIDLobby),
                HostAddressKey);

            networkManagerLobby.networkAddress = hostAddress;
            networkManagerLobby.StartClient();
        }

        private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t gameLobbyJoinRequested)
        {
            SteamMatchmaking.JoinLobby(gameLobbyJoinRequested.m_steamIDLobby);
        }
    }
}
