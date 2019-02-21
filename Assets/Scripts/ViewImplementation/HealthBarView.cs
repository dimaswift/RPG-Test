using UnityEngine;

namespace ViewImplementation
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] RectTransform _bar;

        public void SetHp(float normalizedValue)
        {
            var scale = _bar.localScale;
            scale.x = normalizedValue;
            _bar.localScale = scale;
        }
    }
}