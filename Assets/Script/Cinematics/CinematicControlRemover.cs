using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;


namespace RPG.Cinematics{
    public class CinematicControlRemover : MonoBehaviour
    {   
        GameObject player;
        private void Start() {
            player = GameObject.FindWithTag("Player");
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        void EnableControl(PlayableDirector pd) {
            print("Enable Control");
            player.GetComponent<PlayerController>().enabled = true;
        }
        
        void DisableControl(PlayableDirector pd) {
            print("Disable Control");
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }
    }
}