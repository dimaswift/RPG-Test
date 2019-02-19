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
			_nameText.text = string.Format("Name: {0}",_state.Name);
			_levelText.text = string.Format("Level: {0}",_state.Level + 1);
			_experienceText.text = string.Format("Epx: {0}",_state.Experience);
			_attackText.text = string.Format("Attack: {0}",_state.Attributes.Attack);
			_hpText.text = string.Format("Hp: {0}",_state.Attributes.Hp);
		}
	}

}
