using UnityEngine;
using UnityEngine.AI;
using RPG.Movement;
using RPG.Core;
using System;
using RPG.Saving;
using RPG.Attributes;
// using RPG.Control;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {   
        // [SerializeField] float weaponRange = 2.5f;
        [SerializeField] float timeBetweenAttacks = 1f;
        // [SerializeField] float weaponDamage = 5f;
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] Transform RighthandTransform = null; 
        [SerializeField] Transform LefthandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;

        
        Weapon currentWeapon = null;

 
        Health target;
        float timeSinceLastAttack = Mathf.Infinity;

        private void Start() {
            if(currentWeapon == null){
                EquipWeapon(defaultWeapon);
            }
        }

        private void Update()
        {   
            timeSinceLastAttack += Time.deltaTime;
            if(target == null) return;
            if(target.isDead()) return;

            if (!GetInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                AttackBehavior();
            }
        }

        public void EquipWeapon(Weapon weapon)
        {      
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            // animator.runtimeAnimatorController = weaponOverride;
            weapon.Spawn(RighthandTransform, LefthandTransform, animator);
        }

        public Health GetTarget() {
            return target;
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if(timeSinceLastAttack > timeBetweenAttacks)
            {
                TriggerAttack();
                this.GetComponent<NavMeshAgent>().enabled = false;
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("CancelAttack");
            GetComponent<Animator>().SetTrigger("Attack!");
        }

        private bool GetInRange()
        {   
            if(currentWeapon == null) return false;
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.getRange();
        }

        public bool CanAttack(GameObject combatTarget) {
            if(combatTarget == null) return false;

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.isDead();
        }
        public void Attack(GameObject combatTarget) {
            this.GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel() {
            this.GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<Animator>().ResetTrigger("Attack!");
            GetComponent<Animator>().SetTrigger("CancelAttack");
            target = null;
            GetComponent<Mover>().Cancel();
        }

        //animation event
        void Hit() {
            if(target == null) return;
            if(currentWeapon == null) return;
            if(currentWeapon.hasProjectile()) {
                currentWeapon.LaunchProjectile(RighthandTransform, LefthandTransform, target);
            } else {
                target.TakeDamage(currentWeapon.getDamage());
            }
        }

        void Shoot() {
            Hit();
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            if (gameObject.tag == "Player")
            {
                print(weaponName);
            }
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            if(gameObject.tag == "Player"){
                print("loading weapon");
            }
            EquipWeapon(weapon);
            if(gameObject.tag == "Player") {
                print(currentWeapon.name);
            }
        }
    }
}
