using TMPro;
using UnityEngine;

namespace Kelsey.UGUI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextVersion : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<TextMeshProUGUI>().SetText("Version " +Application.version);
        }
    }
}