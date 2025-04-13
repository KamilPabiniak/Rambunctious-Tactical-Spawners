using UnityEngine;

namespace Core
{
    public class TimedMessage
    {
        public readonly string Text;
        public readonly float TimeAdded;

        public TimedMessage(string text)
        {
            Text = text;
            TimeAdded = Time.time;
        }
    }
}