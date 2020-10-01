using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.PlayerScripts
{
    public class AnimacionGun : MonoBehaviour
    {
        // Start is called before the first frame update

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
        // Update is called once per frame
        void Update()
        {
            if (PlayerController.localPlayer)
            {
                if(Input.GetButtonDown("Fire"))
                {
                    anim.SetBool("Disparar", true);
                }else if(Input.GetButtonUp("Fire"))
                {
                    anim.SetBool("Disparar", false);
                }
            }
        }
    }
}
