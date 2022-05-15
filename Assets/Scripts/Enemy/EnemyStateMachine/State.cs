namespace Enemy.EnemyStateMachine
{
    public abstract class State
    {
        protected EnemyAI EnemyAI;
        protected State(EnemyAI ai)
        {
            EnemyAI = ai;
        }

        public virtual void InitializeState()
        {
            
        }

        public virtual void RunState()
        {
            
        }

        public virtual void InteractWithCellPhone()
        {
            
        }

    }

}