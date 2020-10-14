using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.NetworkScripts;
using Mirror;
namespace Assets.Scripts.PlayerScripts
{
    public class AnimacionGun : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private GameObject Pistola;
        [SerializeField] private GameObject Jugador;
        Animator anim;
        private void Start()
        {
            if (Jugador.GetComponent<NetworkIdentity>().hasAuthority)
            {
                anim = Pistola.GetComponent<Animator>();
            }
        }
        // Update is called once per frame
        void Update()
        {
            
            
                if (Input.GetButtonDown("Fire1"))
                {
                    anim.SetBool("Disparar", true);
                }else if(Input.GetButtonUp("Fire1"))
                {
                    anim.SetBool("Disparar", false);
                }
            
        }
    }
}
