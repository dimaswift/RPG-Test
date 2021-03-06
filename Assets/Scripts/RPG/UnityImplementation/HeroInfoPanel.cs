﻿using System.Collections;
using RPG.Model;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UnityImplementation
{
	public class HeroInfoPanel : View
	{
		[SerializeField] Text _nameText;
		[SerializeField] Text _levelText;
		[SerializeField] Text _hpText;
		[SerializeField] Text _experienceText;
		[SerializeField] Text _attackText;
		[SerializeField] CanvasGroup _canvasGroup;
		
		HeroData _data;
		
		Coroutine _currentAnimation;
		RectTransform _rect;
		protected override void OnInit(Game game)
		{
			base.OnInit(game);
			gameObject.SetActive(false);
			_rect = GetComponent<RectTransform>();
			
		}

		public void SetUp(HeroData data)
		{
			_data = data;
			Render();
		}

		public override void Hide()
		{
			if(!gameObject.activeSelf)
				return;

			_currentAnimation = StartCoroutine(ShowingAnimation(false));
		}

		public void Show(Vector3 position)
		{
			var viewportSize = new Vector2(_rect.rect.width, _rect.rect.height) * transform.parent.localScale.x;
			if (position.x > Screen.width / 2)
				position.x -= viewportSize.x * .75f;
			else position.x += viewportSize.x  * .75f;
			transform.position = position;
			Show();
		}
		
		public override void Show()
		{
			gameObject.SetActive(true);
			if(_currentAnimation != null)
				StopCoroutine(_currentAnimation);
		
			_currentAnimation = StartCoroutine(ShowingAnimation(true));
		}
		
		IEnumerator ShowingAnimation(bool show)
		{
			var t = 0f;
			while (t <= 1)
			{
				t += Time.deltaTime / .5f;
				_canvasGroup.alpha = Mathf.Lerp(show ? 0 : 1, show ? 1 : 0, t);
				yield return null;
			}

			if (!show)
			{
				gameObject.SetActive(false);
			}

			_currentAnimation = null;
			_canvasGroup.alpha = 1;
		}

		public override void Render()
		{
			if(_data == null)
				return;
			_nameText.text = string.Format("Name: {0}",_data.Name);
			_levelText.text = string.Format("Level: {0}",_data.Level + 1);
			_experienceText.text = string.Format("Epx: {0}",_data.Experience);
			_attackText.text = string.Format("Attack: {0}",_data.Attack);
			_hpText.text = string.Format("Hp: {0}", _data.Hp);
		}
	}

}
