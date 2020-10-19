using Assets.Scripts.GameScripts;
using Mirror;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.NetworkScripts
{
    public class NetworkRoomPlayerLobby : NetworkBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject lobbyUI = null;

        [SerializeField] private TMP_InputField RoomId;
        [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
        [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[4];
        [SerializeField] private RawImage[] playerRawImage = new RawImage[4];
        [SerializeField] private Button startGameButton = null;
        [SerializeField] private Dropdown dropdownMaps = null;

        [SyncVar(hook = nameof(HandleDisplayNameChanged))]
        public string DisplayName = "Loading...";
        [SyncVar(hook = nameof(HandleReadyStatusChanged))]
        public bool IsReady = false;


        [SyncVar]
        public string steamID;

        private bool isLeader;

        [SyncVar]
        private string _roomId;
        public bool IsLeader
        {
            set
            {
                isLeader = value;
                RoomId.text = RoomFirebaseSteam.GetRoomId();
                _roomId = RoomId.text;
                startGameButton.gameObject.SetActive(value);
                dropdownMaps.gameObject.SetActive(value);
            }
        }


        private NetworkManagerLobby room;
        private NetworkManagerLobby Room
        {
            get
            {
                if (room != null) { return room; }
                return room = NetworkManager.singleton as NetworkManagerLobby;
            }
        }


        public void Start()
        {
            List<string> roomList = Room.mapSet.Maps.ToList();
            dropdownMaps.ClearOptions();

            foreach (string itemMap in roomList)
            {
                String mapName = Path.GetFileNameWithoutExtension(itemMap);
                List<string> m_DropOptions = new List<string> { mapName };
                dropdownMaps.AddOptions(m_DropOptions);
            }

        }
        public override void OnStartAuthority()
        {
            CmdSetSteamId(SteamUser.GetSteamID().ToString());
            CmdSetDisplayName(SteamFriends.GetPersonaName());

            lobbyUI.SetActive(true);
        }

        public override void OnStartClient()
        {
            Room.RoomPlayers.Add(this);
            UpdateDisplay();
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            Room.RoomPlayers.Remove(this);

            UpdateDisplay();

        }

        [Obsolete]
        public override void OnNetworkDestroy()
        {
            Room.RoomPlayers.Remove(this);

            UpdateDisplay();
        }

        public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();
        public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

        private void UpdateDisplay()
        {
            if (!hasAuthority)
            {
                foreach (var player in Room.RoomPlayers)
                {
                    if (player.hasAuthority)
                    {
                        player.UpdateDisplay();
                        break;
                    }
                }
                RoomId.text = _roomId;
                return;
            }

            for (int i = 0; i < playerNameTexts.Length; i++)
            {
                playerNameTexts[i].text = "Esperando Jugador...";
                playerReadyTexts[i].text = string.Empty;
            }

            for (int i = 0; i < Room.RoomPlayers.Count; i++)
            {
                playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
                try
                {
                    playerRawImage[i].texture = GetImage(Room.RoomPlayers[i].steamID);
                }
                catch(Exception e)
                {
                    Debug.LogError("No se pudo obtener el avatar del usuario : " + e.Message);
                }
                
                playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady ?
                    "<color=green>Listo!</color>" :
                    "<color=red>Aun no</color>";
            }
        }

        public void HandleReadyToStart(bool readyToStart)
        {
            if (!isLeader) { return; }

            startGameButton.interactable = readyToStart;
        }


        [Command]
        private void CmdSetSteamId(string steamId)
        {
            steamID = steamId;
        }
        [Command]
        private void CmdSetDisplayName(string displayName)
        {
            DisplayName = displayName;

        }
        [Command]
        public void CmdReadyUp()
        {
            IsReady = !IsReady;

            Room.NotifyPlayersOfReadyState();
        }

        [Command]
        public void CmdStartGame()
        {      
            if (!String.Equals(Room.RoomPlayers[0].connectionToClient, connectionToClient)) { return; }
            Room.StartGame(dropdownMaps.value);            
        }

        public Texture2D GetImage(string _steamId)
        {
            Texture2D texture = null;

            ulong stimUlong = ulong.Parse(_steamId);
            var csSteamID = new CSteamID(stimUlong);
            int imageId = SteamFriends.GetLargeFriendAvatar(csSteamID);


            bool isValid = SteamUtils.GetImageSize(imageId, out uint w, out uint h);
            
            if (isValid)
            {
                byte[] image = new byte[w * h * 4];



                texture = new Texture2D((int)w, (int)h, TextureFormat.RGBA32, false, true);
                texture.LoadRawTextureData(image);
                texture.Apply();
            }

            return texture;
        }
    }
}
