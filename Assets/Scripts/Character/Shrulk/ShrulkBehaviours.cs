using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using static Misc.Constants;

namespace Character.Shrulk
{
    public class ShrulkBehaviours : Behaviors
    {
        public ShrulkBehaviours()
        {
            IDictionary<int, Action> shrulkActionIds = new Dictionary<int, Action>
            {
                //{ CrouchBackL,  _crouchL },
                //{ CrouchBackM, _crouchM },
                //{ CrouchBackH, _crouchH },

                //{ CrouchL,  _crouchL },
                //{ CrouchM, _crouchM },
                //{ CrouchH, _crouchH },

                //{ CrouchForwardL,  _crouchL },
                //{ CrouchForwardM, _crouchM },
                //{ CrouchForwardH, _crouchH },

                //{ BackL,  _standL },
                //{ BackM, _standM },
                //{ BackH, _standH },

                //{ StandL,  _standL },
                //{ StandM, _standM },
                //{ StandH, _standH },

                //{ ForwardL,  _standL  },
                //{ ForwardM, _forwardM },
                //{ ForwardH, _standH   },

                //{ JumpBackL,  _jumpL },
                //{ JumpBackM, _jumpM },
                //{ JumpBackH, _jumpH },

                //{ JumpL,  _jumpL },
                //{ JumpM, _jumpM },
                //{ JumpH, _jumpH },

                //{ JumpForwardL, _jumpL },
                //{ JumpForwardM, _jumpM },
                //{ JumpForwardH, _jumpH },

                //{ OneS  , _oneS   },
                //{ TwoS  , _twoS   },
                //{ ThreeS, _threeS },
                //{ FourS , _fourS  },
                //{ FiveS , _fiveS  },
                //{ SixS  , _sixS   },
                //{ SevenS, _sixS  },
                //{ EightS, _sixS  },
                //{ NineS , _sixS  },


                //{ ForwardDash   ,    _forwardDash},
                //{ BackDash      ,       _backDash},
                //{ ForwardAirDash, _forwardAirDash},
                //{ BackAirDash   ,    _backAirDash},
                //{ Stun          ,          _stun },
                //{ Block         ,         _block },
                //{ CrouchBlock   ,   _crouchBlock },
                //{ AirBlock      ,      _airBlock },
                //{ Flip          ,          _flip },
                //{ CrouchFlip    ,    _crouchFlip },
                //{ JumpSquat     ,     _jumpSquat },
                //{ Throw         ,         _throw },
                //{ Knockdown     ,     _knockdown },
                //{ KnockdownFall , _knockdownFall },
                //{ Grab          ,          _grab },
                //{ WallBounce    ,    _wallbounce },
                //{ GroundBounce  ,  _groundbounce },
                //{ Victory       ,       _victory },
                //{ Defeat        ,        _defeat },

                //{ CrouchBack, _crouch},
                //{ Crouch, _crouch},
                //{ CrouchForward, _crouch},
                //{ WalkBack, _walkBack},
                { Idle, _idle},
                //{ WalkForward, _walkForward},
                //{ JumpBack, _jumpBack},
                //{ Jump, _jump},
                //{ JumpForward, _jumpForward}
            };

            IDictionary<Action, int> shrulkAnimAction = new Dictionary<Action, int>
            {
                //{ _crouchL, CrouchL },
                //{ _crouchM, CrouchM },
                //{ _crouchH, CrouchH },

                //{ _standL, StandL },
                //{ _standM, StandM },
                //{ _forwardM, ForwardM },
                //{ _standH, StandH },

                //{ _jumpL, JumpL },
                //{ _jumpM, JumpM },
                //{ _jumpH, JumpH },

                //{ _oneS  , OneS   },
                //{ _twoS  , TwoS   },
                //{ _threeS, ThreeS },
                //{ _fourS , FourS  },
                //{ _fiveS , FiveS  },
                //{ _sixS  , SixS   },

                //{ _forwardDash   , ForwardDash},
                //{ _backDash      , BackDash},
                //{ _forwardAirDash, ForwardAirDash},
                //{ _backAirDash   , BackAirDash},
                //{ _stun          , Stun },
                //{ _block         , Block },
                //{ _crouchBlock   , CrouchBlock },
                //{ _airBlock      , AirBlock },
                //{ _flip          , Flip },
                //{ _crouchFlip    , CrouchFlip },
                //{ _jumpSquat     , JumpSquat },
                //{ _throw         , Throw },
                //{ _knockdown     , Knockdown },
                //{ _knockdownFall , KnockdownFall },
                //{ _grab          , Grab },
                //{ _wallbounce    , WallBounce },
                //{ _groundbounce  , GroundBounce },
                //{ _victory       , Victory },
                //{ _defeat        , Defeat }
            };

            SetIds(shrulkActionIds, shrulkAnimAction);
            SetDelegates();
        }

        private readonly Action _idle = new Action
        {
            Frames = new[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
            HurtboxData = new[,]
            {
                { new Action.Rect(0.5f, 2.5f, 6.5f, 5, 40, 1), new Action.Rect(1.5f, 9f, 4, 8, 40, 2) },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox }
            },
        };
    }
}
