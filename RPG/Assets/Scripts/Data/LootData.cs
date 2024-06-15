using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class LootData
    {
        public int Collected;

        public Action Changed;

        public void Collect(Loot loot)
        {
            Collected += loot.Value;
            Changed?.Invoke();
        }
    }
}
