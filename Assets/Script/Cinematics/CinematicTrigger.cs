using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics {

    public class CinematicTrigger : MonoBehaviour
    {
        bool triggered = false;

        private void OnTriggerEnter(Collider other) {
            if(triggered || !other.CompareTag("Player")) { //only trigger once and must be a player
                return;
            }
            GetComponent<PlayableDirector>().Play();
            triggered = true;
        }
    }
}

