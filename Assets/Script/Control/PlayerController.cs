using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using UnityEngine.AI;
using RPG.Attributes;

namespace RPG.Core {
    public class PlayerController : MonoBehaviour
    {
        Health health;
        private void Start() {
            health = GetComponent<Health>();

        }
        private void Update()
        {   
            if(health.isDead()) return;
            if(InteractWithCombat()) return;
            if(InteractWithMovement()) return;
            // print("nothing to do");
        }

        private bool InteractWithCombat() {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach(RaycastHit hit in hits) {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if(target == null ) continue;
                GameObject targetGameObject = target.gameObject;

                if(!GetComponent<Fighter>().CanAttack(targetGameObject)) {
                    continue;
                }

                if (Input.GetMouseButtonDown(0)) {
                    GetComponent<Fighter>().Attack(targetGameObject);
                }
                return true;
            } 
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if(Input.GetMouseButton(0)) {
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
