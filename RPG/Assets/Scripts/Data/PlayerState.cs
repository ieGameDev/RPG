using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class PlayerState
    {
        public float CurrentHP;
        public float MaxHP;

        public void ResetHP() => 
            CurrentHP = MaxHP;
    }
}
