using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.WeaponScripts
{
    [System.Serializable]
    public class PlayerWeapon : MonoBehaviour
    {
        public int id = 0;
        public string name = "Glock";
        public int damage = 10;
        public float range = 100f;

        public float fireRate = 0f;

        public GameObject graphics;
    }
}