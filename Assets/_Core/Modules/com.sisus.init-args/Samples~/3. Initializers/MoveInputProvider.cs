﻿using System;
using UnityEngine;

namespace Sisus.Init.Demos.Initializers
{
	/// <summary>
	/// Raises the <see cref="MoveInputChanged"/> event whenever move input changes.
	/// </summary>
	[Service(ResourcePath = "InitArgs.Demos.Initializers/MoveInputProvider", LazyInit = true)]
	public class MoveInputProvider : MonoBehaviour
	{
		public event Action<Vector2> MoveInputChanged;

		[SerializeField] KeyCode moveUpKey = KeyCode.UpArrow;
		[SerializeField] KeyCode moveLeftKey = KeyCode.LeftArrow;
		[SerializeField] KeyCode moveDownKey = KeyCode.DownArrow;
		[SerializeField] KeyCode moveRightKey = KeyCode.RightArrow;

		Vector2 previousMoveInput = Vector2.zero;

		public void Update()
		{
			Vector2 moveInput = GetMoveInput();
			if(previousMoveInput == moveInput)
			{
				return; 
			}

			previousMoveInput = moveInput;
			MoveInputChanged?.Invoke(moveInput);
			
			Vector2 GetMoveInput()
			{
				var input = Vector2.zero;
				input.x += Input.GetKey(moveLeftKey) ? -1f : Input.GetKey(moveRightKey) ? 1f : 0f;
				input.y += Input.GetKey(moveDownKey) ? -1f : Input.GetKey(moveUpKey) ? 1f : 0f;
				return input;
			}
		}
	}
}