using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.PlayerScripts;
using UnityEngine;

namespace Assets.Scripts.WeaponScripts
{
    /// <summary>
    /// Check teamID, isWeaponLocked? isWeaponDropable.
    /// </summary>
    public class Weapon : NetworkBehaviour
    {
        [SerializeField] public int teamID;
        [SerializeField] public bool isWeaponLocked = false;
        [SerializeField] public bool isWeaponDropable = false;
        [SerializeField] public bool isWeaponShootable = false;

        [SerializeField] public GameObject worldWeaponGameObject;
        public Vector3 originalLocation;

        [SerializeField] private Weapon weapon = null;

        Camera c = new Camera();


        public override void OnStartAuthority()
        {
            enabled = true;
            weapon.gameObject.SetActive(true);

           
        }
        /// <summary>
        /// Set weapon to the current teamID player.
        /// </summary>
        /// <param name="teamID"></param>
        /// <param name="worldGameObject"></param>
        /// <param name="originalLocation"></param>
        [Client]
        public void SetWeaponGameObject(int teamID, GameObject worldGameObject, Vector3 originalLocation)
        {
            this.teamID = teamID;
            if (worldGameObject != null)
            {
                worldWeaponGameObject = worldGameObject;
            }
            this.originalLocation = originalLocation;
        }
        /// <summary>
        /// Drop weapon at set dropLocation
        /// </summary>
        /// <param name="player"></param>
        /// <param name="dropLocation"></param>
        /// 
        [Client]
        public void DropWeapon(Rigidbody player, Vector3 dropLocation)
        {
            
            float distanceToDrop = Vector3.Distance(Camera.allCameras[0].transform.position, dropLocation);
            Vector3 directionToDrop = (dropLocation - Camera.allCameras[0].transform.position).normalized;


            //ray to drop location
            Ray rayToDropLocation = new Ray(Camera.allCameras[0].transform.position, directionToDrop * distanceToDrop);
            RaycastHit raycastHit;
           
            if (Physics.Raycast(rayToDropLocation, out raycastHit, distanceToDrop))
            {
                dropLocation = raycastHit.point;
            }

            //set position in the world
            worldWeaponGameObject.transform.position = dropLocation;


            //ray down
            Renderer rend = worldWeaponGameObject.GetComponent<Renderer>();
            if (rend != null)
            {
                Vector3 topPoint = rend.bounds.center;
                topPoint.y += rend.bounds.extents.y;

                float height = rend.bounds.extents.y * 2;


                Ray rayDown = new Ray(topPoint, Vector3.down);
                RaycastHit raycastHitDown = new RaycastHit();
                if (Physics.Raycast(rayDown, out raycastHitDown, height * 1.1f))
                {
                    dropLocation = raycastHitDown.point;
                    dropLocation.y += rend.bounds.extents.y * 1.1f;
                }
            }

            worldWeaponGameObject.transform.position = dropLocation;



            Rigidbody flagRigidbody = worldWeaponGameObject.GetComponent<Rigidbody>();
            if (flagRigidbody != null && player != null)
            {
                flagRigidbody.velocity = player.velocity;
            }
        }
        public void DropGun(GameObject player, GameObject gun)
        {
            Player jugador = player.GetComponent<Player>();
            jugador.pistola.SetActive(false);
            GameObject pistola = Instantiate(gun,
                   player.transform.position + player.transform.forward + Vector3.up * 1.5f,
                   player.transform.rotation);
            pistola.transform.Rotate(15, 0, 40);
            pistola.GetComponent<Rigidbody>().AddForce(transform.forward * 5, ForceMode.Impulse);
        }
    }
}
