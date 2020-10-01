using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;

namespace Assets.Scripts.GameScripts
{
    class PlayerPrefsManager : PlayerPrefs
    {
        private readonly byte _playerSens;
        private readonly byte _volume_master;
        private readonly byte _volume_music;
        private readonly byte _volume_soundEffects;



        public static float PlayerSensitivity
        {
            get
            {
                return GetFloat(nameof(_playerSens));
            }

            set
            {
                if (value > 0 && value < 1)
                {
                    SetFloat(nameof(_playerSens), value);
                }
            }
        }

        public static float VolumeMaster
        {
            get => GetFloat(nameof(_volume_master));
            set
            {
                SetFloat(nameof(_volume_master), value);
            }
        }
        public static float VolumeMusic
        {
            get => GetFloat(nameof(_volume_music));
            set
            {
                SetFloat(nameof(_volume_music), value);
            }
        }
        public static float VolumeSoundEffects
        {
            get => GetFloat(nameof(_volume_soundEffects));
            set
            {
                SetFloat(nameof(_volume_soundEffects), value);
            }
        }


    }
}
