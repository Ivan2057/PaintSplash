using Mirror;
using UnityEngine;


namespace Assets.Scripts.PlayerScripts
{
    public class Animacion : NetworkBehaviour
    {
        Animator anim;
        public bool puedoSaltar = false;
        bool cayendo = false;
        Collider pies;
        bool agachado = false;
        [SerializeField] private CharacterController cc;
        [SerializeField] private Collider c;
        private void Start()
        {


            if (PlayerController.localPlayer)
            {
                anim = GetComponent<Animator>();
            }


        }
     
        void Update()
        {
            if (PlayerController.localPlayer)
            {
                //if (PlayerController.isGrounded)
                //{
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        anim.SetBool("Correr_w", true);
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            anim.SetBool("Saltar", true);
                            anim.SetBool("Correr_w", false);
                            anim.SetBool("Correr_s", false);
                        }

                    }
                    else if (Input.GetKeyUp(KeyCode.W))
                    {
                        anim.SetBool("Correr_w", false);
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        anim.SetBool("Correr_s", true);
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            anim.SetBool("Saltar", true);
                            anim.SetBool("Correr_w", false);
                            anim.SetBool("Correr_s", false);
                        }
                    }
                    else if (Input.GetKeyUp(KeyCode.S))
                    {
                        anim.SetBool("Correr_s", false);
                    }
                   /* if (Input.GetKeyDown(KeyCode.Space))
                    {
                        anim.SetBool("Saltar", true);
                        anim.SetBool("Correr_w", false);
                        anim.SetBool("Correr_s", false);
                    }*/

                    if (cayendo)
                    {
                        anim.SetBool("Aterrizar", true);
                        anim.SetBool("Caer", false);
                        cayendo = false;
                    }
                    else
                    {
                        anim.SetBool("Aterrizar", false);
                    }
                    if (Input.GetKeyDown(KeyCode.LeftControl))
                    {
                        anim.SetBool("Agacharse", true);
                    }
                    else if (Input.GetKeyUp(KeyCode.LeftControl))
                    {
                        anim.SetBool("Agacharse", false);
                    }

                /*}
                else   //si no esta en el piso
                {
                    anim.SetBool("Saltar", false);
                    anim.SetBool("Caer", true);
                    cayendo = true;
                }
                */
            }
        }
    }

}

