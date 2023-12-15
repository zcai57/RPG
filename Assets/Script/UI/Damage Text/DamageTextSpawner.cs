using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText {
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] Component DamageTextPrefab = null;
        public void Spawn(float damage)
        {
            if (DamageTextPrefab != null)
            {
                Transform TextObject = DamageTextPrefab.transform.GetChild(0).transform.GetChild(0);
                TextObject.GetComponent<Text>().text = damage.ToString();
                Object.Instantiate(DamageTextPrefab, this.transform);
            }
        }
    }
}