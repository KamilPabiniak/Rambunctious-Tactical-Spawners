using UnityEngine;

namespace UI
{
    public class ItIsWhatItIs : MonoBehaviour
    {
        private const string URL = "https://youtu.be/dQw4w9WgXcQ?si=6wFPhQ_0zdPAGQue";

        public void OpenLink()
        {
            Application.OpenURL(URL);
        }
    }
}
