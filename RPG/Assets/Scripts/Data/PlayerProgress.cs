﻿using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public PlayerState PlayerState;
        public WorldData WorldData;
        public Stats PlayerStats;

        public PlayerProgress(string initialLevel)
        {
            WorldData = new WorldData(initialLevel);
            PlayerState = new PlayerState();
            PlayerStats = new Stats();
        }
    }
}
