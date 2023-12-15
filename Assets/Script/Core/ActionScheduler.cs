using UnityEngine;
using RPG.Core;

namespace RPG.Core {
    public class ActionScheduler : MonoBehaviour {
        IAction lastAction;

        public void StartAction(IAction action) {
            if(lastAction == action) {
                return;
            }
            if(lastAction != null) {
                print("stop Action" + lastAction);
                lastAction.Cancel();
            }
            lastAction = action;
        }

        public void CancelCurrentAction() {
            StartAction(null);
        }
    }
}
