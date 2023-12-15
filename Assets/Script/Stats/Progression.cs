using RPG.Attributes;
using UnityEngine;

namespace RPG.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]

    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        public float GetHealth(CharacterClass characterClass, int level) {
            foreach(ProgressionCharacterClass progressionClass in characterClasses) {
                if(characterClass == progressionClass.GetCharacterClass()) {
                    return progressionClass.GetHealth()[level-1];
                }
            }
            return 20;
        }

        [System.Serializable]
        public class ProgressionCharacterClass
        {
            [SerializeField] CharacterClass characterClass;
            [SerializeField] float[] Health;
            
            public CharacterClass GetCharacterClass() {
                return characterClass;
            }

            public float[] GetHealth() {
                return Health;
            }
        }
    }
}

