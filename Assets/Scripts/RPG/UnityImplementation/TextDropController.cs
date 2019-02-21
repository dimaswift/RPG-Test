using RPG.Utils;
using UnityEngine;

namespace RPG.UnityImplementation
{
	public class TextDropController : MonoBehaviour
	{
		[SerializeField] int _poolCapacity = 10;
		[SerializeField] float _moveSpeed = 3;
		[SerializeField] float _duration = 1;
		[SerializeField] TextDrop _textDropPrefab;

		Pool<TextDrop> _textDropPool;
		
		void Awake()
		{
			_textDropPool = new Pool<TextDrop>(_poolCapacity, CreateInstance);
			_textDropPrefab.gameObject.SetActive(false);
		}

		TextDrop CreateInstance()
		{
			var drop = Instantiate(_textDropPrefab);
			drop.transform.SetParent(transform);
			return drop;
		}
		
		public void DropText(string msg, Vector3 position, Color color)
		{
			var text = _textDropPool.Pick();
			text.Drop(msg, color, position, _duration, _moveSpeed);
		}

		void Update()
		{
			foreach (var instance in _textDropPool.GetInstances())
			{
				instance.Process();
			}
		}
	}
}

