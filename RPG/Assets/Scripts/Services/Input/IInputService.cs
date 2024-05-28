using UnityEngine;

namespace Assets.Scripts.Services.Input
{
    public interface IInputService
    {
        Vector2 Axis { get; }
        bool IsAttackButtonUp();
    }
}
