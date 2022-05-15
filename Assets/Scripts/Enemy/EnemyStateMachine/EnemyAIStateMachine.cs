using UnityEngine;

namespace Enemy.EnemyStateMachine
{
    public abstract class EnemyAIStateMachine : MonoBehaviour
    {
        internal State CurrentAIState; 
        
        public void SetState(State pState)
        {
            if(CurrentAIState == pState)
                return;

            CurrentAIState = pState;
            CurrentAIState.InitializeState();
        }

    }

}