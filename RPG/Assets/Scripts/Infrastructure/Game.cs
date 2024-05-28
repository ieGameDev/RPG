using Assets.Scripts.Services.Input;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class Game
    {
        public static IInputService InputService;

        public Game()
        {
            RegisterInputService();
        }

        private static void RegisterInputService()
        {
            if (Application.isEditor)
                InputService = new StandalobeInputService();
            else
                InputService = new MobileInputService();
        }
    }
}