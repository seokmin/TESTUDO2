using UnityEngine;

namespace RpgMaker.GameStage
{
	public abstract class GameStage : MonoBehaviour
	{
		public class StageContext
		{
			public System.Type _prevStage = null;
		}

		/// <summary>
		/// This function called only once when app starts
		/// </summary>
		public virtual void OnStageInitialize()
		{
			// initialize
		}

		/// <summary>
		/// Override this method to compose stage with components
		/// </summary>
		/// <param name="context">Previous stage passes current context that explains why and how next stage loads</param>
		public virtual void OnStageEnter(StageContext context)
		{
			// enter
		}

		/// <summary>
		/// This method substitutes MonoBehavior's Update() method
		/// </summary>
		/// <param name="deltaTime"></param>
		public virtual void OnStageUpdate(float deltaTime)
		{
			// update
		}

		/// <summary>
		/// Override this method to clean up contexts of current stage and disposable objects
		/// </summary>
		public virtual void OnStageExit()
		{
			// exit
		}

		public System.Type NextStageType = null;
		public StageContext Context = null;

		protected void goToNextStage(System.Type nextStage, StageContext context)
		{
			NextStageType = nextStage;
			Context = context;
		}
	}
}