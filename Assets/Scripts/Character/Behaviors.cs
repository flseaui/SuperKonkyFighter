using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;

namespace Character
{
    public abstract class Behaviors {
        
        protected const int Low = 1;//must be crouchblocked
        protected const int Mid = 2;//can be crouch or stand blocked
        protected const int High = 3;//must be stand blocked
        protected const int Unblockable = 4;//cannot be blocked

        protected const int None = 0; //no knockdown (follows hitstun numbers)
        protected const int Softkd = 1; //soft knockdown (techable)
        protected const int Hardkd = 2; //hard knockdown (untechable for 20 frames, OTG possible)
        protected const int Softgb = 3; //soft ground bounce (ground bounce with soft knockdown)
        protected const int Hardgb = 4; //hard ground bounce (ground bounce with hard knockdown)
        protected const int Softwb = 5; //soft wall bounce (wall bounce with soft knockdown)
        protected const int Hardwb = 6; //hard wall bounce (wall bounce with hard knockdown)

        
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

        protected static Action.Rect[,] CreateBoxes(int length, (int frame, Action.Rect[] boxes)[] frames)
        {
            var longest = frames.Select(frame => frame.boxes.Length).Concat(new[] {0}).Max();
            var boxes = new Action.Rect[longest, length];

            for (var i = 0; i < longest; ++i)
            {
                for (var j = 0; j < length; j++)
                {
                    boxes[i, j] = NullBox;
                }
            }

            foreach (var (frame, rects) in frames)
            {
                for (var i = 0; i < longest; ++i)
                {
                    boxes[i, frame] = rects[i];
                }
            }

            return boxes;
        }
        
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

