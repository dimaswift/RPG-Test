using UnityEngine;
using UnityEngine.UI;

namespace RPG.View
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] Image _bar;

        public void SetHp(float normalizedValue)
        {
            _bar.fillAmount = normalizedValue;
        }
    }
}