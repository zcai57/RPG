using System;
using UnityEngine;


namespace RPG.Attributes {
    public class HealthBar : MonoBehaviour
    {   
        [SerializeField] Health health = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas canvas = null;
        // Update is called once per frame
        void Update()
        {   
            var remainHealth = health.healthPercentage() / 100;
            if(Mathf.Approximately(remainHealth, 0) || Mathf.Approximately(remainHealth, 1)) {
                canvas.enabled = false;
                return;
            } 
            canvas.enabled = true;
            foreground.localScale = new Vector3(remainHealth , 1, 1);
        }
    }
}

