using UnityEngine;
using UnityEngine.Events;

namespace Init.Demo
{
	/// <summary>
	/// Component that invokes an <see cref="UnityEvent"/> whenever the
	/// <see cref="IPlayerKilledEvent"/> event is triggered.
	/// </summary>
	[AddComponentMenu("Initialization/Demo/Events/On Player Killed")]
	public sealed class OnPlayerKilled : OnEvent<IPlayerKilledEvent> { }
}