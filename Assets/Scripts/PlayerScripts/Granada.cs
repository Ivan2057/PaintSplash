using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PlayerScripts
{
    public class Granada : MonoBehaviour
    {
        public GameObject tirador;
        [SerializeField]
        private GameObject efecto;
        private float cuentaregresiva = 3;
        void Update()
        {
            cuentaregresiva -= Time.deltaTime;
            if (cuentaregresiva <= 0)
            {
                Explode();
            }
        }
        public void Explode()
        {
            Collider[] collidersHealth = Physics.OverlapSphere(transform.position, 6);

            foreach (Collider nearbyObject in collidersHealth)
            {
                Debug.Log("Exploto");
                if (nearbyObject.tag == "Player")
                {
                    Debug.Log("Found enemy or a player");
                    Debug.Log("Collided with " + nearbyObject.name);

                    RaycastHit dmg;
                    if (Physics.Linecast(transform.position, nearbyObject.transform.position, out dmg, 5))
                    {
                        Player jugador = dmg.collider.GetComponent<Player>();

                        if (dmg.collider == nearbyObject)
                        {
                            Debug.Log("No Obstructions");
                            if (dmg.rigidbody)
                                dmg.rigidbody.AddExplosionForce(300, transform.position, 4, 50f);
                            if (jugador != null)
                            {
                                jugador.RpcTakeDamage(50, nearbyObject.gameObject, tirador);
                                Debug.Log("enemy hit");
                            }
                        }
                    }
                }
                
            }
            Instantiate(efecto, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
