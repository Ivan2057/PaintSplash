using Assets.Scripts.PlayerScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Flag : MonoBehaviour
{
    [SerializeField] int teamID;
    public Vector3 originalLocation;


    private void Start()
    {
        originalLocation = transform.position;
 
    }


    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();


        if(player != null)
        {//its a player
            if (player.Team == teamID)
            {//cant pick up your own team's flag

                //return flag
                return;
            }

            Debug.Log("Capturing Flag");

            player.PickUpWeapon(gameObject, originalLocation, teamID,1);

            gameObject.SetActive(false);
        }
    }
}
