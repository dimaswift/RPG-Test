using System.Collections;
using System.Collections.Generic;
using RPG.Model;
using RPG.View;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.View
{
	public class HeroInfoPanel : View
	{
		[SerializeField] Text _nameText;
		[SerializeField] Text _levelText;
		[SerializeField] Text _hpText;
		[SerializeField] Text _experienceText;
		[SerializeField] Text _attackText;
		
		HeroState _state;
	
		public void SetUp(HeroState state)
		{
			_state = state;
			Render();
		}

		public override void Render()
		{
			if(_state == null)
				return;
			_nameText.text = _state.Name;
			_levelText.text = _state.Level.ToString();
			_experienceText.text = _state.Experience.ToString();
			_attackText.text = _state.Attributes.Attack.ToString("0.00");
			_hpText.text = _state.Attributes.Hp.ToString("0.00");
		}
	}

}
