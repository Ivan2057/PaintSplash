using Assets.Scripts.GameScripts;
using Assets.Scripts.WeaponScripts;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.PlayerScripts
{
    public class Player : NetworkBehaviour
    {
        #region Declarations

        [Header("General Params")]
        //player 
        [SerializeField]
        public GameObject jugador;
        [SerializeField]
        public GameObject pistola;
        [SerializeField]
        private GameObject granada;
        public GameObject ArmaTocada;
        public bool GuninHand;
        public static NetworkConnection userConnection;
        private Rigidbody rigidBody;
        private int BanderaTeam;
        [SerializeField] Canvas vidacanvas;
        [SerializeField] GameObject menuPanel;
        [SerializeField] Canvas menucanvas;
        [SerializeField] GameObject muertePanel;
        [SerializeField] Canvas muerteCanvas;
        [SerializeField] Camera miCamera;
        public static List<Transform> spawnPoints = new List<Transform>();

        [SyncVar]
        private bool _isDead = false;


        public bool isDead
        {
            get { return _isDead; }
            protected set { _isDead = value; }
        }
        [SerializeField]
        private int maxHealth = 100;

        [SyncVar(hook = nameof(ShowVidaCanvas))]
        public int currentHealth;

        public static int vida;
        public int cantGranadas;

        [SerializeField]
        private Behaviour[] disableOnDeath;
        private bool[] wasEnabled;

        [SerializeField] public TextMeshProUGUI hp;

        //weapons
        [SerializeField] public List<Weapon> weapons;
        [SerializeField] public List<GameObject> ObjectWeapons;
        [SerializeField] public List<GameObject> OrigenWeapons;
        [SerializeField] public int currentWeapon = 0;
        [SerializeField] int lastWeapon = 0;
        [SerializeField] public List<GameObject> weaponsHand;
        [SerializeField] public float forwardDropOffset;
        [SerializeField] public float upDropOffset;

        private GameModeCTF gameModeCTF;

        public int ScoreTeam0 = 0;
        public int ScoreTeam1 = 0;

        public static Transform posInicial;

        [SerializeField]
        public int Team;
        [SerializeField]
        private EndGame canvasFin;
        private TextMeshProUGUI time1Puntos;
        private TextMeshProUGUI time2Puntos;


        private Collider lastCollision;



        public int muertes = 0;
        public int kills = 0;
        public int points = 0;
        #endregion Declarations

        #region setup
        private void Start()
        {
            if (GetComponent<NetworkIdentity>().hasAuthority)
            {
                vidacanvas.enabled = true;
                gameModeCTF = FindObjectOfType<GameModeCTF>();
                //CmdSetTeam(jugador, userConnection.connectionId);
                if (Team == 0)
                {
                    spawnPoints = PlayerSpawnSystem.spawnPoints;
                }
                else
                {

                    spawnPoints = PlayerSpawnSystem.spawnPoints1;
                }
                canvasFin = GetComponent<EndGame>();
                GetGameObjectsFromMap();

            }
            else
            {
                vidacanvas.targetDisplay = 2;
                menucanvas.targetDisplay = 2;
                muerteCanvas.targetDisplay = 2;
                vidacanvas.enabled = false;
            }
            vidacanvas.sortingOrder = 1;
            vidacanvas.sortingOrder = 0;
            Setup();
            GameMode.LoadPlayer(this);
        }

        public void Setup()
        {
            rigidBody = GetComponent<Rigidbody>();
            if (rigidBody == null)
            {
                Debug.LogError("Player Rigidbody not found");

            }
            wasEnabled = new bool[disableOnDeath.Length];
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }

            SetDefaults();
        }

        public void SetDefaults()
        {
            GuninHand = true;
            isDead = false;
            this.gameObject.GetComponent<PlayerCameraController>().vivo = true;
            currentHealth = maxHealth;
            //jugador.AddComponent<Material>


            for (int i = 0; i < disableOnDeath.Length; i++)
            {
                disableOnDeath[i].enabled = wasEnabled[i];
            }

            Collider _col = GetComponent<Collider>();
            if (_col != null)
            {
                _col.enabled = true;
            }
            else
            {
                Debug.LogError("No se encontró un Collider en el personaje");
            }
        }

        private void GetGameObjectsFromMap()
        {
            try
            {
                gameModeCTF = FindObjectOfType<GameModeCTF>();
            }
            catch (Exception e)
            {
                Debug.LogError("No se pudo encontrar el objeto GameModeCTF : " + e.Message);
            }

            try
            {
                time1Puntos = GameObject.Find("Time1Puntos").GetComponent<TextMeshProUGUI>();
                time2Puntos = GameObject.Find("Time2Puntos").GetComponent<TextMeshProUGUI>();
            }
            catch (Exception e)
            {
                Debug.LogError("No se pudo encontrar el HUD PUNTOS en la scena : " + e.Message);
            }
        }

        [Command]
        private void CmdSetTeam(GameObject player, int conn)
        {
            RpcSetTeam(player, conn);

        }

        [ClientRpc]
        private void RpcSetTeam(GameObject player, int conn)
        {
            player.GetComponent<Player>().Team = TeamManager.ReturnTeam(conn);
        }

        #endregion setup

        #region Base Functions (Update / OnTriggerEnter)

        [Client]
        private void Update()
        {
            this.hp.text = this.currentHealth.ToString();
            if (Input.GetKeyDown(KeyCode.X))
            {

                    DropWeapon(currentWeapon);
                
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (cantGranadas > 0)
                {
                    throwGranade(this.gameObject);
                }
            }


        }



        [Client]
        private void OnTriggerEnter(Collider objectCollider)
        {
            lastCollision = objectCollider;
            PlayerWeapon armaTocada = objectCollider.GetComponent<PlayerWeapon>();
            
            if (GetComponent<NetworkIdentity>().hasAuthority)
            {

                CaptureZone captureZone = objectCollider.GetComponent<CaptureZone>();
                if (captureZone == null || Team != captureZone.teamID)   //si no fue el capturezone
                {
                    if (armaTocada != null)
                    {
                        if (weaponsHand.Count < 2)
                        {
                            ArmaTocada = objectCollider.gameObject;
                            //  AgarrarArma(this.gameObject);
                            weaponsHand.Add(OrigenWeapons[ArmaTocada.GetComponent<PlayerWeapon>().id]);
                            weaponsHand[0].SetActive(true);
                            if (currentWeapon != 1)
                            {
                                currentWeapon = ArmaTocada.GetComponent<PlayerWeapon>().id;
                            }
                            ArmaTocada.SetActive(false);
                            GuninHand = true;

                        }
                    }
                    return;
                }
                else //
                {
                    if (lastCollision == objectCollider)
                    {
                        return;
                    }
                    else
                    {
                        lastCollision = objectCollider;
                        if (IsHoldingFlag())
                        {
                            CmdReturnWeapon(1, jugador);
                            CmdUpdateScore(Team);
                        }
                    }
                }
            }
            if (armaTocada != null)
            {
                if (weaponsHand.Count < 2)
                {
                    ArmaTocada = objectCollider.gameObject;
                    //  AgarrarArma(this.gameObject);
                    weaponsHand.Add(OrigenWeapons[ArmaTocada.GetComponent<PlayerWeapon>().id]);
                    weaponsHand[0].SetActive(true);
                    if (currentWeapon != 1)
                    {
                        currentWeapon = ArmaTocada.GetComponent<PlayerWeapon>().id;
                    }
                    ArmaTocada.SetActive(false);
                    GuninHand = true;

                }
            }
        }

        #endregion Base Functions

        #region Shoot/Damage/Life/Respawn
        public void throwGranade(GameObject jugadorTirador)
        {
            CmdthrowGranade(jugadorTirador);
        }
        [Command]
        public void CmdthrowGranade(GameObject jugadorTirador)
        {
            RpcthrowGranade(jugadorTirador);
        }
        [ClientRpc]
        public void RpcthrowGranade(GameObject jugadorTirador)
        {
            Player jugador = jugadorTirador.GetComponent<Player>();
            jugador.cantGranadas--;
            GameObject granade = Instantiate(granada,
                   jugadorTirador.transform.position + jugadorTirador.transform.forward + Vector3.up * 1.5f,
                   jugadorTirador.transform.rotation);
            granade.transform.Rotate(15, 0, 40);
            granade.GetComponent<Rigidbody>().AddForce(transform.forward * 6, ForceMode.Impulse);
            granade.GetComponent<Granada>().tirador = this.gameObject;
        }
        public void AgarrarArma(GameObject jugadorAgarrador)
        {
            CmdAgarrarArma(jugadorAgarrador);
        }

        [Command]
        public void CmdAgarrarArma(GameObject jugadorAgarrador)
        {
            RpcAgarrarArma(jugadorAgarrador);
        }

        [ClientRpc]
        public void RpcAgarrarArma(GameObject jugadorAgarrador)
        {
            jugadorAgarrador.GetComponent<Player>().weaponsHand.Add(OrigenWeapons[jugadorAgarrador.GetComponent<Player>().ArmaTocada.GetComponent<PlayerWeapon>().id]);
            jugadorAgarrador.GetComponent<Player>().weaponsHand[0].SetActive(true);
            jugadorAgarrador.GetComponent<Player>().currentWeapon = jugadorAgarrador.GetComponent<Player>().ArmaTocada.GetComponent<PlayerWeapon>().id;
            jugadorAgarrador.GetComponent<Player>().ArmaTocada.SetActive(false);
            jugadorAgarrador.GetComponent<Player>().GuninHand = true;
        }

        public static int DevolverVida()
        {
            return vida;
        }

        public string GetHealth()
        {
            return currentHealth.ToString();
        }
        public void AddKill()
        {
            this.kills++;
            GameMode.UpdatePlayer(this);
        }

        [ClientRpc]
        public void RpcUpdateScoreboard()
        {
            GameMode.LoadPlayer(this);
        }

        [ClientRpc]
        public void RpcTakeDamage(int _amount, GameObject jugadorPeticion, GameObject asesino)
        {


            if (isDead)
            {
                return;
            }
            ChangeLife(_amount);

            Debug.Log(transform.name + " now has " + currentHealth + " health.");
            if (currentHealth <= 0)
            {
                asesino.GetComponent<Player>().AddKill();
                jugadorPeticion.GetComponent<Player>().Die();
            }

        }

        [Client]
        public void ChangeLife(int _ammount)
        {
            currentHealth = currentHealth - _ammount;
            hp.text = currentHealth.ToString();
        }

        public void Die()
        {
            isDead = true;
            this.gameObject.GetComponent<PlayerCameraController>().vivo = false;
            vidacanvas.enabled = false;
            muertePanel.SetActive(true);
            for (int i = 0; i < disableOnDeath.Length; i++)
            {
                disableOnDeath[i].enabled = false;
            }
            if (weapons[currentWeapon].isWeaponDropable)
            {
                CmdReturnWeapon(1, jugador);
            }
            Collider _col = GetComponent<Collider>();
            if (_col == null)
            {
                Debug.LogError("No se encontro un collider");
            }

            Debug.Log(transform.name + " esta muerto!");

            StartCoroutine(Respawn());

            muertes++;
        }

        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(3f);

            muertePanel.SetActive(false);
            vidacanvas.enabled = true;
            cantGranadas = 2;

            int i = UnityEngine.Random.Range(0, spawnPoints.Count);
            transform.position = spawnPoints[i].position;
            transform.rotation = spawnPoints[i].rotation;

            // transform.rotation = posInicial.rotation;
            SetDefaults();
            Debug.Log(transform.name + " respawned.");
        }


        public static void SetConnection(NetworkConnection conn)
        {
            userConnection = conn;
        }

        public static NetworkConnection ReturnConnection()
        {
            return userConnection;
        }

        #endregion Shoot/Damage/Life/Respawn

        #region Drop/Switch Weapon or Flag
        /// <summary>
        /// Pick up a weapon/flag
        /// </summary>
        /// <param name="weaponObject"></param>
        /// <param name="originalLocation"></param>
        /// <param name="teamID"></param>
        /// <param name="weaponID"></param>
        /// <param name="overrideLock"></param>
        [Client]
        public void PickUpWeapon(GameObject weaponObject, Vector3 originalLocation, int teamID, int weaponID, bool overrideLock = false)
        {
            SwitchWeapon(weaponID, overrideLock);

            weapons[weaponID].SetWeaponGameObject(teamID, weaponObject, originalLocation);

        }
        /// <summary>
        /// Switch weapons
        /// </summary>
        /// <param name="weaponID"></param>
        /// <param name="overrideLock"></param>
        [Client]
        public void SwitchWeapon(int weaponID, bool overrideLock = false)
        {

            if (!overrideLock && weapons[currentWeapon].isWeaponLocked == true)
            {
                return;
            }

            lastWeapon = currentWeapon;
            currentWeapon = weaponID;

            foreach (Weapon weapon in weapons)
            {
                weapon.gameObject.SetActive(false);
            }

            weapons[currentWeapon].gameObject.SetActive(true);

        }
        /// <summary>
        /// Player able to drop his weapon/flag.
        /// </summary>
        /// <param name="weaponID"></param>
        [Client]
        public void DropWeapon(int weaponID)
        {
            CmdDropWeapon(weaponID, jugador);

        }

        [Command]
        public void CmdDropWeapon(int weaponID, GameObject jugadorPeticion)
        {
            RpcDropWeapon(weaponID, jugadorPeticion);


        }
        [ClientRpc]
        public void RpcDropWeapon(int weaponID, GameObject jugadorPeticion)
        {

            Player miPlayerPeticion = jugadorPeticion.GetComponent<Player>();
            if (!miPlayerPeticion.weapons[weaponID].isWeaponDropable) return;
            Vector3 forward = miPlayerPeticion.transform.forward; ;
            forward.y = 0;
            forward *= miPlayerPeticion.forwardDropOffset;
            forward.y = miPlayerPeticion.upDropOffset;
            Vector3 dropLocation = miPlayerPeticion.transform.position + forward;
            if (!miPlayerPeticion.weapons[weaponID].isWeaponShootable)
            {
                miPlayerPeticion.weapons[weaponID].DropWeapon(miPlayerPeticion.rigidBody, dropLocation);
                miPlayerPeticion.weapons[weaponID].worldWeaponGameObject.SetActive(true);


                miPlayerPeticion.SwitchWeapon(miPlayerPeticion.lastWeapon, true);
            }
            else
            {
                if (GuninHand)
                {
                    miPlayerPeticion.weapons[weaponID].DropGun(miPlayerPeticion.gameObject, miPlayerPeticion.ObjectWeapons[weaponID]);
                    
                    for (int i = 0; i < weaponsHand.Count; i++)
                    {
                        if (weaponsHand[i].GetComponent<PlayerWeapon>().id == currentWeapon) { weaponsHand.RemoveAt(i); }
                    }
                    if (weaponsHand.Count == 0)
                    {
                        GuninHand = false;
                    }
                    else
                    {
                        weaponsHand[0].SetActive(true);
                    }
                }
            }
            //if possible
            Debug.Log("Dropping Weapon/flag");

        }

        [Command]
        public void CmdReturnWeapon(int weaponID, GameObject jugadorPeticion)
        {
            RpcReturnWeapon(weaponID, jugadorPeticion);
        }
        /// <summary>
        /// Switch back to the last weapon equipped
        /// </summary>
        /// <param name="weaponID"></param>
        [ClientRpc]
        public void RpcReturnWeapon(int weaponID, GameObject jugadorPeticion)
        {
            Player playerPeticion = jugadorPeticion.GetComponent<Player>();
            if (playerPeticion.weapons[weaponID].isWeaponDropable)//flag
            {
                Vector3 returnLocation = playerPeticion.weapons[weaponID].originalLocation;

                playerPeticion.weapons[weaponID].worldWeaponGameObject.transform.position = returnLocation;
                playerPeticion.weapons[weaponID].worldWeaponGameObject.SetActive(true);

                playerPeticion.SwitchWeapon(playerPeticion.lastWeapon, true);//if possible
            }
        }

        //bad
        [Client]
        public bool IsHoldingFlag()
        {
            bool isHoldingFlag;
            if (currentWeapon == 1)
            {
                isHoldingFlag = true;
            }
            else
            {
                isHoldingFlag = false;
            }

            return isHoldingFlag;
        }

        [Client]
        public int GetWeaponTeamID()
        {
            //   return weapons[currentWeapon].teamID;
            return 0;
        }



        #endregion Drop/Switch Weapon or Flag

        #region Canvas/Score
        [Command]
        private void CmdUpdateScore(int banderaTeam)
        {


            RpcAddScoreToCanvas(banderaTeam);
        }

        [ClientRpc]
        void RpcAddScoreToCanvas(int banderaTeam)
        {

            Debug.Log("RpcAddScoreCanvas");
            if (banderaTeam == 0)
            {
                gameModeCTF.ScoreTeam0 += 1;
                ScoreTeam0 = gameModeCTF.ScoreTeam0;
                time1Puntos.text = gameModeCTF.ScoreTeam0.ToString();
                Debug.Log("Time 1 puntos: " + gameModeCTF.ScoreTeam0 + " " + ScoreTeam0);
            }
            else
            {
                gameModeCTF.ScoreTeam1 += 1;
                ScoreTeam1 = gameModeCTF.ScoreTeam1;
                time2Puntos.text = gameModeCTF.ScoreTeam1.ToString();
                Debug.Log("Time 1 puntos: " + gameModeCTF.ScoreTeam1 + " " + ScoreTeam1);
            }
            if (gameModeCTF.ScoreTeam0 >= gameModeCTF.Meta)
            {
                canvasFin.FinJuego(0);
                menucanvas.enabled = false;
                vidacanvas.enabled = false;
            }
            else if (gameModeCTF.ScoreTeam1 >= gameModeCTF.Meta)
            {
                menucanvas.enabled = false;
                vidacanvas.enabled = false;
                canvasFin.FinJuego(1);
            }
        }

        void ShowVidaCanvas(int oldValue, int newValue)
        {
            if (GetComponent<NetworkIdentity>().hasAuthority)
            {
                hp.text = newValue.ToString();
                Debug.Log("Hook - Cambiando vida");
            }
        }

        #endregion Canvas/Score
    }
}