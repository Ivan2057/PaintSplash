using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class MainPanel : MonoBehaviour
{


    public void Friends()
    {
        SteamFriends.ActivateGameOverlayInviteDialog(SteamUser.GetSteamID());
      
    }

}
