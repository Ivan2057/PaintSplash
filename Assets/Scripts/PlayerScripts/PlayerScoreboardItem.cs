using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PlayerScripts
{
   public class PlayerScoreboardItem : MonoBehaviour
   {
        [SerializeField]
        Text usernameText;

        [SerializeField]
        Text killsText;

        [SerializeField]
        Text deathsText;

        public void Setup(string username, int kills, int deaths)
        {
            usernameText.text = username;
            killsText.text = "Kills: " + kills;
            deathsText.text = "Deaths: " + deaths;
        }
   }
}
