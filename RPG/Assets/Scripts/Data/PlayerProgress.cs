using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public PlayerState PlayerState;
        public WorldData WorldData;

        public PlayerProgress(string initialLevel)
        {
            WorldData = new WorldData(initialLevel);
            PlayerState = new PlayerState();
        }
    }
}
