using UnityEngine;
namespace Enemy.EnemyStateMachine
{
    public class EscapingState : State {

        public EscapingState(EnemyAI ai) : base(ai)
        {
        }

        public override void InitializeState()
        {
            EnemyAI.NotifyStateChange(EnemyState.Escaping);
            
            EnemyAI._canReceiveDamage = false;
            EnemyAI._currentHealth = 100;
            EnemyAI._masksController.DestroyMask();
            
            AudioManager.Instance.Play("Escape");
            
            Escape();
        }

        public override void RunState()
        {
            if (EnemyAI._enemyMovementController.HasArrivedToDestination() || EnemyAI._canEscape)
            {
                SpawnManager.Instance.SendEnemyToRandomSpawnLocation();
                EnemyAI.ResetEnemyStats();
                EnemyAI.SetState(new HidingState(EnemyAI));
            }
        }
        
        private void Escape()
        {
            Vector3 escapeRoute = SpawnManager.Instance.GetEscapeRoute().position;
            EnemyAI._enemyMovementController.SetEnemyDestination(escapeRoute);
        }
    }

}