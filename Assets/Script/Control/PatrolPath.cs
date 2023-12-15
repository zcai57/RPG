using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control {
    public class PatrolPath : MonoBehaviour
    {
        const float waypointGizmoRadius = 0.3f;

        private void OnDrawGizmos() {
            for (int i = 0; i < transform.childCount; i++)
            {   
                int nextIndex = getNextIndex(i);
                Gizmos.DrawSphere(GetWayPoint(i), waypointGizmoRadius);
                Gizmos.DrawLine(GetWayPoint(i), GetWayPoint(nextIndex));
            }
        }

        public int getNextIndex (int i) {
            if(i == transform.childCount - 1){
                return 0;
            }
            return i + 1;
        }

        public Vector3 GetWayPoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
