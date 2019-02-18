using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Utils
{
	public class Pool<T> where T : IPoolable
	{
		readonly Func<T> _instantiationHandler;

		readonly List<T> _instances;
		
		public Pool(int capacity, Func<T> instantiationHandler)
		{
			_instantiationHandler = instantiationHandler;
			_instances = new List<T>(capacity);

			for (int i = 0; i < capacity; i++)
			{
				AddInstance();
			}
		}

		T AddInstance()
		{
			var instance = _instantiationHandler();
			instance.Init();
			instance.ReturnToPool();
			_instances.Add(instance);
			return instance;
		}

		public T Pick()
		{
			foreach (var instance in _instances)
			{
				if (!instance.IsActive)
				{
					instance.PickFromPool();
					return instance;
				}
			}

			var newInstance = AddInstance();
			newInstance.PickFromPool();
			return newInstance;

		}
	}
}
