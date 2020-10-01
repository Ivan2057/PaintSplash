using Assets.Scripts.NetworkScripts;
using Mirror;
using UnityEngine;

namespace Assets.Scripts.PlayerScripts
{
    namespace Assets.Scripts.PlayerScripts
    {
        [RequireComponent(typeof(Player))]
        public class PlayerSetup : NetworkBehaviour
        {

            [SerializeField]
            Behaviour[] componentsToDisable;

            [SerializeField]
            string remoteLayerName = "RemotePlayer";

            [SerializeField]
            string dontDrawLayerName = "DontDraw";

            [SerializeField]
            GameObject playerGraphics;

            [SerializeField]
            GameObject playerUIPrefab;
            private GameObject playerUIInstance;


            Camera sceneCamera;
            void Start()
            {
                if (!PlayerController.localPlayer)
                {
                    DisableComponents();
                    AssignRemoteLayer();
                    return;
                }
                else
                {
                    sceneCamera = Camera.main;
                    if (sceneCamera != null)
                    {
                        sceneCamera.gameObject.SetActive(false);
                    }

                    //disable player graphjics for local player
                    SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

                    //Cre3ate player UI
                    playerUIInstance = Instantiate(playerUIPrefab);
                    playerUIInstance.name = playerUIPrefab.name;


                }


                GetComponent<Player>().Setup();
            }

            void SetLayerRecursively(GameObject obj, int newLayer)
            {
                obj.layer = newLayer;
                foreach (Transform child in obj.transform)
                {
                    SetLayerRecursively(child.gameObject, newLayer);
                }
            }
            public override void OnStartClient()
            {
                base.OnStartClient();

                string _netID = GetComponent<NetworkIdentity>().netId.ToString();
                Player _player = GetComponent<Player>();

              NetworkRoomManagerExt.RegisterPlayer(_netID, _player);


            }

            void AssignRemoteLayer()
            {
                gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
            }
            void DisableComponents()
            {
                for (int i = 0; i < componentsToDisable.Length; i++)
                {
                    componentsToDisable[i].enabled = false;
                }
            }

            private void OnDisable()
            {
                Destroy(playerUIInstance);



                if (sceneCamera != null)
                {
                    sceneCamera.gameObject.SetActive(true);
                }
             NetworkRoomManagerExt.UnRegisterPlayer(transform.name);
            }

        }
    }
}