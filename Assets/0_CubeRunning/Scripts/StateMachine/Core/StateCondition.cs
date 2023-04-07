using StateMachine.ScriptableObjects;

namespace StateMachine
{
    /// <summary>
    /// Class that represents a conditional statement.
    /// </summary>
    public abstract class Condition : IStateComponent
    {
        private bool isCached = false;
        private bool cachedStatement = default;
        internal StateConditionSO _originSO;
        protected StateConditionSO OriginSO => _originSO;

        /// <summary>
        /// Specify the statement to evaluate.
        /// </summary>
        /// <returns></returns>
        protected abstract bool Statement();

        /// <summary>
        /// Wrap the <see cref="Statement"/> so it can be cached.
        /// </summary>
        internal bool GetStatement()
        {
            if (!isCached)
            {
                isCached = true;
                cachedStatement = Statement();
            }

            return cachedStatement;
        }

        internal void ClearStatementCache()
        {
            isCached = false;
        }

        /// <summary>
        /// Awake is called when creating a new instance. Use this method to cache the components needed for the condition.
        /// </summary>
        /// <param name="stateMachine">The <see cref="StateMachine"/> this instance belongs to.</param>
        public virtual void Awake(StateMachine stateMachine)
        {
        }

        public virtual void OnStateEnter()
        {
        }

        public virtual void OnStateExit()
        {
        }
    }

    /// <summary>
    /// Struct containing a Condition and its expected result.
    /// </summary>
    public readonly struct StateCondition
    {
        internal readonly StateMachine stateMachine;
        internal readonly Condition condition;
        internal readonly bool expectedResult;

        public StateCondition(StateMachine stateMachine, Condition condition, bool expectedResult)
        {
            this.stateMachine = stateMachine;
            this.condition = condition;
            this.expectedResult = expectedResult;
        }

        public bool IsMet()
        {
            bool statement = condition.GetStatement();
            bool isMet = statement == expectedResult;

#if UNITY_EDITOR
            stateMachine._debugger.TransitionConditionResult(condition._originSO.name, statement, isMet);
#endif
            return isMet;
        }
    }
}