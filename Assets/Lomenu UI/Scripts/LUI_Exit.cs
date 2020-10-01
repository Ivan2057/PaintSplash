using UnityEngine;
using System.Collections;
using Assets.Scripts.GameScripts;

public class LUI_Exit : MonoBehaviour
{	
		public void QuitGame()
		{
			GameManager gm = new GameManager();
			gm.QuitGame();

		}
}
