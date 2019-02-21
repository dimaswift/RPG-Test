using System.Collections;
using RPG.Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UnityImplementation
{
	public class HeroInfoTrigger : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerExitHandler
	{
		HeroInfoPanel _infoPanel;
		float _tapTime;
		Coroutine _waitForShowInfoRoutine;
		PointerEventData _pointer;

		HeroData _data;
		
		public void SetHeroData(HeroData heroData)
		{
			_data = heroData;
		}

		void Awake()
		{
			_infoPanel = FindObjectOfType<HeroInfoPanel>();
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if(_data == null)
				return;
			_pointer = eventData;
			StopWaitForShowInfo();
			_waitForShowInfoRoutine = StartCoroutine(WaitForShowInfo());
		}
		
		void StopWaitForShowInfo()
		{
			if (_waitForShowInfoRoutine != null)
			{
				StopCoroutine(_waitForShowInfoRoutine);
				_waitForShowInfoRoutine = null;
			}
		}
        
		IEnumerator WaitForShowInfo()
		{
			yield return new WaitForSeconds(1);
			_infoPanel.transform.position = _pointer.position;
			_infoPanel.SetUp(_data);
			_infoPanel.Show();
			StopWaitForShowInfo();
		}

		public void OnDrag(PointerEventData eventData)
		{
			_pointer = eventData;
		}

		public bool IsHeroInfoPanelActive()
		{
			return _infoPanel.gameObject.activeSelf;
		}

		public void HideHeroInfo()
		{
			_infoPanel.Hide();
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			StopWaitForShowInfo();
			_infoPanel.Hide();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			StopWaitForShowInfo();
			_infoPanel.Hide();
		}
	}

}
