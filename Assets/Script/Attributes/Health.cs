using UnityEngine;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;
using System;
using RPG.UI.DamageText;
using UnityEngine.Networking;

namespace RPG.Attributes {
    public class Health : MonoBehaviour,  ISaveable{
        [SerializeField] float health = 100f;
        [SerializeField] GameObject damageTextSpawner = null;

        bool death = false;
        private void Start() {
            health = GetComponent<BaseStats>().getHealth();
        }

        public bool isDead()
        {   
            return death;
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health = (float)state;
            if(health == 0) {
                Die();
            } else {
                // GetComponent<Animator>().SetTrigger("Revive!");
            }
        }

        public void TakeDamage(float damage) {
            if(death == true) return; 
            health = Mathf.Max(health - damage, 0);
            if(health == 0)
            {
                Die();
            }
            if(damageTextSpawner != null) {
                damageTextSpawner.GetComponent<DamageTextSpawner>().Spawn(damage);
            }
            // print(health);
        }

        public float healthPercentage () {
            return health / GetComponent<BaseStats>().getHealth() * 100;
        }

        private void Die()
        {
            if (death) return;
            GetComponent<Animator>().SetTrigger("Death!");
            death = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}