using UnityEngine;

namespace Enemy.EnemyStateMachine
{
    public class MaterializedState : State
    {

        public MaterializedState(EnemyAI ai) : base(ai)
        {
        }

        public override void InitializeState()
        {
            EnemyAI.NotifyStateChange(EnemyState.Materialized);
            
            EnemyAI._enemyModel.SetActive(true);
            EnemyAI._canReceiveDamage = true;
            Vector3 seletedPosition = EnemyAI.Action_SelectRandomPositionFarAwayFromPlayerInsideMovementArea();
            EnemyAI._enemyMovementController.SetEnemyDestination(seletedPosition);
            AudioManager.Instance.Play("monsterMaterialize");


        }

        public override void RunState()
        {
            EnemyAI._onMaterializedTimer += Time.deltaTime;

            if (EnemyAI._enemyMovementController.HasArrivedToDestination())
            {
                Vector3 seletedPosition = Vector3.zero;

                if (EnemyAI._onMaterializedTimer >= EnemyAI._changeRoomThreshold)
                {
                    EnemyAI._onMaterializedTimer = 0;
                    seletedPosition = Action_ChangeRoom();
                }
                else
                {
                    seletedPosition = EnemyAI.Action_SelectRandomPositionFarAwayFromPlayerInsideMovementArea();

                }

                EnemyAI._enemyMovementController.SetEnemyDestination(seletedPosition);

            }

            if (EnemyAI._currentHealth <= EnemyAI._escapeThresholds[EnemyAI._escapeThresholdindex])
            {
                EnemyAI._escapeThresholdindex++;
                EnemyAI.SetState(new EscapingState(EnemyAI));

            }
        }
        private Vector3 Action_ChangeRoom()
        {
            return SpawnManager.Instance.GetRandomRoom();
        }
    }

}