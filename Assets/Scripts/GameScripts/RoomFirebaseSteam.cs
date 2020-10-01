using Steamworks;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameScripts
{

    public class RoomFirebaseSteam
    {

        private static string _steamId;
        private static string _roomId;

        public static bool isFirebaseAppInstanciated = true;
        public static string GetRoomId()
        {
            _steamId = SteamUser.GetSteamID().ToString();
            string ipAdress = _steamId;
           
            if (isFirebaseAppInstanciated)
            {
                if (_roomId == null)
                {
                    ipAdress = GenerateRandomRoomId();
                }
                else
                {
                    ipAdress = _roomId;
                }
            }
            else
            {
               
                ipAdress = _steamId;
            }

            return ipAdress;
        }


       

        public static Dictionary<string, System.Object> ToDictionary()
        {
            if(_roomId == null)
            {
                _roomId = GenerateRandomRoomId();
            }

            Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
            result["steamId"] = SteamUser.GetSteamID().ToString();
            
            return result;
        }

        private static string GenerateRandomRoomId()
        {
            string randomRoomId;
            Random rnd = new Random();
            //TODO: Falta validar quen o exista otra room igual

                randomRoomId = rnd.Next(1000, 1010).ToString();
            return randomRoomId;
        }

       
    }
}
