﻿using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class LootData
    {
        public int Collected;
        public LootPieceDataDictionary LootPiecesOnScene = new LootPieceDataDictionary();

        public Action Changed;

        public void Collect(Loot loot)
        {
            Collected += loot.Value;
            Changed?.Invoke();
        }
    }
}
