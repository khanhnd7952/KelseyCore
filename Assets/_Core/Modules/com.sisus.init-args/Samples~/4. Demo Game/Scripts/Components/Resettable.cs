using Sisus.Init;
using UnityEngine;
using UnityEngine.Events;

namespace Init.Demo
{
	/// <summary>
	/// Component that invokes a <see cref="UnityEvent"/> whenever the game is reset.
	/// </summary>
	[AddComponentMenu("Initialization/Demo/Resettable")]
	public sealed class Resettable : MonoBehaviour<UnityEvent>, IResettable
	{
		[SerializeField, Tooltip("Event invoked whenever the game is reset")]
		private UnityEvent reset = new UnityEvent();

		/// <inheritdoc/>
		protected override void Init(UnityEvent reset) => this.reset = reset;

		/// <inheritdoc/>
		void IResettable.ResetState() => reset.Invoke();
	}
}
