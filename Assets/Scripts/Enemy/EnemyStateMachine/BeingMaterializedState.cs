using UnityEngine;

namespace Enemy.EnemyStateMachine
{
    public class BeingMaterializedState : State
    {
        public BeingMaterializedState(EnemyAI ai) : base(ai)
        {
        }

        public override void InitializeState()
        {
            EnemyAI._enemyModel.SetActive(true);
            EnemyAI.NotifyStateChange(EnemyState.Materializing);
        }

        public override void RunState()
        {
            EnemyAI._onMaterializingTimer += Time.deltaTime;

            if (EnemyAI._onMaterializingTimer >= EnemyAI._onMaterializingRoutineDuration)
            {
                EnemyAI._onMaterializingTimer = 0;
                EnemyAI.SetState(new MaterializedState(EnemyAI));
            }
        }

    }

}