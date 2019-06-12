using System.Collections.Generic;
using Core;
using Player;
using static Misc.Constants;

namespace Character.Dark_Konky
{
    public class DkBehaviours : Behaviors
    {
        public DkBehaviours()
        {
            IDictionary<int, Action> dkActionIds = new Dictionary<int, Action>
            {
                { CrouchBackL,  _crouchL },
                { CrouchBackM, _crouchM },
                { CrouchBackH, _crouchH },

                { CrouchL,  _crouchL },
                { CrouchM, _crouchM },
                { CrouchH, _crouchH },

                { CrouchForwardL,  _crouchL },
                { CrouchForwardM, _crouchM },
                { CrouchForwardH, _crouchH },

                { BackL,  _standL },
                { BackM, _standM },
                { BackH, _standH },

                { StandL,  _standL },
                { StandM, _standM },
                { StandH, _standH },

                { ForwardL,  _standL  },
                { ForwardM, _forwardM },
                { ForwardH, _standH   },

                { JumpBackL,  _jumpL },
                { JumpBackM, _jumpM },
                { JumpBackH, _jumpH },

                { JumpL,  _jumpL },
                { JumpM, _jumpM },
                { JumpH, _jumpH },

                { JumpForwardL, _jumpL },
                { JumpForwardM, _jumpM },
                { JumpForwardH, _jumpH },

                { OneS  , _oneS   },
                { TwoS  , _twoS   },
                { ThreeS, _threeS },
                { FourS , _fourS  },
                { FiveS , _fiveS  },
                { SixS  , _sixS   },
                { SevenS, _sixS  },
                { EightS, _sixS  },
                { NineS , _sixS  },


                { ForwardDash   ,    _forwardDash},
                { BackDash      ,       _backDash},
                { ForwardAirDash, _forwardAirDash},
                { BackAirDash   ,    _backAirDash},
                { Stun          ,          _stun },
                { Block         ,         _block },
                { CrouchBlock   ,   _crouchBlock },
                { AirBlock      ,      _airBlock },
                { Flip          ,          _flip },
                { CrouchFlip    ,    _crouchFlip },
                { JumpSquat     ,     _jumpSquat },
                { Throw         ,         _throw },
                { Knockdown     ,     _knockdown },
                { KnockdownFall , _knockdownFall },
                { Grab          ,          _grab },
                { WallBounce    ,    _wallbounce },
                { GroundBounce  ,  _groundbounce },
                { Victory       ,       _victory },
                { Defeat        ,        _defeat },

                { CrouchBack, _crouch},
                { Crouch, _crouch},
                { CrouchForward, _crouch},
                { WalkBack, _walkBack},
                { Idle, _idle},
                { WalkForward, _walkForward},
                { JumpBack, _jumpBack},
                { Jump, _jump},
                { JumpForward, _jumpForward}
            };

            IDictionary<Action, int> dkAnimAction = new Dictionary<Action, int>
            {
                { _crouchL, CrouchL },
                { _crouchM, CrouchM },
                { _crouchH, CrouchH },

                { _standL, StandL },
                { _standM, StandM },
                { _forwardM, ForwardM },
                { _standH, StandH },

                { _jumpL, JumpL },
                { _jumpM, JumpM },
                { _jumpH, JumpH },

                { _oneS  , OneS   },
                { _twoS  , TwoS   },
                { _threeS, ThreeS },
                { _fourS , FourS  },
                { _fiveS , FiveS  },
                { _sixS  , SixS   },

                { _forwardDash   , ForwardDash},
                { _backDash      , BackDash},
                { _forwardAirDash, ForwardAirDash},
                { _backAirDash   , BackAirDash},
                { _stun          , Stun },
                { _block         , Block },
                { _crouchBlock   , CrouchBlock },
                { _airBlock      , AirBlock },
                { _flip          , Flip },
                { _crouchFlip    , CrouchFlip },
                { _jumpSquat     , JumpSquat },
                { _throw         , Throw },
                { _knockdown     , Knockdown },
                { _knockdownFall , KnockdownFall },
                { _grab          , Grab },
                { _wallbounce    , WallBounce },
                { _groundbounce  , GroundBounce },
                { _victory       , Victory },
                { _defeat        , Defeat }
            };

            SetIds(dkActionIds, dkAnimAction);
            SetDelegates();
        }

        //0 - Startup
        //1 - Active
        //2 - Multihit Recovery
        //3 - Recovery
        //4 - Buffer Window

        //Action class:
        //0 = light
        //1 = medium
        //2 = heavy
        //3 = special
        //4 = super

        // Standing Light
        private readonly Action _standL = new Action
        {
            Tier = 0,
            Frames = new[] { 0, 4, 4, 4, 4, 4, 1, 1, 3, 3, 3, 3 },// 6 | 2 | 4
            Damage = new[] { 0, 0, 0, 0, 0, 0, 300, 300, 300, 0, 0, 0, 0 },
            HitboxFrames = 3,
            HurtboxFrames = 17,
            HitboxData = new[,]
            {
                { new Action.Rect( 2, 9, 5, 2, 2, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
                {NullBox, },
                {NullBox, }
            },
            HurtboxData = new[,]
            {
                { new Action.Rect(0.5f, 2.5f, 6.5f, 5, 12, 0), new Action.Rect(1.5f, 9f, 4, 8, 12, 1), },
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
            },
            HMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            VMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            Level = 1,
            P1Scaling = .9f,
            Block = Mid,
            Knockdown = None,
            ActionCancels = new[] { 2, 15, 12, 16, 25, 22, 31, 32, 33, 34, 35, 36 },
            GAngle = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            GStrength = new[] { 0, 0, 0, 0, 0, .5f, .5f, .5f, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AAngle = new[] { 0, 0, 0, 0, 0, 30, 30, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, },
            AStrength = new float[] { 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            HitSound = AudioManager.Sound.Light,
            WhiffSound = AudioManager.Sound.Whiff,
        };

        // Standing Medium
        private readonly Action _standM = new Action
        {
            Tier = 1,
            Frames = new[] { 0, 0, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 7 | 6 | 18
            Damage = new[] { 0, 0, 0, 0, 0, 0, 0, 600, 600, 600, 600, 600, 600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            HitboxData = new[,]
            {
                { new Action.Rect(5, 9, 12, 4, 6, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
            },
            HurtboxData = new[,]
            {
                { new Action.Rect(-0.5f, 7, 6, 14, 7, 1), },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                { new Action.Rect(4, 4, 15, 8, 24, 1), },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
            },
            HMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            VMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            Level = 1,
            P1Scaling = 1.1f,
            Block = Mid,
            Knockdown = Softwb,
            ActionCancels = new[] { 22, 25, 31, 32, 33, 34, 35, 36 },
            GAngle = new[] { 0, 0, 0, 0, 0, 0, 0, 45, 45, 45, 45, 45, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            GStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 20, 20, 20, 20, 20, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AAngle = new[] { 0, 0, 0, 0, 0, 0, 0, 45, 45, 45, 45, 45, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 20, 20, 20, 20, 20, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            HitSound = AudioManager.Sound.Medium,
            WhiffSound = AudioManager.Sound.Whiff,
        };

        // Standing Heavy
        private readonly Action _standH = new Action
        {
            Tier = 2,
            Frames = new[] { 0, 0, 0, 0, 0, 0, 4, 4, 4, 4, 4, 5, 5, 5, 5, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 11 | 4 | 19
            Damage = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 900, 900, 900, 900, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            HitboxData = new[,]
            {
                { new Action.Rect(8, 7.5f, 7.5f, 2, 4, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
                {NullBox, },
                {NullBox, },
                {NullBox, }
            },
            HurtboxData = new[,]
            {
                { new Action.Rect(4, 2.5f, 6.5f, 5, 34, 1), new Action.Rect(3, 9f, 4, 8, 34, 9), },
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
            },
            HMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            VMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            Level = 1,
            P1Scaling = 1f,
            Block = Mid,
            Knockdown = Softkd,
            ActionCancels = new[] { 22, 31, 32, 33, 34, 35, 36 },
            GAngle = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 15, 15, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            GStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 15, 15, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AAngle = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 15, 15, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 15, 15, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            HitSound = AudioManager.Sound.Heavy,
            WhiffSound = AudioManager.Sound.Whiff,
        };

        // Crouching Light
        private readonly Action _crouchL = new Action
        {
            Tier = 0,
            Frames = new[] { 4, 4, 4, 4, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3 },// 4 | 4 | 6
            Damage = new[] { 0, 0, 0, 0, 300, 300, 300, 300, 0, 0, 0, 0, 0, 0 },
            HitboxData = new[,]
            {
                { new Action.Rect(2, 6, 8, 7, 4, 0), },
                {NullBox, },
                {NullBox, },
                {NullBox, }
            },
            HurtboxData = new[,]
            {
                { new Action.Rect(0.5f, 4, 7, 8, 14, 5), },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
            },
            HMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            VMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            Level = 1,
            P1Scaling = .9f,
            Block = Low,
            Knockdown = None,
            ActionCancels = new[] { 15, 12, 16, 25, 22, 31, 32, 33, 34, 35, 36 },
            GAngle = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            GStrength = new[] { .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f },
            AAngle = new[] { 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30 },
            AStrength = new[] { .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f },
            HitSound = AudioManager.Sound.Light,
            WhiffSound = AudioManager.Sound.Whiff,
        };

        // Crouching Medium
        private readonly Action _crouchM = new Action
        {
            Tier = 1,
            Frames = new[] { 0, 0, 4, 4, 4, 4, 4, 1, 2, 1, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 7 | 4 | 14
            Damage = new[] { 0, 0, 0, 0, 0, 0, 0, 250, 250, 250, 250, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            HitboxData = new[,]
            {
                { new Action.Rect(3, 0.5f, 8, 1, 1, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
                { new Action.Rect(3, 0.5f, 8, 1, 1, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
            },
            HurtboxData = new[,]
            {
                { new Action.Rect(0.5f, 2.5f, 5, 5, 25, 1), new Action.Rect(-1.5f, 5.5f, 4, 4, 25, 2), },
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
            HMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            VMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            Level = 2,
            P1Scaling = 1f,
            Block = Mid,
            Knockdown = None,
            ActionCancels = new[] { 25, 22, 31, 32, 33, 34, 35, 36 },
            GAngle = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            GStrength = new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            AAngle = new[] { 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30 },
            AStrength = new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            HitSound = AudioManager.Sound.Medium,
            WhiffSound = AudioManager.Sound.Whiff,
        };

        // Crouching Heavy
        private readonly Action _crouchH = new Action
        {
            Tier = 3,
            Frames = new[] { 4, 4, 4, 4, 4, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 5 | 3 | 25
            Damage = new[] { 0, 0, 0, 0, 0, 1000, 1000, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            HitboxData = new[,]
            {
                { new Action.Rect(6, 9.5f, 4, 14, 3, 0), new Action.Rect(9, 10, 3, 8, 3, 1), }, // Frame 1 - 1 hitbox lasts 4 frames
                {NullBox, NullBox, },
                {NullBox, NullBox, },
            },
            HurtboxData = new[,]
            {
                { new Action.Rect(0.5f, 4, 7, 8, 5, 2), NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                { new Action.Rect(2, 2.5f, 8, 5, 28, 2), new Action.Rect(3.5f, 9f, 4, 8, 28, 3), },
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
            },
            HMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            VMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            Level = 5,
            P1Scaling = 1f,
            Block = Mid,
            Knockdown = None,
            ActionCancels = new[] { 31, 32, 33, 34, 35, 36, 40 },
            GAngle = new[] { 0, 0, 0, 0, 0, 90, 90, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            GStrength = new float[] { 0, 0, 0, 0, 0, 10, 10, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AAngle = new[] { 0, 0, 0, 0, 0, 90, 90, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AStrength = new float[] { 0, 0, 0, 0, 0, 10, 10, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            HitSound = AudioManager.Sound.Heavy,
            WhiffSound = AudioManager.Sound.Whiff,
        };

        // Jumping Light
        private readonly Action _jumpL = new Action
        {
            Tier = 0,
            Frames = new[] { 0, 4, 4, 4, 4, 4, 1, 2, 1, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 6 | 2*2 | 12
            Damage = new[] { 0, 0, 0, 0, 0, 0, 300, 300, 300, 300, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            HitboxData = new[,]
            {
                { new Action.Rect(3.5f, 6, 9, 3, 1, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
                { new Action.Rect(3.5f, 6, 9, 3, 1, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
            },
            HurtboxData = new[,]
            {
                { new Action.Rect(1, 9, 4, 6, 24, 1),  new Action.Rect(3, 6, 8, 2, 24, 0),},
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
            },
            HMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            VMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            Level = 3,
            P1Scaling = .8f,
            Block = High,
            Knockdown = None,
            ActionCancels = new[] { 17, 18, 19, 27, 28, 29, 34, 36, 40, 43, 44 },
            GAngle = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            GStrength = new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            AAngle = new[] { 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45 },
            AStrength = new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            HitSound = AudioManager.Sound.Light,
            WhiffSound = AudioManager.Sound.Whiff,
        };

        // Jumping Medium
        private readonly Action _jumpM = new Action
        {
            Tier = 1,
            Frames = new[] { 0, 0, 0, 0, 4, 4, 4, 4, 4, 1, 1, 2, 1, 1, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 9 | 3*2 | 19
            Damage = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 400, 400, 400, 400, 400, 400, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            HitboxData = new[,]
            {
                { new Action.Rect(6, 6, 8, 8, 3, 0), }, // Frame 1 - 1 hitbox lasts 4 frames
                {NullBox, },
                {NullBox, },
                { new Action.Rect(6, 6, 8, 8, 3, 0), }, // Frame 1 - 1 hitbox lasts 4 frames
                {NullBox, },
                {NullBox, },
            },
            HurtboxData = new[,]
            {
                { new Action.Rect(-0.5f, 5, 5, 6, 34, 1), new Action.Rect(4, 9, 5, 5, 34, 2), },
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox},
                {NullBox, NullBox}
            },
            HMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            VMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            Level = 4,
            P1Scaling = .8f,
            Block = High,
            Knockdown = None,
            ActionCancels = new[] { 27, 28, 29, 34, 36, 40, 43, 44 },
            GAngle = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            GStrength = new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
            AAngle = new[] { 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 0, 0, 0, 0 },
            AStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            HitSound = AudioManager.Sound.Medium,
            WhiffSound = AudioManager.Sound.Whiff,
        };

        // Jumping Heavy
        private readonly Action _jumpH = new Action
        {
            Tier = 2,
            Frames = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 4, 4, 4, 4, 4, 5, 5, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 13 | 2 | 30
            Damage = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 900, 900, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            HitboxData = new[,]
            {
                { new Action.Rect(7, 6, 10, 5, 2, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
                {NullBox, },
            },
            HurtboxData = new[,]
            {
                { new Action.Rect(1.5f, 8.5f, 6, 12, 13, 2), NullBox,},
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
                {NullBox, new Action.Rect(5, 4, 6, 4, 32, 2), },
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
            },
            HMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            VMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            Level = 5,
            P1Scaling = .8f,
            Block = High,
            Knockdown = Softwb,
            ActionCancels = new[] { 34, 36, 40, 43, 44 },
            GAngle = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 45, 45, 45, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            GStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 20, 20, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AAngle = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 45, 45, 45, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 20, 20, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            HitSound = AudioManager.Sound.Heavy,
            WhiffSound = AudioManager.Sound.Whiff,
        };

        // Forward Medium
        private readonly Action _forwardM = new Action
        {
            Tier = 1,
            Frames = new[] { 0, 0, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 7 | 6 | 18
            Damage = new[] { 0, 0, 0, 0, 0, 0, 0, 600, 600, 600, 600, 600, 600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            HitboxData = new[,]
            {
                { new Action.Rect(5, 14, 12, 4, 6, 0),  }, // Frame 1 - 1 hitbox lasts 4 frames
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
            },
            HurtboxData = new[,]
            {
                { new Action.Rect(-0.5f, 7, 6, 14, 7, 1), },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                { new Action.Rect(4, 4, 15, 8, 24, 1), },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, },
            },
            HMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            VMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            Level = 1,
            P1Scaling = 1.1f,
            Block = Mid,
            Knockdown = Softwb,
            ActionCancels = new[] { 22, 25, 31, 32, 33, 34, 35, 36 },
            GAngle = new[] { 0, 0, 0, 0, 0, 0, 0, 45, 45, 45, 45, 45, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            GStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 20, 20, 20, 20, 20, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AAngle = new[] { 0, 0, 0, 0, 0, 0, 0, 45, 45, 45, 45, 45, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 20, 20, 20, 20, 20, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            HitSound = AudioManager.Sound.Medium,
            WhiffSound = AudioManager.Sound.Whiff,
        };


        // One Super
        private readonly Action _oneS = new Action
        {
            Tier = 3,
            Frames = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 15 | 5 | 15
            Damage = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            HitboxData = new Action.Rect[,]
            {

            },
            HurtboxData = new[,]
            {
                { new Action.Rect(0.5f, 2.5f, 6.5f, 5, 15, 1), new Action.Rect(1.5f, 9f, 4, 8, 15, 2), },
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
                { new Action.Rect(0, 0, 0, 0, 5, 1), new Action.Rect(0, 0, 0, 0, 5, 2), },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                { new Action.Rect(0.5f, 2.5f, 6.5f, 5, 15, 1), new Action.Rect(1.5f, 9f, 4, 8, 15, 2), },
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
            },
            HMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            VMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, -40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            Level = 5,
            P1Scaling = 1,
            Block = Mid,
            Knockdown = Softwb,
            ActionCancels = new[] { 35, 40, 41, 42, 43, 44 },
            GAngle = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            GStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AAngle = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AirOk = true,
            HitSound = AudioManager.Sound.Dp,
            WhiffSound = AudioManager.Sound.Whiff,
        };

        // Two Super
        private readonly Action _twoS = new Action
        {
            Tier = 3,
            Frames = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 15 | 5 | 15
            Damage = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            HitboxData = new Action.Rect[,]
            {

            },
            HurtboxData = new[,]
            {
                { new Action.Rect(0.5f, 2.5f, 6.5f, 5, 15, 1), new Action.Rect(1.5f, 9f, 4, 8, 15, 2), },
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
                { new Action.Rect(0, 0, 0, 0, 5, 1), new Action.Rect(0, 0, 0, 0, 5, 2), },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                { new Action.Rect(0.5f, 2.5f, 6.5f, 5, 15, 1), new Action.Rect(1.5f, 9f, 4, 8, 15, 2), },
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
            },
            HMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            VMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, -40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            Level = 5,
            P1Scaling = 1,
            Block = Mid,
            Knockdown = Softwb,
            ActionCancels = new[] { 35, 40, 41, 42, 43, 44 },
            GAngle = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            GStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AAngle = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AirOk = true,
            HitSound = AudioManager.Sound.Dp,
            WhiffSound = AudioManager.Sound.Whiff,
        };

        // Three Super
        private readonly Action _threeS = new Action
        {
            Tier = 3,
            Frames = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, // 15 | 5 | 15
            Damage = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 800, 800, 800, 800, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            HitboxData = new[,]
            {
                { new Action.Rect(6, 10, 6, 6, 2, 0), },
                {NullBox, },
                {NullBox, },
                {NullBox, },
                {NullBox, }
            },
            HurtboxData = new[,]
            {
                { new Action.Rect(0.5f, 2.5f, 6.5f, 5, 35, 1), new Action.Rect(2, 9f, 7, 8, 35, 2), },
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
            },
            HMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            VMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            Level = 3,
            P1Scaling = 1,
            Block = Mid,
            Knockdown = None,
            ActionCancels = new[] { 35 },
            GAngle = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            GStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 5, 5, 5, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AAngle = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 45, 45, 45, 45, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 5, 5, 5, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AirOk = true,
            HitSound = AudioManager.Sound.Fireball,
            WhiffSound = AudioManager.Sound.Fireball,
        };

        // Four Super
        private readonly Action _fourS = new Action
        {
            Tier = 3,
            Frames = new[] { 0, 0, 0, 0, 0, 4, 4, 4, 4, 4, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 5, 5, 5, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 10 | 1 (1) 1 (1) 1 (1) 1 (1) 1 (1) 1 (1) 1 (1) 1 (1) 1 (1) (8) 3 | 26
            Damage = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 200, 0, 0, 0, 0, 0, 0, 0, 0, 1500, 1500, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            HitboxData = new[,]
            {
                { new Action.Rect(2, 6, 8, 6, 1, 0), },
                { new Action.Rect(2, 6, 8, 6, 1, 0), },
                { new Action.Rect(2, 6, 8, 6, 1, 0), },
                { new Action.Rect(2, 6, 8, 6, 1, 0), },
                { new Action.Rect(2, 6, 8, 6, 1, 0), },
                { new Action.Rect(2, 6, 8, 6, 1, 0), },
                { new Action.Rect(2, 6, 8, 6, 1, 0), },
                { new Action.Rect(2, 6, 8, 6, 1, 0), },
                { new Action.Rect(2, 6, 8, 6, 1, 0), },
                { new Action.Rect(6, 7, 4, 14, 3, 1) },
                {NullBox, },
                {NullBox, },
            },
            HurtboxData = new[,]
            {
                { new Action.Rect(0.5f, 2.5f, 6.5f, 5, 10, 1), new Action.Rect(1.5f, 9f, 4, 8, 10, 2), },//startup
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                { new Action.Rect(0.5f, 4, 7, 8, 18, 5), NullBox },//active
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
                { new Action.Rect(0.5f, 4, 7, 8, 8, 5), NullBox },//gap
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                {NullBox, NullBox },
                { new Action.Rect(0.5f, 4, 7, 8, 3, 5), NullBox },//active
                {NullBox, NullBox },
                {NullBox, NullBox },
                { new Action.Rect(0.5f, 4, 7, 8, 26, 5), NullBox },//recovery
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
            },
            HMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            VMovement = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            Level = 5,
            P1Scaling = 1,
            Block = Mid,
            Knockdown = Softgb,
            ActionCancels = new[] { 35 },
            GAngle = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -45, -45, -45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            GStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 25, 25, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AAngle = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 0, 0, 0, 0, 0, 0, 0, 0, -45, -45, -45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 25, 25, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AirOk = true,
            HitSound = AudioManager.Sound.Dp,
            WhiffSound = AudioManager.Sound.Whiff,
        };

        // Five Super
        private readonly Action _fiveS = new Action
        {
            Tier = 4,
            Frames = new[] { 0, 0, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },// 2 | 10 | 55
            Damage = new[] { 0, 0, 3000, 3000, 3000, 3000, 3000, 3000, 3000, 3000, 3000, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            HitboxData = new[,]
            {
                { new Action.Rect(4, 10, 4, 12, 2, 0), },
                { NullBox, },
                { NullBox, },
                { NullBox, },
                { NullBox, },
                { NullBox, },
                { NullBox, },
                { NullBox, },
                { NullBox, },
                { NullBox, },
            },
            HurtboxData = new[,]
            {
                { new Action.Rect(0, 0, 0, 0, 12, 0), },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                { new Action.Rect(1, 6, 4, 12, 55, 0), },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
            },
            HMovement = new float[] {  },
            VMovement = new float[] {  },
            Level = 5,
            P1Scaling = 1.5f,
            Block = Mid,
            Knockdown = Softwb,
            Super = true,
            ActionCancels = new[] { 35 },
            GAngle = new[] { 0, 0, 120, 120, 120, 120, 120, 120, 120, 120, 120, 120, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            GStrength = new float[] { 0, 0, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AAngle = new[] { 0, 0, 120, 120, 120, 120, 120, 120, 120, 120, 120, 120, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AStrength = new float[] { 0, 0, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AirOk = false,
            HitSound = AudioManager.Sound.Super,
            WhiffSound = AudioManager.Sound.Whiff,
        };

        // Six Super
        private readonly Action _sixS = new Action
        {
            Tier = 3,
            Frames = new[] { 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },// 4 | 10 | 70
            Damage = new[] { 0, 0, 0, 0, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            HitboxData = new[,]
            {
                { new Action.Rect(4, 10, 4, 12, 10, 0), },
                { NullBox, },
                { NullBox, },
                { NullBox, },
                { NullBox, },
                { NullBox, },
                { NullBox, },
                { NullBox, },
                { NullBox, },
                { NullBox, },
            },
            HurtboxData = new[,]
            {
                { new Action.Rect(0, 0, 0, 0, 14, 0), },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                { new Action.Rect(1, 6, 4, 12, 70, 0), },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
            },
            HMovement = new[] { 0, 0, 0, 0, .2f, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, },
            VMovement = new float[] { 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            Level = 5,
            P1Scaling = 1f,
            Block = Mid,
            Knockdown = Softkd,
            ActionCancels = new[] { 35 },
            GAngle = new[] { 0, 0, 0, 0, 90, 90, 90, 90, 90, 90, 90, 90, 90, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            GStrength = new float[] { 0, 0, 0, 0, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AAngle = new[] { 0, 0, 0, 0, 90, 90, 90, 90, 90, 90, 90, 90, 90, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AStrength = new float[] { 0, 0, 0, 0, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AirOk = true,
            HitSound = AudioManager.Sound.Dp,
            WhiffSound = AudioManager.Sound.Whiff,
        };

        // Throw
        private readonly Action _throw = new Action
        {
            Tier = 2,
            Frames = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
            Damage = new[] { 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1500, },
            HitboxData = new[,]
            {
                { new Action.Rect(0, 0, 20, 120, 3, 20), },
                {NullBox },
                {NullBox },
            },
            HurtboxData = new[,]
            {
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
            },
            Level = 5,
            P1Scaling = .5f,
            Block = Unblockable,
            Knockdown = Softwb,
            ActionCancels = new int[] { },
            GAngle = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 30, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            GStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 15, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AAngle = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 30, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AStrength = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 15, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            AirOk = true,
            HitSound = AudioManager.Sound.Heavy,
            WhiffSound = AudioManager.Sound.Whiff,
        };

        // Throw
        private readonly Action _grab = new Action
        {
            Tier = 2,
            Frames = new[] { 1, 1 },
            Damage = new[] { 0, 0 },
            HitboxData = new[,]
            {
                { new Action.Rect(2, 6, 2, 12, 2, 6), },
                {NullBox },
            },
            HurtboxData = new[,]
            {
                { new Action.Rect(0, 0, 0, 0, 2, 6), },
                {NullBox },

            },
            Level = 5,
            P1Scaling = .5f,
            Block = Unblockable,
            Knockdown = None,
            ActionCancels = new int[] { },
            GAngle = new[] { 0, 0 },
            GStrength = new float[] { 0, 0 },
            AAngle = new[] { 0, 0 },
            AStrength = new float[] { 0, 0 },
            AirOk = false,
        };


        // Jump Squat
        private readonly Action _jumpSquat = new Action
        {
            Frames = new[] { 0, 0 },
            ActionCancels = new int[] { },
            HurtboxData = new[,]
            {
                { new Action.Rect(0.5f, 2.5f, 6.5f, 5, 2, 1), new Action.Rect(0.5f, 9f, 4, 8, 2, 2), },
                {NullBox, NullBox },
            },
        };

        // Turns
        private readonly Action _flip = new Action
        {
            Frames = new[] { 0, 0, 0, },
            ActionCancels = new int[] { },
            HurtboxData = new[,]
            {
                { new Action.Rect(0.5f, 2.5f, 6.5f, 5, 3, 1), new Action.Rect(0.5f, 9f, 4, 8, 3, 2), },
                {NullBox, NullBox },
                {NullBox, NullBox }
            },
        };

        // crouch Turns
        private readonly Action _crouchFlip = new Action
        {
            Frames = new[] { 0, 0, 0, },
            ActionCancels = new int[] { },
            HurtboxData = new[,]
            {
                { new Action.Rect(0.5f, 4, 7, 8, 3, 5), },
                {NullBox },
                {NullBox },
            },
        };

        // Back Dash
        private readonly Action _backDash = new Action
        {
            Frames = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            ActionCancels = new int[] { },
            HurtboxData = new[,]
            {
                { new Action.Rect(0, 0, 0, 0, 10, 1), new Action.Rect(0, 0, 0, 0, 10, 2), },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {new Action.Rect(0.5f, 2.5f, 6.5f, 5, 10, 1), new Action.Rect(-1.5f, 9f, 4, 8, 10, 2), },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {NullBox, NullBox, }
            },
        };

        // Forward Dash
        private readonly Action _forwardDash = new Action
        {
            Frames = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3 },
            ActionCancels = new[] { 40 },
            Infinite = true,
            HurtboxData = new[,]
            {
                { new Action.Rect(-4, 2.5f, 8, 5, 14, 1), new Action.Rect(2.5f, 7, 5, 8, 14, 2), },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                {NullBox, NullBox, },
                { new Action.Rect(-4, 2.5f, 8, 5, 1000, 1), new Action.Rect(2.5f, 7, 5, 8, 1000, 2), }
            },
        };

        // Stun
        private readonly Action _stun = new Action
        {
            Frames = new[] { 3 },
            ActionCancels = new int[] { },
            Infinite = true,
            HurtboxData = new[,]
            {
                { new Action.Rect(0.5f, 6, 4, 12, 40, 9)},
            },
        };

        private readonly Action _knockdown = new Action
        {
            Frames = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            ActionCancels = new int[] { },
            Infinite = false,
            HurtboxData = new[,]
            {
                { new Action.Rect(0, 0, 0, 0, 50, 1)},
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },

            },
        };

        private readonly Action _knockdownFall = new Action
        {
            Frames = new[] { 0 },
            ActionCancels = new int[] { },
            Infinite = true,
            HurtboxData = new[,]
            {
                { new Action.Rect(0.5f, 2.5f, 6.5f, 5, 2, 9), new Action.Rect(1.5f, 9f, 4, 8, 2, 10), },
            }
        };

        private readonly Action _wallbounce = new Action
        {
            Frames = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, },
            ActionCancels = new int[] { },
            Infinite = false,
            HurtboxData = new[,]
            {
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
            }
        };

        private readonly Action _groundbounce = new Action
        {
            Frames = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, },
            ActionCancels = new int[] { },
            Infinite = false,
            HurtboxData = new[,]
            {
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
            }
        };

        private readonly Action _victory = new Action
        {
            Frames = new[] { 0 },
            Infinite = true,
            HurtboxData = new[,]
            {
                {NullBox, NullBox }
            },
        };

        private readonly Action _defeat = new Action
        {
            Frames = new[] { 0 },
            Infinite = true,
            HurtboxData = new[,]
            {
                {NullBox, NullBox }
            },
        };


        // Block
        private readonly Action _block = new Action
        {
            Frames = new[] { 0 },
            ActionCancels = new int[] { },
            Infinite = true,
            HurtboxData = new[,]
            {
                { new Action.Rect(0.5f, 2.5f, 6.5f, 5, 1, 3), new Action.Rect(1.5f, 9f, 4, 8, 1, 4), },
            },
            HitSound = AudioManager.Sound.Block
        };

        // Crouching Block
        private readonly Action _crouchBlock = new Action
        {
            Frames = new[] { 0 },
            ActionCancels = new int[] { },
            Infinite = true,
            HurtboxData = new[,]
            {
                { new Action.Rect(0.5f, 4, 7, 8, 1, 5), },
            },
            HitSound = AudioManager.Sound.Block
        };

        // Air Block
        private readonly Action _airBlock = new Action
        {
            Frames = new[] { 0 },
            ActionCancels = new int[] { },
            Infinite = true,
            HurtboxData = new[,]
            {
                { new Action.Rect(1, 6, 4, 12, 10, 10), },
            }
        };

        // Air Dash
        private readonly Action _forwardAirDash = new Action
        {
            Frames = new[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, },
            ActionCancels = new[] { 17, 18, 19, 27, 28, 29, 40, 43, 44 },
            HurtboxData = new[,]
            {
                //{ new Action.rect(2, 7, 14, 4, 15, 1),},
                { new Action.Rect(-4, 2.5f, 8, 5, 15, 1), new Action.Rect(2.5f, 7, 5, 8, 15, 2), },
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
            },
        };
        private readonly Action _backAirDash = new Action
        {
            Frames = new[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
            ActionCancels = new int[] { },
            HurtboxData = new[,]
            {
                { new Action.Rect(0, 0, 0, 0, 3, 1), new Action.Rect(0, 0, 0, 0, 3, 2), },
                {NullBox, NullBox },
                {NullBox, NullBox },
                { new Action.Rect(3, 3, 3, 6, 17, 1), new Action.Rect(1, 9, 3, 6, 17, 2), },
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

        private readonly Action _crouch = new Action
        {
            Frames = new[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
            HurtboxData = new[,]
            {
                { new Action.Rect(0.5f, 4, 7, 8, 40, 5), },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox }
            },
        };

        private readonly Action _walkBack = new Action
        {
            Frames = new[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
            HurtboxData = new[,]
            {
                { new Action.Rect(0.5f, 2.5f, 6.5f, 5, 20, 3), new Action.Rect(1.5f, 9f, 4, 8, 20, 4), },
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
            },
        };

        private readonly Action _idle = new Action
        {
            Frames = new[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
            HurtboxData = new[,]
            {
                { new Action.Rect(0.5f, 2.5f, 6.5f, 5, 40, 1), new Action.Rect(1.5f, 9f, 4, 8, 40, 2), },
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

        private readonly Action _walkForward = new Action
        {
            Frames = new[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
            HurtboxData = new[,]
            {
                { new Action.Rect(0.5f, 2.5f, 6.5f, 5, 25, 6), new Action.Rect(1.5f, 9f, 4, 8, 25, 7), },
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
            },
        };

        private readonly Action _jumpBack = new Action
        {
            Frames = new[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
            HurtboxData = new[,]
            {
                { new Action.Rect(3, 3, 3, 6, 10, 8), new Action.Rect(1, 9, 3, 6, 10, 9), },
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

        private readonly Action _jump = new Action
        {
            Frames = new[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
            HurtboxData = new[,]
            {
                { new Action.Rect(1, 6, 4, 12, 10, 10), },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox },
                {NullBox }
            },
        };

        private readonly Action _jumpForward = new Action
        {
            Frames = new[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
            HurtboxData = new[,]
            {
                { new Action.Rect(0, 3, 3, 6, 10, 11), new Action.Rect(2, 9, 4, 8, 10, 12), },
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

        public override void SetDelegates()
        {
            OnAdvancedActionCallbacks = new[]
            {
                AdvJump,
                AdvDashForward,
                AdvDashBack,
                AdvAirDashForward,
                AdvAirDashBack,
                AdvStun,
                AdvBlock,
                AdvCrouchBlock,
                AdvAirBlock,
                AdvFlip,
                AdvCrouchFlip,
                AdvJumpSquat,
                AdvThrow,
                AdvKnockdown,
                AdvKnockdownFall,
                AdvGrab,
                AdvWallbounce,
                AdvGroundbounce,
                AdvVictory,
                new OnAdvancedAction(AdvDeath)
            };
        }

        // ADVANCED ACTIONS
        private void AdvJump(PlayerScript player)
        {

        }

        private void AdvDashForward(PlayerScript player)
        {
            player.hVelocity = ForwardSpeed * DashForwardSpeed;
            if (InfiniteDashForward)
                player.CheckDashEnd();
        }

        private void AdvDashBack(PlayerScript player)
        {
            player.hVelocity = BackwardSpeed * DashBackSpeed;
        }

        private void AdvAirDashForward(PlayerScript player)
        {
            player.vVelocity = 0;
            player.hVelocity = ForwardSpeed * AirDashForwardSpeed;
        }

        private void AdvAirDashBack(PlayerScript player)
        {
            player.vVelocity = 0;
            player.hVelocity = BackwardSpeed * AirDashBackSpeed;
        }

        private void AdvStun(PlayerScript player)
        {
            if (player.firstStun)
            {
                player.hVelocity = 0;
                player.vVelocity = 0;
                player.firstStun = false;
            }

            if (player.shouldWallbounce && (player.transform.position.x <= player.cameraLeft.position.x || player.transform.position.x >= player.cameraRight.position.x))
            {
                player.ActionEnd();
                player.executingAction = 56;
            }
            if (player.shouldGroundbounce && player.transform.position.y <= 0)
            {
                player.ActionEnd();
                player.executingAction = 57;
            }

            player.stunTimer--;
            if (player.stunTimer <= 0)
            {
                player.ActionEnd();
            }
        }

        private void AdvBlock(PlayerScript player)
        {
            if (player.inputManager.CurrentInput[7] && !player.otherPlayer.GetComponent<PlayerScript>().behaviors.GetAction(player.otherPlayer.GetComponent<PlayerScript>().executingAction).NonPushBlockable)
            {
                player.otherPlayer.GetComponent<PlayerScript>().hKnockback = (player.otherPlayer.GetComponent<PlayerScript>().playerSide ? -2 : 2);
                AudioManager.Instance.PlaySound(AudioManager.Sound.PushBlock);
            }

            player.hVelocity = 0;
            player.CheckBlockEnd();
        }

        private void AdvCrouchBlock(PlayerScript player)
        {
            player.hVelocity = 0;
            player.CheckBlockEnd();
        }

        private void AdvAirBlock(PlayerScript player)
        {
            player.hVelocity = 0;
            player.vVelocity = 0;
            player.CheckBlockEnd();
        }

        private void AdvFlip(PlayerScript player)
        {

        }

        private void AdvCrouchFlip(PlayerScript player)
        {

        }

        private void AdvJumpSquat(PlayerScript player)
        {
            player.hVelocity = 0;
            player.vVelocity = 0;

            if (player.actionFrameCounter == 1)
            {
                player.jumpSquated = true;
            }
        }

        private void AdvThrow(PlayerScript player)
        {
            if (player.currentFrameType == 1 || player.currentFrameType == 5)
                player.ThrowThatMfOtherPlayer();
        }

        private void AdvKnockdown(PlayerScript player)
        {
            player.hVelocity = 0;
            player.hKnockback = 0;
            player.vVelocity = 0;
            player.vKnockback = 0;
        }

        private void AdvKnockdownFall(PlayerScript player)
        {
            player.hVelocity = 0;
            player.hKnockback = 0;

        }

        private void AdvGrab(PlayerScript player)
        {
            player.hVelocity = 0;
            if (player.actionFrameCounter > 0)
            {
                if (player.damageDealt && !player.otherPlayer.GetComponent<PlayerScript>().airborn)
                {
                    player.ActionEnd();
                    player.executingAction = 52;
                }
                else
                {
                    player.ActionEnd();
                    player.executingAction = 25;
                }
            }
        }

        private void AdvWallbounce(PlayerScript player)
        {

            if (player.actionFrameCounter == 1)
            {
                player.stored = player.hKnockback;
            }

            if (player.actionFrameCounter == 9)
            {
                player.hKnockback = player.stored * -.5f;
                player.ActionEnd();
                player.executingAction = 54;
            }
        }

        private void AdvGroundbounce(PlayerScript player)
        {
            if (player.actionFrameCounter == 1)
            {
                player.stored = player.vKnockback * -1;
                player.vKnockback = 0;
                player.hKnockback = 0;
            }

            if (player.actionFrameCounter == 9)
            {
                player.vKnockback = player.stored + 3;
                player.ActionEnd();
                player.executingAction = 54;
            }
        }

        private void AdvVictory(PlayerScript player)
        {

        }

        private void AdvDeath(PlayerScript player)
        {

        }


        public override void SetStats()
        {
            ForwardSpeed = 0.23f;
            BackwardSpeed = -0.17f;
            JumpDirectionSpeed = 1.25f;
            DashForwardSpeed = 4f;
            DashBackSpeed = 4f;
            AirDashForwardSpeed = 5f;
            AirDashBackSpeed = 3f;
            Gravity = -0.05f;
            MaxHealth = 10500;
            MaxMeter = 8000;
            InfiniteDashForward = true;
        }

        public override float GetAttackMovementHorizontal(int attackState)
        {
            switch (attackState)
            {
                //Crouching Light
                case 1:
                    return 1;
                case 2:
                    return 1;
                case 3:
                    return 1;

                //Crouching Medium
                case 11:
                    return 1;
                case 12:
                    return 1;
                case 13:
                    return 1;

                //Crouching Heavy
                case 21:
                    return 0;
                case 22:
                    return 0;
                case 23:
                    return 0;

                //Standing Light
                case 4:
                    return 0;
                case 5:
                    return 0;
                case 6:
                    return 0;

                //Standing Medium
                case 14:
                    return 1;
                case 15:
                    return 1;

                //Forward Medium
                case 16:
                    return 1;

                //Standing Heavy
                case 24:
                    return 1;
                case 25:
                    return 1;
                case 26:
                    return 1;

                //Jumping Light
                case 7:
                    return 1.5f;
                case 8:
                    return 1.5f;
                case 9:
                    return 1.5f;

                //Jumping Medium
                case 17:
                    return .5f;
                case 18:
                    return .5f;
                case 19:
                    return .5f;

                //Jumping Heavy
                case 27:
                    return 0;
                case 28:
                    return 0;
                case 29:
                    return 0;

                default:
                    return 0;

            }
        }

        public override float GetAttackMovementVertical(int attackState)
        {
            switch (attackState)
            {
                //Jumping Light
                case 7:
                    return 1.5f;
                case 8:
                    return 1.5f;
                case 9:
                    return 1.5f;

                //Jumping Medium
                case 17:
                    return .5f;
                case 18:
                    return .5f;
                case 19:
                    return .5f;

                //Jumping Heavy
                case 27:
                    return 0f;
                case 28:
                    return 0f;
                case 29:
                    return 0f;

                default:
                    return 0;

            }
        }
    }
}