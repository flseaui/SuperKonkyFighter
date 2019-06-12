namespace Misc
{
    public static class Constants
    {
        /* 
        * Action ID FORMAT
        * id = numpad + power
        * - light    = + 0
        * - medium   = + 10
        * - heavy    = + 20
        * - special  = + 30
        * - advanced = + 40
        * example: standM = 5 + 10 = 15
         
        * ADVANCED ID FORMAT
        * 1 - Forward Dash
        * 2 - Back Dash
        * 3 - Forward Air Dash
        * 4 - Back Air Dash
        * 5 - Stun
        * 6 - Block
        * 7 - crouchBlock
        * 8 - airBlock
        * 9 - flip
        * 10 - crouchFlip
        * 11 - jumpSquat
        * 12 - throw
        * Other
        * 0 - Jump
        */

        
        // Action categories
        public const int NoAction = 0;
        public const int Medium = 10;
        public const int Heavy = 20;
        public const int Special = 30;
        public const int Advanced = 40;
        
        public const int CrouchBackL = 1;
        public const int CrouchBackM = 11;
        public const int CrouchBackH = 21;
        public const int CrouchL = 2;
        public const int CrouchM = 12;
        public const int CrouchH = 22;
        public const int CrouchForwardL = 3;
        public const int CrouchForwardM = 13;
        public const int CrouchForwardH = 23;


        public const int BackL    = 4;
        public const int BackM    = 14;
        public const int BackH    = 24;
        public const int StandL   = 5;
        public const int StandM   = 15;
        public const int StandH   = 25;
        public const int ForwardL = 6;
        public const int ForwardM = 16;
        public const int ForwardH = 26;

        public const int JumpBackL = 7;
        public const int JumpBackM = 17;
        public const int JumpBackH = 27;
        public const int JumpL = 8;
        public const int JumpM = 18;
        public const int JumpH = 28;
        public const int JumpForwardL = 9;
        public const int JumpForwardM = 19;
        public const int JumpForwardH = 29;

        public const int OneS   = 31;
        public const int TwoS   = 32;
        public const int ThreeS = 33;
        public const int FourS  = 34;
        public const int FiveS  = 35;
        public const int SixS   = 36;
        public const int SevenS   = 37;
        public const int EightS   = 38;
        public const int NineS   = 39;

        public const int ForwardDash    = 41;
        public const int BackDash       = 42;
        public const int ForwardAirDash = 43;
        public const int BackAirDash    = 44;
        public const int Stun           = 45;
        public const int Block          = 46;
        public const int CrouchBlock    = 47;
        public const int AirBlock       = 48;
        public const int Flip           = 49;
        public const int CrouchFlip     = 50;
        public const int JumpSquat      = 51;
        public const int Throw          = 52;
        public const int Knockdown      = 53;
        public const int KnockdownFall  = 54;
        public const int Grab           = 55;
        public const int WallBounce     = 56;
        public const int GroundBounce   = 57;
        public const int Victory        = 58;
        public const int Defeat         = 59;

        public const int CrouchBack = 101;
        public const int Crouch = 102;
        public const int CrouchForward = 103;
        public const int WalkBack = 104;
        public const int Idle = 105;
        public const int WalkForward = 106;
        public const int JumpBack = 107;
        public const int Jump = 108;
        public const int JumpForward = 109;
    }
}