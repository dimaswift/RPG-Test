using System.Collections;
using RPG.Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UnityImplementation
{
	public class HeroInfoTrigger : View, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerExitHandler
	{
		float _tapTime;
		Coroutine _waitForShowInfoRoutine;
		PointerEventData _pointer;

		HeroData _data;
		
		public void SetHeroData(HeroData heroData)
		{
			_data = heroData;
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
			Game.HeroInfoPanel.transform.position = _pointer.position;
			Game.HeroInfoPanel.SetUp(_data);
			Game.HeroInfoPanel.Show();
			StopWaitForShowInfo();
		}

		public void OnDrag(PointerEventData eventData)
		{
			_pointer = eventData;
		}

		public bool IsHeroInfoPanelActive()
		{
			return Game.HeroInfoPanel.gameObject.activeSelf;
		}

		public void HideHeroInfo()
		{
			Game.HeroInfoPanel.Hide();
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			StopWaitForShowInfo();
			Game.HeroInfoPanel.Hide();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			StopWaitForShowInfo();
			Game.HeroInfoPanel.Hide();
		}

		public override void Render()
		{
			
		}
	}

}
