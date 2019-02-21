using RPG.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UnityImplementation
{
	[RequireComponent(typeof(Text))]
	public class TextDrop : MonoBehaviour, IPoolable
	{
		Text _text;
		float _timeLeft;
		float _currentDuration;
		float _moveSpeed;
		Color _color;

		public void Drop(string message, Color color, Vector3 position, float duration, float speed)
		{
			_color = color;
			_text.text = message;
			transform.position = position;
			_timeLeft = duration;
			_currentDuration = duration;
			_moveSpeed = speed;
		}
		
		public void Init()
		{
			_text = GetComponent<Text>();
		}

		public void PickFromPool()
		{
			gameObject.SetActive(true);
		}

		public void ReturnToPool()
		{
			gameObject.SetActive(false);
		}

		public bool IsActive
		{
			get { return gameObject.activeSelf; }
		}

		public void Process()
		{
			if(_timeLeft <= 0)
				return;
			_timeLeft -= Time.deltaTime;
			transform.Translate(Vector3.up * Time.deltaTime * _moveSpeed);
			_color.a = Mathf.Lerp(1, 0, 1 - (_timeLeft / _currentDuration));
			_text.color = _color;
			if(_timeLeft <= 0)
				ReturnToPool();
		}

	}

}
