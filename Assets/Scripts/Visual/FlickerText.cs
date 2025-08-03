using System.Collections;
using TMPro;
using UnityEngine;

namespace Visual
{
    public class FlickerText : MonoBehaviour
    {
        public TMP_Text tmpText;
        [SerializeField] private float first = 0.2f;
        [SerializeField] private float second = 0.1f;

        private readonly Color _originalColor = Color.white;

        private void OnEnable()
        {
            StartCoroutine(nameof(Flick));
        }

        private void OnDisable()
        {
            StopCoroutine(nameof(Flick));
        }

        private IEnumerator Flick()
        {
            yield return new WaitForSeconds(first);
            tmpText.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 255);
            yield return new WaitForSeconds(second);
            tmpText.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0);
            yield return new WaitForSeconds(second);
            tmpText.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 255);
            yield return new WaitForSeconds(second);
            tmpText.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0);
            StartCoroutine(Flick());
        }
    }
}