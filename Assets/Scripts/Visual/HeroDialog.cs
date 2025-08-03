using TMPro;
using UnityEngine;

namespace Visual
{
    public class HeroDialog : MonoBehaviour
    {
        [SerializeField] private TMP_Text dialog;

        private string[] _dialogs =
        {
            "I will..",
            "Okay...",
            "It should...",
            ""
        };
    }
}