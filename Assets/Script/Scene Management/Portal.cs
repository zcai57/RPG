using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.SceneManagement {
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier{
            A, B, C, D, E
        }
      
        [SerializeField] int SceneToLoad = -1;
        [SerializeField] Transform SpawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] DestinationIdentifier id;
        [SerializeField] float FadeOutTime = 3f;
        [SerializeField] float FadeInTime = 2f;
        [SerializeField] float FadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider other) {
            if(!other.CompareTag("Player")) {
                return;
            }
            StartCoroutine(Transition());
        }

        private IEnumerator Transition () {
            if(SceneToLoad < 0) {
                Debug.LogError("Scene to load not set");
                yield break;
            }
            
            Fader fader = FindObjectOfType<Fader>();


            DontDestroyOnLoad(gameObject);
            yield return fader.FadeOut(FadeOutTime);
            //save
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.Save();

            yield return SceneManager.LoadSceneAsync(SceneToLoad);
            
            // Maybe disable control here for potential bugs
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<PlayerController>().enabled = false;

            //load
            wrapper.Load();

            Portal endPortal = GetPortal();
            UpdatePlayer(endPortal);

            wrapper.Save(); // save again to load correct scene when restart

            yield return new WaitForSeconds(FadeWaitTime);
            yield return fader.FadeIn(FadeInTime);
            // Resume Control
            player.GetComponent<PlayerController>().enabled = true;

            Destroy(gameObject);
        }

        private Portal GetPortal() {
            foreach (Portal portal in FindObjectsOfType<Portal>()) {
                if(portal == this) continue;
                if(this.destination != portal.id) continue;

                return portal;
            }
            return null;
        }

        private void UpdatePlayer(Portal endPortal) {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(endPortal.SpawnPoint.position);
            player.transform.rotation = endPortal.SpawnPoint.rotation;
        }
    }
}
