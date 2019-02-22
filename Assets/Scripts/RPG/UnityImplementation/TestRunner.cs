using System;
using RPG.UnitTests;
using UnityEngine;

namespace RPG.UnityImplementation
{
	public class TestRunner : MonoBehaviour 
	{

		void Start ()
		{
			Test();
		}
		
		class UnityTestLogger : ITestLogger
		{
			public void Log(string message)
			{
				Debug.Log(message);
			}
		}
        
		void Test()
		{
			var test = new GameTest(new UnityTestLogger());
			
		}
	
	}

}
