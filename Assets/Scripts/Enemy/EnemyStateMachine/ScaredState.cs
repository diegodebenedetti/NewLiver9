using UnityEngine;

namespace Enemy.EnemyStateMachine
{
    public class ScaredState : State
    {
        public ScaredState(EnemyAI ai) : base(ai)
        {
        }

        public override void InitializeState()
        {
            EnemyAI._canReceiveDamage = false;
            EnemyAI.NotifyStateChange(EnemyState.Scared);
        }

        public override void RunState()
        {
            EnemyAI._onScaredTimer += Time.deltaTime;
            if (EnemyAI._enemyMovementController.HasArrivedToDestination())
            {
                Vector3 selectedPosition = EnemyAI.Action_SelectRandomPositionFarAwayFromPlayerInsideMovementArea();
                EnemyAI._enemyMovementController.SetEnemyDestination(selectedPosition);
                EnemyAI._onScaredTimer = 0;
            }

            if (EnemyAI._canMaterialize)
            {
                EnemyAI.SetState(new BeingMaterializedState(EnemyAI));
            }
        }

        public override void InteractWithCellPhone()
        {
            EnemyAI.IncreaseCellPhoneFocusByFactor(15f);
            if (EnemyAI._currentScareLevel >= EnemyAI._materializeThreshold)
            {
                Mathf.Clamp(EnemyAI._currentScareLevel, 0, 100);
                EnemyAI._readyForMaterialize = true;
            }
        }


    }

}