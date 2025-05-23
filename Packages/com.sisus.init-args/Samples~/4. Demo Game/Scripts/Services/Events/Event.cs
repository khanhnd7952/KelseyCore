#if DEBUG
using System.Text;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace Init.Demo
{
	/// <summary>
	/// A <see cref="ScriptableObject"/> asset that represents an event.
	/// <para>
	/// Whenever the event is <see cref="Trigger">triggered</see> all
	/// methods that have subscribed to receive a callback are executed.
	/// </para>
	/// </summary>
	public abstract class Event : ScriptableObject, IEvent, IEventTrigger
	{
		protected const string CreateAssetMenuDirectory = "Init(args) Demo/Events/";

		private UnityAction listeners = null;

		#if DEBUG
		[SerializeField]
		private bool debugTrigger = false;
		#endif

		/// <inheritdoc/>
		public void AddListener(UnityAction method) => listeners += method;

		/// <inheritdoc/>
		public void RemoveListener(UnityAction method) => listeners -= method;

		/// <inheritdoc/>
		public void Trigger()
		{
			#if DEBUG
			if(debugTrigger) Debug.Log(GenerateOnTriggeredDebugMessage(), this);
			#endif

			listeners?.Invoke();
		}

		#if DEBUG
		private string GenerateOnTriggeredDebugMessage()
		{
			if(listeners is null)
			{
				return name + " Triggered with no listeners.";
			}

			var invocationList = listeners.GetInvocationList();
			var sb = new StringBuilder();
			sb.Append(name);
			sb.Append(" Triggered with ");
			sb.Append(invocationList.Length);
			sb.Append(" listeners.");

			foreach(var invocation in invocationList)
			{
				if(invocation.Method is null)
				{
					continue;
				}

				sb.AppendLine();

				if(invocation.Method.DeclaringType != null)
				{
					sb.Append(invocation.Method.DeclaringType.Name);
					sb.Append(".");
				}

				sb.Append(invocation.Method.Name);
			}

			return sb.ToString();
		}
		#endif
	}
}