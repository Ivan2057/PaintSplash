using UnityEngine;
using Assets.Scripts.GameScripts;

namespace Assets.Scripts.PlayerScripts
{
    public class PlayerSpawnPoint1 : MonoBehaviour
    {
        private void Awake()
        {
            PlayerSpawnSystem.AddSpawnPoint1(transform);
        }
        private void OnDestroy() => PlayerSpawnSystem.RemoveSpawnPoint1(transform);
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 1f);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
        } 
    }
}
