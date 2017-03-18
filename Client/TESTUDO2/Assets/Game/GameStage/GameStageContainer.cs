using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RpgMaker.GameStage
{
	public class GameStageContainer : MonoBehaviour
	{
		[SerializeField]
		private Dictionary<System.Type, GameStage> _stages = null;

		[SerializeField]
		private GameStage _currentStage = null;

		public void Start()
		{
			var _stages = (	from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
							from assemblyType in domainAssembly.GetTypes()
							where assemblyType.IsSubclassOf(typeof(GameStage))
							select gameObject.AddComponent(assemblyType) as GameStage )
							.ToDictionary(_ => _.GetType());

			foreach (var kvp in _stages)
			{
				var stage = kvp.Value;
				stage.OnStageInitialize();
			}
		}

		public void Update()
		{
			if (_currentStage.NextStageType != null)
			{
				_currentStage.OnStageExit();
				var nextStage = _stages[_currentStage.NextStageType];

				var context = _currentStage.Context;
				if (context != null)
					context._prevStage = _currentStage.GetType();

				nextStage.OnStageEnter(context);
				_currentStage = nextStage;
			}

			_currentStage.OnStageUpdate(Time.deltaTime);
		}
	}
}