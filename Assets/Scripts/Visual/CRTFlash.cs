using UnityEngine;

namespace Visual
{
    public class CRTFlash : MonoBehaviour
    {
        [SerializeField] private GameObject lineU;
        [SerializeField] private GameObject lineD;
        private bool _half;
        private void FixedUpdate()
        {
            lineU.SetActive(_half);
            lineD.SetActive(!_half);
            _half = !_half;
        }
    }
}