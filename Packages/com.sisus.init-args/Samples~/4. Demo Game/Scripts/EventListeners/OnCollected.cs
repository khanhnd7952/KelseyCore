using UnityEngine;
using UnityEngine.Events;

namespace Init.Demo
{
	/// <summary>
	/// Component that invokes an <see cref="UnityEvent"/> whenever the
	/// <see cref="ICollectedEvent"/> event is triggered.
	/// </summary>
	[AddComponentMenu("Initialization/Demo/Events/On Collected")]
	public sealed class OnCollected : OnEvent<ICollectedEvent> { }
}