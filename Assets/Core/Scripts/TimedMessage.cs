using UnityEngine;

namespace Core
{
    public class TimedMessage
    {
        public string Text;
        public float TimeAdded;

        public TimedMessage(string text)
        {
            Text = text;
            TimeAdded = Time.time;
        }
    }
}