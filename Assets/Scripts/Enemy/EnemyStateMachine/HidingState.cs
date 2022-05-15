using UnityEngine;

namespace Enemy.EnemyStateMachine
{
    public class HidingState : State
    {
        public HidingState(EnemyAI ai) : base(ai)
        {
        }

        public override void InitializeState()
        {
            EnemyAI._enemyModel.SetActive(false);
            EnemyAI._canReceiveDamage = false;
            EnemyAI.NotifyStateChange(EnemyState.Hiding);
        }

        public override void RunState()
        {
            EnemyAI._onHideTimer += Time.deltaTime;
            
            if (IsTimeToChangeHidingLocation())
            {
                Vector3 selectedPosition = Action_SelectRandomPositionInsideMovementArea();
                EnemyAI._enemyMovementController.SetEnemyDestination(selectedPosition);
                EnemyAI._onHideTimer = 0;
            }
            
            if (EnemyAI._canScare)
            {
                EnemyAI.SetState(new ScaredState(EnemyAI));
            }
        }

        public override void InteractWithCellPhone()
        {
            EnemyAI.IncreaseCellPhoneFocusByFactor(8f);
            if (EnemyAI._currentScareLevel >= EnemyAI._scareThreshold)
                EnemyAI._canScare = true;
        }

        private bool IsTimeToChangeHidingLocation()
        {
            return EnemyAI._onHideTimer >= EnemyAI._onHidingPositionChangeTimer;
        }
        private Vector3 Action_SelectRandomPositionInsideMovementArea()
        {
            Vector3 position = Helpers.ChooseRandomPositionInsideCollider(EnemyAI._movementArea);
            return position;
        }

    }


}