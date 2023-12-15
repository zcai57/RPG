using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEditor.Build;
using UnityEngine;

namespace RPG.Combat.dropLoot {
    public class BossLoot : MonoBehaviour
    {
        [SerializeField] GameObject loot = null;
        Boolean lootDropped = false;
        [SerializeField] Transform lootDropPoint = null;

        private void Update() {
            if(transform.GetComponent<Health>().isDead() && !lootDropped) {
                dropLoot();
                lootDropped = true;
            }
        }

        private void dropLoot()
        {   
            if(loot != null) {
                print("dropped loot");
                Instantiate(loot, lootDropPoint);
            }
        }
    }
}
