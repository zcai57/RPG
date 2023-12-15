using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat {
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Health health;

        // private void Awake() {
        //     health = GameObject.FindWithTag("Player").GetComponent<Fighter>().GetTarget();
        // }

        public void Update () {
            health = GameObject.FindWithTag("Player").GetComponent<Fighter>().GetTarget();
            if (health == null) {
                GetComponent<Text>().text = "N/A";
            } else {
                GetComponent<Text>().text = string.Format("{0:0.0}%", health.healthPercentage());
            }
        }

    }
}