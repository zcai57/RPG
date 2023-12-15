using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes {
    public class HealthDisplay : MonoBehaviour
    {
        Health health;

        private void Awake() {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        public void Update () {
            GetComponent<Text>().text = string.Format( "{0:0.0}%", health.healthPercentage());
        }

    }
}