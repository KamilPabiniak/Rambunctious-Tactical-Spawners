using UnityEngine;

namespace Core
{
    public class TickService : ITickService
    {
        public float TickRate { get; private set; } = 1f;

        public void SetTickRate(float newRate)
        {
            TickRate = newRate;
            Time.timeScale = newRate;
        }
    }
}