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
        
        #region keys
        private readonly byte _playerSens;
        private readonly byte _volume_master;
        private readonly byte _volume_music;
        private readonly byte _volume_soundEffects;

        private readonly byte _move_left;
        private readonly byte _move_right;
        private readonly byte _move_back;
        private readonly byte _move_jump;
        private readonly byte _move_crouch;
        private readonly byte _move_forward;
        private readonly byte _move_run;

        private readonly byte _ui_scoreboard;
        #endregion 


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

        #region Volume
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
        #endregion

        #region Moves
        public static string MoveLeft
        {
            get
            {
                return GetString(nameof(_move_left));
            }

            set
            {
                SetString(nameof(_move_left), value);
            }
        }
        public static string MoveRight
        {
            get
            {
                return GetString(nameof(_move_right));
            }

            set
            {
                SetString(nameof(_move_right), value);
            }
        }
        public static string MoveFordward
        {
            get
            {
                return GetString(nameof(_move_forward));
            }

            set
            {
                SetString(nameof(_move_forward), value);
            }
        }
        public static string MoveBack
        {
            get
            {
                return GetString(nameof(_move_back));
            }

            set
            {
                SetString(nameof(_move_back), value);
            }
        }
        public static string MoveJump
        {
            get
            {
                return GetString(nameof(_move_jump));
            }

            set
            {
                SetString(nameof(_move_jump), value);
            }
        }
        public static string MoveCrouch
        {
            get
            {
                return GetString(nameof(_move_crouch));
            }

            set
            {
                SetString(nameof(_move_crouch), value);
            }
        }
        public static string MoveRun
        {
            get
            {
                return GetString(nameof(_move_run));
            }

            set
            {
                SetString(nameof(_move_run), value);
            }
        }
        #endregion


        #region UI

        public static string UIScoreboard
        {
            get
            {
                return GetString(nameof(_ui_scoreboard));
            }

            set
            {
                SetString(nameof(_ui_scoreboard), value);
            }
        }
        #endregion UI
    }
}
