using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Assets.Scripts.LobbyScripts
{
    public class SettingsMenu : MonoBehaviour
    {
        Resolution[] resolutions;
       // public Dropdown dropdownMenu;
        public AudioMixer audioMixer;
        void Start()
        {/*
            int currentResolution = 0;
            Screen.fullScreen = true;
            resolutions = Screen.resolutions;
            dropdownMenu.onValueChanged.AddListener(delegate { Screen.SetResolution(resolutions[dropdownMenu.value].width, resolutions[dropdownMenu.value].height, false); });
            for (int i = 0; i < resolutions.Length; i++)
            {
                dropdownMenu.options[i].text = ResToString(resolutions[i]);
                dropdownMenu.value = i;
                dropdownMenu.options.Add(new Dropdown.OptionData(dropdownMenu.options[i].text));
                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolution = i;
                }
            }
            dropdownMenu.value = currentResolution;
            */
        }

        /// <summary>
        /// Set the game fullscreen if true.
        /// </summary>
        /// <param name="isFullScreen"></param>
        public void SetFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
        }
        /// <summary>
        /// Change the quality of the game
        /// </summary>
        /// <param name="qualityIndex"></param>
        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }
        /// <summary>
        /// Reduce/Increase the audio source volume
        /// </summary>
        /// <param name="volume"></param>
        public void SetVolume(float volume)
        {
    //        Debug.Log("Volume: " + Value);
            audioMixer.SetFloat("Volume", volume);
            
        }
        /// <summary>
        /// Change resolutin.
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        string ResToString(Resolution res)
        {
            return res.ToString();//res.width + " x " + res.height;
        }
    }
}

