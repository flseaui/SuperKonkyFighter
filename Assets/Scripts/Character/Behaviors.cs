using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Character
{
    public abstract class Behaviors {
        private IDictionary<int, Action> _actionIds;
        private IDictionary<Action, int> _animAction;

        protected float ForwardSpeed = 0.25f,
            BackwardSpeed = -0.15f,
            DashForwardSpeed = 3f,
            DashBackSpeed = 3f,
            AirDashForwardSpeed = 3f,
            AirDashBackSpeed = 3f,
            JumpDirectionSpeed = 1.25f,
            Gravity = -0.05f;

        protected int MaxHealth = 11000;
        protected int MaxMeter = 8000;
        protected bool InfiniteDashForward = true;
        protected static Action.Rect NullBox = new Action.Rect(0, 0, -100, -100, 0, -1);

        public delegate void OnAdvancedAction(PlayerScript player);
        public OnAdvancedAction[] OnAdvancedActionCallbacks;

        // Returns Action object from given id
        public Action GetAction(int id)
        {
            if (!_actionIds.ContainsKey(id))
                Debug.Log("Invalid Action: " + id);
            return _actionIds[id];
        }

        protected void SetIds(IDictionary<int, Action> actionIds, IDictionary<Action, int> animAction)
        {
            _actionIds = actionIds;
            _animAction = animAction;
        }

        public virtual void SetDelegates() { }

        public virtual void SetStats()
        {
            ForwardSpeed = 0.25f;
            BackwardSpeed = -0.15f;
            JumpDirectionSpeed = 1.25f;
            DashForwardSpeed = 3f;
            DashBackSpeed = 3f;
            AirDashForwardSpeed = 3f;
            AirDashBackSpeed = 3f;
            Gravity = -0.05f;
            MaxHealth = 11000;
            MaxMeter = 8000;
            InfiniteDashForward = true;
        }

        public virtual float GetAttackMovementHorizontal(int attackState)
        {
            return 0;
        }

        public virtual float GetAttackMovementVertical(int attackState)
        {
            return 0;
        }

        public int GetAnimAction(Action id)
        {
            return _animAction[id];
        }

        public float GetForwardSpeed()
        {
            return ForwardSpeed;
        }

        public float GetBackwardSpeed()
        {
            return BackwardSpeed;
        }

        public float GetJumpDirectionSpeed()
        {
            return JumpDirectionSpeed;
        }

        public float GetDashForwardSpeed()
        {
            return DashForwardSpeed;
        }

        public float GetDashBackSpeed()
        {
            return DashBackSpeed;
        }

        public float GetAirDashForwardSpeed()
        {
            return AirDashForwardSpeed;
        }

        public float GetAirDashBackSpeed()
        {
            return AirDashBackSpeed;
        }

        public bool GetInfiniteDashForward()
        {
            return InfiniteDashForward;
        }

        public float GetGravity()
        {
            return Gravity;
        }

        public int GetMaxHealth()
        {
            return MaxHealth;
        }

        public int GetMaxMeter()
        {
            return MaxMeter;
        }
    }
}

