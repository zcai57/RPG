using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat {
    
    public class WeaponPickup : MonoBehaviour {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 5;

        private void OnTriggerEnter (Collider collider) {
            print("hit");
            if(collider.gameObject.tag == "Player") {
                print("player pick up");
            
            collider.GetComponent<Fighter>().EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(respawnTime));
            }
        }
        
        private IEnumerator HideForSeconds(float seconds) {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            foreach(Transform child in transform) { // cant disable parent because coroutine will not run if disabled
                child.gameObject.SetActive(shouldShow); 
            }
        }

    }
}
