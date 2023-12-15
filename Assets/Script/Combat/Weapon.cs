using UnityEngine;
using RPG.Core;
using RPG.Attributes;

namespace RPG.Combat {
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController weaponOverride = null;
        [SerializeField] GameObject equipedPrefab = null;
        [SerializeField] float weaponDamage = 10f;
        [SerializeField] float weaponRange = 3f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "weapon";

        public void Spawn (Transform RighthandTransform, Transform LefthandTransform, Animator animator) {

            DestroyOldWeapon(RighthandTransform, LefthandTransform);
            
            if(equipedPrefab != null) {
                Transform handTranform = GetHandTransform(RighthandTransform, LefthandTransform);
                GameObject weapon = Instantiate(equipedPrefab, handTranform);
                weapon.name = weaponName;
            }
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (weaponOverride != null) {
                animator.runtimeAnimatorController = weaponOverride;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController; // get the parent of override animator controller
            }
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand) {
            Transform oldWeapon = rightHand.Find(weaponName);
            if(oldWeapon == null) {
                oldWeapon = leftHand.Find(weaponName);
            }
            if(oldWeapon == null) return;

            oldWeapon.name = "Destroying";

            Destroy(oldWeapon.gameObject);
        }
        
        private Transform GetHandTransform (Transform RighthandTransform, Transform LefthandTransform) {
            if(isRightHanded) return RighthandTransform;
            else return LefthandTransform;
        }


        public bool hasProjectile () {
            return projectile != null;
        }

        public void LaunchProjectile (Transform leftHand, Transform rightHand, Health target) {
            Projectile projectileInstance = Instantiate(projectile, GetHandTransform(leftHand, rightHand).position, Quaternion.identity);
            projectileInstance.setTarget(target, weaponDamage);
        }


        public float getRange() {
            return weaponRange;
        }
        
        public float getDamage() {
            return weaponDamage;
        }
    }
}
