using Microsoft.Xna.Framework;

namespace terraria
{
    public static class Constants
    {
        //Game vars
        public const string AppName = "Shameless Terraria ripoff";
        public const int WindowWidth = 1280;
        public const int WindowHeight = 720;
        //World movement vars
        public const float FollowSpeed = 0.05f;//between 0f and 1f
        
        //Movement vars
        public const float Gravity = 0.98f;//acceleration
        public const float GroundLevel = 456f;

        //block vars
        public const uint BlockSize = 24;


        //player enums
        public enum PlayerMovementEffects
        {
            None,
            Slowed,
            Burning,
            Frozen
        }
    }
}    

