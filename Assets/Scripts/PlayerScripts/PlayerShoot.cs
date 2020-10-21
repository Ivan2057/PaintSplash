using Assets.Scripts.NetworkScripts;
using Assets.Scripts.WeaponScripts;
using Mirror;
using UnityEngine;


namespace Assets.Scripts.PlayerScripts
{
    [RequireComponent(typeof(WeaponManager))]
    public class PlayerShoot : NetworkBehaviour
    {

        private const string PLAYER_TAG = "PlayerScripts";
        [Header("Arma")]
        public LayerMask CanBeShoot;
        [SyncVar]
        public string tag;
        [Header("BulletHole")]
        public GameObject bulletHolePrefab;
        [SerializeField] GameObject[] disableOnClient;

        [SerializeField]
        private Camera cam;

        [SerializeField]
        private LayerMask mask = new LayerMask();

        public PlayerWeapon currentWeapon;
        private WeaponManager weaponManager;
        PlayerShoot playershoter;
        RaycastHit _hit;
        




        void Start()
        {
            if (cam == null)
            {
                Debug.LogError("PlayerShoot: No camera referenced!");
                this.enabled = false;
            }
            foreach (GameObject go in disableOnClient)
            {
                go.SetActive(false);
            }

           
            weaponManager = GetComponent<WeaponManager>();
        }

        void Update()
        {
            if (PlayerController.localPlayer)
            {
                RaycastHit mira;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out mira, currentWeapon.range))
            {
                tag = mira.collider.name;
            }

                //currentWeapon = weaponManager.GetCurrentWeapon();
            
                if (currentWeapon.fireRate <= 0f)
                {
                    if (Input.GetButtonDown("Fire1"))
                    {
                        Shoot();
                    }
                }
                else
                {
                    if (Input.GetButtonDown("Fire1"))
                    {
                        InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);
                    }
                    else if (Input.GetButtonUp("Fire1"))
                    {
                        CancelInvoke("Shoot");
                    }
                }

            }
        }



        [Client]
        void Shoot()
        {
            Debug.Log("SHOOT");
            
            
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask))
            {
                playershoter = _hit.collider.GetComponent<PlayerShoot>();
                //FindObjectOfType<AudioManager>().PlayInPosition("Gun Shoot", currentWeapon.transform.position);
                if (playershoter != null)
                {                  
                    CmdPlayerShot(_hit.collider.name, currentWeapon.damage, this.gameObject,true);

                    
                }
                else
                {
                    Debug.Log("piso piso");
                    CmdBulletHole(this.gameObject);
                }
            }


           
            Transform t_spawn = transform.Find("Camera");

    }

        [Command]
        void CmdPlayerShot(string _playerID, int _Damage, GameObject gameObject, bool leDio)
        {

            RpcBulletHoles(gameObject, leDio);
            Debug.Log(_playerID + "has been shot");
            Player _player = NetworkRoomManagerExt.GetPlayer(_playerID);
            _player.RpcTakeDamage(_Damage,_player.jugador, gameObject);


            //  _player.RpcTakeDamage(_Damage);
        }

        [ClientRpc]
        void RpcBulletHoles(GameObject ObjectplayerDisparo, bool LeDio)
        {
            PlayerShoot playerDisparo = ObjectplayerDisparo.GetComponent<PlayerShoot>();
            RaycastHit _hitDisparo;
            Physics.Raycast(playerDisparo.cam.transform.position, playerDisparo.cam.transform.forward, out _hitDisparo, playerDisparo.currentWeapon.range, mask);
            if (LeDio)
            {
                GameObject t_newHole = Instantiate(bulletHolePrefab, _hitDisparo.point + (_hitDisparo.normal * 0.1f), Quaternion.LookRotation(_hitDisparo.normal), NetworkIdentity.spawned[_hitDisparo.collider.GetComponent<NetworkIdentity>().netId].transform) as GameObject;
                t_newHole.transform.LookAt(_hitDisparo.point + _hitDisparo.normal);
                Destroy(t_newHole, 2f);
            }
            else
            {
                GameObject t_newHole = Instantiate(bulletHolePrefab, _hitDisparo.point + _hitDisparo.normal * 0.1f, Quaternion.LookRotation(_hitDisparo.normal)) as GameObject;
                t_newHole.transform.LookAt(_hitDisparo.point + _hitDisparo.normal);
                Destroy(t_newHole, 5f);
            }
        }
        [Command]
        void CmdBulletHole(GameObject playerDisparo)
            {
            RpcBulletHoles(playerDisparo,false);
        }
    }
    

}