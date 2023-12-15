﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Attributes;

namespace RPG.Combat {
    public class Projectile : MonoBehaviour
    {   
        [SerializeField] float speed = 1;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 10;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 2;

        Health target = null;
        // Update is called once per frame
        float damage = 0;

        void Start() {
            transform.LookAt(GetAimLocation());
        }

        void Update()
        {   
            if (target == null) return;
            if(isHoming && !target.isDead()) {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * Time.deltaTime * speed);    
        }

        //set the target of the projectile
        public void setTarget (Health target, float damage) {
            this.target = target;
            this.damage = damage;

            Destroy(gameObject, maxLifeTime);
        }

        //
        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if(targetCapsule == null) {
                return target.transform.position;
            }

            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other) {
            if(other.GetComponent<Health>() != target) {
                return;
            }
            if(target.isDead()) return;
            target.TakeDamage(damage);
            speed = 0;

            if(hitEffect != null) {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }
            foreach(GameObject toDestroy in destroyOnHit) {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }
    }

}