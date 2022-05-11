using UnityEngine;

namespace Enemy.EnemyStateMachine
{
    public class DeadState : State
    {

        public DeadState(EnemyAI ai) : base(ai)
        {
        }

        public override void InitializeState()
        {
            EnemyAI._canReceiveDamage = false;
            EnemyAI.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            EnemyAI._masksController.MainMaskFinale();
        }

    }
}