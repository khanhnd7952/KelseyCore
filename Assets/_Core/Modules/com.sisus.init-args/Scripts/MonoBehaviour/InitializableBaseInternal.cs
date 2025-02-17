﻿//#define DEBUG_DEFERRED_ON_AWAKE

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Sisus.Init.Internal.InitializerUtility;
using Debug = UnityEngine.Debug;
#if UNITY_EDITOR
using Sisus.Init.EditorOnly;
#endif

namespace Sisus.Init.Internal
{
	/// <summary>
	/// Base class for all MonoBehaviour{T...} classes.
	/// </summary>
	public abstract class InitializableBaseInternal : MonoBehaviour, IInitializable
	#if UNITY_EDITOR
		, IInitializableEditorOnly
	#endif
	{
		internal InitState initState;
		
		bool IInitializable.HasInitializer => HasInitializer(this);
		bool IInitializable.Init(Context context) => InitInternal(context);
		
		/// <summary>
		/// A value against which any <see cref="object"/> can be compared to determine whether or not it is
		/// <see langword="null"/> or an <see cref="UnityEngine.Object"/> which has been <see cref="UnityEngine.Object.Destroy">destroyed</see>.
		/// </summary>
		/// <example>
		/// <code>
		/// private IEvent trigger;
		/// 
		///	private void OnDisable()
		///	{
		///		if(trigger != Null)
		///		{
		///			trigger.RemoveListener(this);
		///		}
		///	}
		/// </code>
		/// </example>
		[NotNull]
		protected static NullExtensions.NullComparer Null => NullExtensions.Null;

		/// <summary>
		/// A value against which any <see cref="object"/> can be compared to determine whether or not it is
		/// <see langword="null"/> or an <see cref="UnityEngine.Object"/> which is <see cref="GameObject.activeInHierarchy">inactive</see>
		/// or has been <see cref="UnityEngine.Object.Destroy(UnityEngine.Object)">destroyed</see>.
		/// </summary>
		/// <example>
		/// <code>
		/// private ITrackable target;
		/// 
		/// private void Update()
		/// {
		/// 	if(target != NullOrInactive)
		/// 	{
		/// 		transform.LookAt(target.Position);
		/// 	}
		/// }
		/// </code>
		/// </example>
		[NotNull]
		protected static NullExtensions.NullOrInactiveComparer NullOrInactive => NullExtensions.NullOrInactive;

		/// <summary>
		/// <see cref="OnAwake"/> is called when the script instance is being loaded during the <see cref="Awake"/> event after the <see cref="InitInternal"/> function has finished.
		/// <para>
		/// <see cref="OnAwake"/> is called either when an active <see cref="GameObject"/> that contains the script is initialized when a <see cref="UnityEngine.SceneManagement.Scene">Scene</see> loads,
		/// or when a previously <see cref="GameObject.activeInHierarchy">inactive</see> <see cref="GameObject"/> is set active, or after a <see cref="GameObject"/> created with <see cref="UnityEngine.Object.Instantiate"/>
		/// is initialized.
		/// </para>
		/// <para>
		/// Unity calls <see cref="OnAwake"/> only once during the lifetime of the script instance. A script's lifetime lasts until the Scene that contains it is unloaded.
		/// If the Scene is loaded again, Unity loads the script instance again, so <see cref="OnAwake"/> will be called again.
		/// If the Scene is loaded multiple times additively, Unity loads several script instances, so <see cref="OnAwake"/> will be called several times (once on each instance).
		/// </para>
		/// <para>
		/// For active <see cref="GameObject">GameObjects</see> placed in a Scene, Unity calls <see cref="OnAwake"/> after all active <see cref="GameObject">GameObjects</see>
		/// in the Scene are initialized, so you can safely use methods such as <see cref="GameObject.FindWithTag"/> to query other <see cref="GameObject">GameObjects</see>.
		/// </para>
		/// <para>
		/// The order that Unity calls each <see cref="GameObject"/>'s Awake (and by extension <see cref="OnAwake"/>) is not deterministic.
		/// Because of this, you should not rely on one <see cref="GameObject"/>'s Awake being called before or after another
		/// (for example, you should not assume that a reference assigned by one GameObject's <see cref="OnAwake"/> will be usable in another GameObject's <see cref="Awake"/>).
		/// Instead, you should use <see cref="Awake"/>/<see cref="OnAwake"/> to set up references between scripts, and use Start, which is called after all <see cref="Awake"/>
		/// and <see cref="OnAwake"/> calls are finished, to pass any information back and forth.
		/// </para>
		/// <para>
		/// <see cref="OnAwake"/> is always called before any Start functions. This allows you to order initialization of scripts.
		/// <see cref="OnAwake"/> is called even if the script is a disabled component of an active GameObject.
		/// <see cref="OnAwake"/> can not act as a coroutine.
		/// </para>
		/// <para>
		/// Note: Use <see cref="OnAwake"/> instead of the constructor for initialization, as the serialized state of the <see cref="Component"/> is undefined at construction time.
		/// </para>
		/// </summary>
		protected virtual void OnAwake() { }
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private protected bool InitInternal(Context context)
		{
			if(initState != InitState.Uninitialized)
			{
				return true;
			}

			initState = InitState.Initializing;
			bool success = false;
			
			#if DEBUG || INIT_ARGS_SAFE_MODE
			try {
			#endif
			
			success = Init(context);
			
			#if DEBUG || INIT_ARGS_SAFE_MODE
			} finally {
			#endif
			
			initState = success ? InitState.Initialized : InitState.Uninitialized;
			
			#if DEBUG || INIT_ARGS_SAFE_MODE
			}
			#endif
			
			return success;
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract bool Init(Context context);
		
		internal void Awake()
		{
			if(initState is InitState.Uninitialized)
			{
				#if UNITY_EDITOR
				bool isInitialized =
				#endif
					InitInternal(Context.Awake);

				#if UNITY_EDITOR
				if(!isInitialized && ShouldSelfGuardAgainstNull(this))
				{
					throw new MissingInitArgumentsException(this);
				}
				#endif
			}
			else if(initState is InitState.Initializing)
			{
				#if DEV_MODE && DEBUG_DEFERRED_ON_AWAKE
				Debug.Log($"{GetType().Name} - won't execute OnAwake yet because initialization is still in progress. Expecting Initializer to do it manually once it is safe to do so.");
				#endif
				return;
			}

			OnAwake();
		}
		
		/// <summary>
		/// Checks if the <paramref name="argument"/> is <see langword="null"/> and throws an <see cref="System.ArgumentNullException"/> if it is.
		/// <para>
		/// This method call is ignored in non-development builds.
		/// </para>
		/// </summary>
		/// <param name="argument"> The argument to test. </param>
		[Conditional("DEBUG"), Conditional("INIT_ARGS_SAFE_MODE"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void ThrowIfNull<TArgument>(TArgument argument)
		{
			#if DEBUG || INIT_ARGS_SAFE_MODE
			if(argument == Null) throw new ArgumentNullException(typeof(TArgument).Name, $"Init argument of type {typeof(TArgument).Name} passed to {GetType().Name} was null.");
			#endif
		}

		/// <summary>
		/// Checks if the <paramref name="argument"/> is <see langword="null"/> and logs an assertion message to the console if it is.
		/// <para>
		/// This method call is ignored in non-development builds.
		/// </para>
		/// </summary>
		/// <param name="argument"> The argument to test. </param>
		[Conditional("DEBUG"), Conditional("INIT_ARGS_SAFE_MODE"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void AssertNotNull<TArgument>(TArgument argument)
		{
			#if DEBUG || INIT_ARGS_SAFE_MODE
			if(argument == Null) Debug.LogAssertion($"Init argument of type {typeof(TArgument).Name} passed to {GetType().Name} was null.", this);
			#endif
		}

		/// <summary>
		/// Method that can be overridden and used to validate the initialization arguments that were received by this object.
		/// <para>
		/// You can use the <see cref="ThrowIfNull{T}"/> method to throw an <see cref="ArgumentNullException"/>
		/// if an argument is <see cref="Null">null</see>.
		/// <example>
		/// <code>
		/// protected override void ValidateArgument(IInputManager inputManager)
		/// {
		///		ThrowIfNull(inputManager);
		/// }
		/// </code>
		/// </example>
		/// </para>
		/// <para>
		/// You can use the <see cref="AssertNotNull{TArgument}"/> method to log an assertion to the Console
		/// if an argument is <see cref="Null">null</see>.
		/// <example>
		/// <code>
		/// protected override void ValidateArguments(IInputManager inputManager)
		/// {
		///		AssertNotNull(inputManager);
		/// }
		/// </code>
		/// </example>
		/// </para>
		/// <para>
		/// Calls to this method are ignored in non-development builds.
		/// </para>
		/// </summary>
		/// <param name="argument"> The received argument to validate. </param>
		[Conditional("DEBUG"), Conditional("INIT_ARGS_SAFE_MODE"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual void ValidateArgument<TArgument>(TArgument argument)
		{
			#if DEBUG || INIT_ARGS_SAFE_MODE
			AssertNotNull(argument);
			#endif
		}

		/// <summary>
		/// Method that can be overridden and used to validate the initialization arguments that were received by this object.
		/// <para>
		/// You can use the <see cref="ThrowIfNull{TArgument}"/> method to throw an <see cref="ArgumentNullException"/>
		/// if an argument is <see cref="Null">null</see>.
		/// <example>
		/// <code>
		/// protected override void ValidateArguments(IInputManager inputManager, Camera camera)
		/// {
		///		ThrowIfNull(inputManager);
		///		ThrowIfNull(camera);
		/// }
		/// </code>
		/// </example>
		/// </para>
		/// <para>
		/// You can use the <see cref="AssertNotNull{TArgument}"/> method to log an assertion to the Console
		/// if an argument is <see cref="Null">null</see>.
		/// <example>
		/// <code>
		/// protected override void ValidateArguments(IInputManager inputManager, Camera camera)
		/// {
		///		AssertNotNull(inputManager);
		///		AssertNotNull(camera);
		/// }
		/// </code>
		/// </example>
		/// </para>
		/// <para>
		/// Calls to this method are ignored in non-development builds.
		/// </para>
		/// </summary>
		/// <param name="firstArgument"> The first received argument to validate. </param>
		/// <param name="secondArgument"> The second received argument to validate. </param>
		[Conditional("DEBUG"), Conditional("INIT_ARGS_SAFE_MODE"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual void ValidateArguments<TFirstArgument, TSecondArgument>(TFirstArgument firstArgument, TSecondArgument secondArgument)
		{
			#if DEBUG || INIT_ARGS_SAFE_MODE
			AssertNotNull(firstArgument);
			AssertNotNull(secondArgument);
			#endif
		}

		/// <summary>
		/// Method that can be overridden and used to validate the initialization arguments that were received by this object.
		/// <para>
		/// You can use the <see cref="ThrowIfNull{TArgument}"/> method to throw an <see cref="ArgumentNullException"/>
		/// if an argument is <see cref="Null">null</see>.
		/// <example>
		/// <code>
		/// protected override void ValidateArguments(IInputManager inputManager, Camera camera)
		/// {
		///		ThrowIfNull(inputManager);
		///		ThrowIfNull(camera);
		/// }
		/// </code>
		/// </example>
		/// </para>
		/// <para>
		/// You can use the <see cref="AssertNotNull{TArgument}"/> method to log an assertion to the Console
		/// if an argument is <see cref="Null">null</see>.
		/// <example>
		/// <code>
		/// protected override void ValidateArguments(IInputManager inputManager, Camera camera)
		/// {
		///		AssertNotNull(inputManager);
		///		AssertNotNull(camera);
		/// }
		/// </code>
		/// </example>
		/// </para>
		/// <para>
		/// Calls to this method are ignored in non-development builds.
		/// </para>
		/// </summary>
		/// <param name="firstArgument"> The first received argument to validate. </param>
		/// <param name="secondArgument"> The second received argument to validate. </param>
		/// <param name="thirdArgument"> The third received argument to validate. </param>
		[Conditional("DEBUG"), Conditional("INIT_ARGS_SAFE_MODE"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual void ValidateArguments<TFirstArgument, TSecondArgument, TThirdArgument>(TFirstArgument firstArgument, TSecondArgument secondArgument, TThirdArgument thirdArgument)
		{
			#if DEBUG || INIT_ARGS_SAFE_MODE
			AssertNotNull(firstArgument);
			AssertNotNull(secondArgument);
			AssertNotNull(thirdArgument);
			#endif
		}

		/// <summary>
		/// Method that can be overridden and used to validate the initialization arguments that were received by this object.
		/// <para>
		/// You can use the <see cref="ThrowIfNull{TArgument}"/> method to throw an <see cref="ArgumentNullException"/>
		/// if an argument is <see cref="Null">null</see>.
		/// <example>
		/// <code>
		/// protected override void ValidateArguments(IInputManager inputManager, Camera camera)
		/// {
		///		ThrowIfNull(inputManager);
		///		ThrowIfNull(camera);
		/// }
		/// </code>
		/// </example>
		/// </para>
		/// <para>
		/// You can use the <see cref="AssertNotNull{TArgument}"/> method to log an assertion to the Console
		/// if an argument is <see cref="Null">null</see>.
		/// <example>
		/// <code>
		/// protected override void ValidateArguments(IInputManager inputManager, Camera camera)
		/// {
		///		AssertNotNull(inputManager);
		///		AssertNotNull(camera);
		/// }
		/// </code>
		/// </example>
		/// </para>
		/// <para>
		/// Calls to this method are ignored in non-development builds.
		/// </para>
		/// </summary>
		/// <param name="firstArgument"> The first received argument to validate. </param>
		/// <param name="secondArgument"> The second received argument to validate. </param>
		/// <param name="thirdArgument"> The third received argument to validate. </param>
		/// <param name="fourthArgument"> The fourth received argument to validate. </param>
		[Conditional("DEBUG"), Conditional("INIT_ARGS_SAFE_MODE"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual void ValidateArguments<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument>(TFirstArgument firstArgument, TSecondArgument secondArgument, TThirdArgument thirdArgument, TFourthArgument fourthArgument)
		{
			#if DEBUG || INIT_ARGS_SAFE_MODE
			AssertNotNull(firstArgument);
			AssertNotNull(secondArgument);
			AssertNotNull(thirdArgument);
			AssertNotNull(fourthArgument);
			#endif
		}

		/// <summary>
		/// Method that can be overridden and used to validate the initialization arguments that were received by this object.
		/// <para>
		/// You can use the <see cref="ThrowIfNull{TArgument}"/> method to throw an <see cref="ArgumentNullException"/>
		/// if an argument is <see cref="Null">null</see>.
		/// <example>
		/// <code>
		/// protected override void ValidateArguments(IInputManager inputManager, Camera camera)
		/// {
		///		ThrowIfNull(inputManager);
		///		ThrowIfNull(camera);
		/// }
		/// </code>
		/// </example>
		/// </para>
		/// <para>
		/// You can use the <see cref="AssertNotNull{TArgument}"/> method to log an assertion to the Console
		/// if an argument is <see cref="Null">null</see>.
		/// <example>
		/// <code>
		/// protected override void ValidateArguments(IInputManager inputManager, Camera camera)
		/// {
		///		AssertNotNull(inputManager);
		///		AssertNotNull(camera);
		/// }
		/// </code>
		/// </example>
		/// </para>
		/// <para>
		/// Calls to this method are ignored in non-development builds.
		/// </para>
		/// </summary>
		/// <param name="firstArgument"> The first received argument to validate. </param>
		/// <param name="secondArgument"> The second received argument to validate. </param>
		/// <param name="thirdArgument"> The third received argument to validate. </param>
		/// <param name="fourthArgument"> The fourth received argument to validate. </param>
		/// <param name="fifthArgument"> The fifth received argument to validate. </param>
		[Conditional("DEBUG"), Conditional("INIT_ARGS_SAFE_MODE"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual void ValidateArguments<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument>(TFirstArgument firstArgument, TSecondArgument secondArgument, TThirdArgument thirdArgument, TFourthArgument fourthArgument, TFifthArgument fifthArgument)
		{
			#if DEBUG || INIT_ARGS_SAFE_MODE
			AssertNotNull(firstArgument);
			AssertNotNull(secondArgument);
			AssertNotNull(thirdArgument);
			AssertNotNull(fourthArgument);
			AssertNotNull(fifthArgument);
			#endif
		}

		/// <summary>
		/// Method that can be overridden and used to validate the initialization arguments that were received by this object.
		/// <para>
		/// You can use the <see cref="ThrowIfNull{TArgument}"/> method to throw an <see cref="ArgumentNullException"/>
		/// if an argument is <see cref="Null">null</see>.
		/// <example>
		/// <code>
		/// protected override void ValidateArguments(IInputManager inputManager, Camera camera)
		/// {
		///		ThrowIfNull(inputManager);
		///		ThrowIfNull(camera);
		/// }
		/// </code>
		/// </example>
		/// </para>
		/// <para>
		/// You can use the <see cref="AssertNotNull{TArgument}"/> method to log an assertion to the Console
		/// if an argument is <see cref="Null">null</see>.
		/// <example>
		/// <code>
		/// protected override void ValidateArguments(IInputManager inputManager, Camera camera)
		/// {
		///		AssertNotNull(inputManager);
		///		AssertNotNull(camera);
		/// }
		/// </code>
		/// </example>
		/// </para>
		/// <para>
		/// Calls to this method are ignored in non-development builds.
		/// </para>
		/// </summary>
		/// <param name="firstArgument"> The first received argument to validate. </param>
		/// <param name="secondArgument"> The second received argument to validate. </param>
		/// <param name="thirdArgument"> The third received argument to validate. </param>
		/// <param name="fourthArgument"> The fourth received argument to validate. </param>
		/// <param name="fifthArgument"> The fifth received argument to validate. </param>
		/// <param name="sixthArgument"> The sixth received argument to validate. </param>
		[Conditional("DEBUG"), Conditional("INIT_ARGS_SAFE_MODE"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual void ValidateArguments<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument>(TFirstArgument firstArgument, TSecondArgument secondArgument, TThirdArgument thirdArgument, TFourthArgument fourthArgument, TFifthArgument fifthArgument, TSixthArgument sixthArgument)
		{
			#if DEBUG || INIT_ARGS_SAFE_MODE
			AssertNotNull(firstArgument);
			AssertNotNull(secondArgument);
			AssertNotNull(thirdArgument);
			AssertNotNull(fourthArgument);
			AssertNotNull(fifthArgument);
			AssertNotNull(sixthArgument);
			#endif
		}

		/// <summary>
		/// Method that can be overridden and used to validate the initialization arguments that were received by this object.
		/// <para>
		/// You can use the <see cref="ThrowIfNull{TArgument}"/> method to throw an <see cref="ArgumentNullException"/>
		/// if an argument is <see cref="Null">null</see>.
		/// <example>
		/// <code>
		/// protected override void ValidateArguments(IInputManager inputManager, Camera camera)
		/// {
		///		ThrowIfNull(inputManager);
		///		ThrowIfNull(camera);
		/// }
		/// </code>
		/// </example>
		/// </para>
		/// <para>
		/// You can use the <see cref="AssertNotNull{TArgument}"/> method to log an assertion to the Console
		/// if an argument is <see cref="Null">null</see>.
		/// <example>
		/// <code>
		/// protected override void ValidateArguments(IInputManager inputManager, Camera camera)
		/// {
		///		AssertNotNull(inputManager);
		///		AssertNotNull(camera);
		/// }
		/// </code>
		/// </example>
		/// </para>
		/// <para>
		/// Calls to this method are ignored in non-development builds.
		/// </para>
		/// </summary>
		/// <param name="firstArgument"> The first received argument to validate. </param>
		/// <param name="secondArgument"> The second received argument to validate. </param>
		/// <param name="thirdArgument"> The third received argument to validate. </param>
		/// <param name="fourthArgument"> The fourth received argument to validate. </param>
		/// <param name="fifthArgument"> The fifth received argument to validate. </param>
		/// <param name="sixthArgument"> The sixth received argument to validate. </param>
		/// <param name="seventhArgument"> The seventh received argument to validate. </param>
		[Conditional("DEBUG"), Conditional("INIT_ARGS_SAFE_MODE"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual void ValidateArguments<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument>(TFirstArgument firstArgument, TSecondArgument secondArgument, TThirdArgument thirdArgument, TFourthArgument fourthArgument, TFifthArgument fifthArgument, TSixthArgument sixthArgument, TSeventhArgument seventhArgument)
		{
			#if DEBUG || INIT_ARGS_SAFE_MODE
			AssertNotNull(firstArgument);
			AssertNotNull(secondArgument);
			AssertNotNull(thirdArgument);
			AssertNotNull(fourthArgument);
			AssertNotNull(fifthArgument);
			AssertNotNull(sixthArgument);
			AssertNotNull(seventhArgument);
			#endif
		}

		/// <summary>
		/// Method that can be overridden and used to validate the initialization arguments that were received by this object.
		/// <para>
		/// You can use the <see cref="ThrowIfNull{TArgument}"/> method to throw an <see cref="ArgumentNullException"/>
		/// if an argument is <see cref="Null">null</see>.
		/// <example>
		/// <code>
		/// protected override void ValidateArguments(IInputManager inputManager, Camera camera)
		/// {
		///		ThrowIfNull(inputManager);
		///		ThrowIfNull(camera);
		/// }
		/// </code>
		/// </example>
		/// </para>
		/// <para>
		/// You can use the <see cref="AssertNotNull{TArgument}"/> method to log an assertion to the Console
		/// if an argument is <see cref="Null">null</see>.
		/// <example>
		/// <code>
		/// protected override void ValidateArguments(IInputManager inputManager, Camera camera)
		/// {
		///		AssertNotNull(inputManager);
		///		AssertNotNull(camera);
		/// }
		/// </code>
		/// </example>
		/// </para>
		/// <para>
		/// Calls to this method are ignored in non-development builds.
		/// </para>
		/// </summary>
		/// <param name="firstArgument"> The first received argument to validate. </param>
		/// <param name="secondArgument"> The second received argument to validate. </param>
		/// <param name="thirdArgument"> The third received argument to validate. </param>
		/// <param name="fourthArgument"> The fourth received argument to validate. </param>
		/// <param name="fifthArgument"> The fifth received argument to validate. </param>
		/// <param name="sixthArgument"> The sixth received argument to validate. </param>
		/// <param name="seventhArgument"> The seventh received argument to validate. </param>
		/// <param name="eighthArgument"> The eighth received argument to validate. </param>
		[Conditional("DEBUG"), Conditional("INIT_ARGS_SAFE_MODE"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual void ValidateArguments<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument, TEighthArgument>(TFirstArgument firstArgument, TSecondArgument secondArgument, TThirdArgument thirdArgument, TFourthArgument fourthArgument, TFifthArgument fifthArgument, TSixthArgument sixthArgument, TSeventhArgument seventhArgument, TEighthArgument eighthArgument)
		{
			#if DEBUG || INIT_ARGS_SAFE_MODE
			AssertNotNull(firstArgument);
			AssertNotNull(secondArgument);
			AssertNotNull(thirdArgument);
			AssertNotNull(fourthArgument);
			AssertNotNull(fifthArgument);
			AssertNotNull(sixthArgument);
			AssertNotNull(seventhArgument);
			AssertNotNull(eighthArgument);
			#endif
		}
		
		/// <summary>
		/// Method that can be overridden and used to validate the initialization arguments that were received by this object.
		/// <para>
		/// You can use the <see cref="ThrowIfNull{TArgument}"/> method to throw an <see cref="ArgumentNullException"/>
		/// if an argument is <see cref="Null">null</see>.
		/// <example>
		/// <code>
		/// protected override void ValidateArguments(IInputManager inputManager, Camera camera)
		/// {
		///		ThrowIfNull(inputManager);
		///		ThrowIfNull(camera);
		/// }
		/// </code>
		/// </example>
		/// </para>
		/// <para>
		/// You can use the <see cref="AssertNotNull{TArgument}"/> method to log an assertion to the Console
		/// if an argument is <see cref="Null">null</see>.
		/// <example>
		/// <code>
		/// protected override void ValidateArguments(IInputManager inputManager, Camera camera)
		/// {
		///		AssertNotNull(inputManager);
		///		AssertNotNull(camera);
		/// }
		/// </code>
		/// </example>
		/// </para>
		/// <para>
		/// Calls to this method are ignored in non-development builds.
		/// </para>
		/// </summary>
		/// <param name="firstArgument"> The first received argument to validate. </param>
		/// <param name="secondArgument"> The second received argument to validate. </param>
		/// <param name="thirdArgument"> The third received argument to validate. </param>
		/// <param name="fourthArgument"> The fourth received argument to validate. </param>
		/// <param name="fifthArgument"> The fifth received argument to validate. </param>
		/// <param name="sixthArgument"> The sixth received argument to validate. </param>
		/// <param name="seventhArgument"> The seventh received argument to validate. </param>
		/// <param name="eighthArgument"> The eighth received argument to validate. </param>
		/// <param name="ninthArgument"> The ninth received argument to validate. </param>
		[Conditional("DEBUG"), Conditional("INIT_ARGS_SAFE_MODE"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual void ValidateArguments<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument, TEighthArgument, TNinthArgument>(TFirstArgument firstArgument, TSecondArgument secondArgument, TThirdArgument thirdArgument, TFourthArgument fourthArgument, TFifthArgument fifthArgument, TSixthArgument sixthArgument, TSeventhArgument seventhArgument, TEighthArgument eighthArgument, TNinthArgument ninthArgument)
		{
			#if DEBUG || INIT_ARGS_SAFE_MODE
			AssertNotNull(firstArgument);
			AssertNotNull(secondArgument);
			AssertNotNull(thirdArgument);
			AssertNotNull(fourthArgument);
			AssertNotNull(fifthArgument);
			AssertNotNull(sixthArgument);
			AssertNotNull(seventhArgument);
			AssertNotNull(eighthArgument);
			AssertNotNull(ninthArgument);
			#endif
		}
		
		/// <summary>
		/// Method that can be overridden and used to validate the initialization arguments that were received by this object.
		/// <para>
		/// You can use the <see cref="ThrowIfNull{TArgument}"/> method to throw an <see cref="ArgumentNullException"/>
		/// if an argument is <see cref="Null">null</see>.
		/// <example>
		/// <code>
		/// protected override void ValidateArguments(IInputManager inputManager, Camera camera)
		/// {
		///		ThrowIfNull(inputManager);
		///		ThrowIfNull(camera);
		/// }
		/// </code>
		/// </example>
		/// </para>
		/// <para>
		/// You can use the <see cref="AssertNotNull{TArgument}"/> method to log an assertion to the Console
		/// if an argument is <see cref="Null">null</see>.
		/// <example>
		/// <code>
		/// protected override void ValidateArguments(IInputManager inputManager, Camera camera)
		/// {
		///		AssertNotNull(inputManager);
		///		AssertNotNull(camera);
		/// }
		/// </code>
		/// </example>
		/// </para>
		/// <para>
		/// Calls to this method are ignored in non-development builds.
		/// </para>
		/// </summary>
		/// <param name="firstArgument"> The first received argument to validate. </param>
		/// <param name="secondArgument"> The second received argument to validate. </param>
		/// <param name="thirdArgument"> The third received argument to validate. </param>
		/// <param name="fourthArgument"> The fourth received argument to validate. </param>
		/// <param name="fifthArgument"> The fifth received argument to validate. </param>
		/// <param name="sixthArgument"> The sixth received argument to validate. </param>
		/// <param name="seventhArgument"> The seventh received argument to validate. </param>
		/// <param name="eighthArgument"> The eighth received argument to validate. </param>
		/// <param name="ninthArgument"> The ninth received argument to validate. </param>
		/// <param name="tenthArgument"> The tenth received argument to validate. </param>
		[Conditional("DEBUG"), Conditional("INIT_ARGS_SAFE_MODE"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual void ValidateArguments<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument, TEighthArgument, TNinthArgument, TTenthArgument>(TFirstArgument firstArgument, TSecondArgument secondArgument, TThirdArgument thirdArgument, TFourthArgument fourthArgument, TFifthArgument fifthArgument, TSixthArgument sixthArgument, TSeventhArgument seventhArgument, TEighthArgument eighthArgument, TNinthArgument ninthArgument, TTenthArgument tenthArgument)
		{
			#if DEBUG || INIT_ARGS_SAFE_MODE
			AssertNotNull(firstArgument);
			AssertNotNull(secondArgument);
			AssertNotNull(thirdArgument);
			AssertNotNull(fourthArgument);
			AssertNotNull(fifthArgument);
			AssertNotNull(sixthArgument);
			AssertNotNull(seventhArgument);
			AssertNotNull(eighthArgument);
			AssertNotNull(ninthArgument);
			AssertNotNull(tenthArgument);
			#endif
		}
		
		/// <summary>
		/// Method that can be overridden and used to validate the initialization arguments that were received by this object.
		/// <para>
		/// You can use the <see cref="ThrowIfNull{TArgument}"/> method to throw an <see cref="ArgumentNullException"/>
		/// if an argument is <see cref="Null">null</see>.
		/// <example>
		/// <code>
		/// protected override void ValidateArguments(IInputManager inputManager, Camera camera)
		/// {
		///		ThrowIfNull(inputManager);
		///		ThrowIfNull(camera);
		/// }
		/// </code>
		/// </example>
		/// </para>
		/// <para>
		/// You can use the <see cref="AssertNotNull{TArgument}"/> method to log an assertion to the Console
		/// if an argument is <see cref="Null">null</see>.
		/// <example>
		/// <code>
		/// protected override void ValidateArguments(IInputManager inputManager, Camera camera)
		/// {
		///		AssertNotNull(inputManager);
		///		AssertNotNull(camera);
		/// }
		/// </code>
		/// </example>
		/// </para>
		/// <para>
		/// Calls to this method are ignored in non-development builds.
		/// </para>
		/// </summary>
		/// <param name="firstArgument"> The first received argument to validate. </param>
		/// <param name="secondArgument"> The second received argument to validate. </param>
		/// <param name="thirdArgument"> The third received argument to validate. </param>
		/// <param name="fourthArgument"> The fourth received argument to validate. </param>
		/// <param name="fifthArgument"> The fifth received argument to validate. </param>
		/// <param name="sixthArgument"> The sixth received argument to validate. </param>
		/// <param name="seventhArgument"> The seventh received argument to validate. </param>
		/// <param name="eighthArgument"> The eighth received argument to validate. </param>
		/// <param name="ninthArgument"> The ninth received argument to validate. </param>
		/// <param name="tenthArgument"> The tenth received argument to validate. </param>
		/// <param name="eleventhArgument"> The eleventh received argument to validate. </param>
		[Conditional("DEBUG"), Conditional("INIT_ARGS_SAFE_MODE"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual void ValidateArguments<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument, TEighthArgument, TNinthArgument, TTenthArgument, TEleventhArgument>(TFirstArgument firstArgument, TSecondArgument secondArgument, TThirdArgument thirdArgument, TFourthArgument fourthArgument, TFifthArgument fifthArgument, TSixthArgument sixthArgument, TSeventhArgument seventhArgument, TEighthArgument eighthArgument, TNinthArgument ninthArgument, TTenthArgument tenthArgument, TEleventhArgument eleventhArgument)
		{
			#if DEBUG || INIT_ARGS_SAFE_MODE
			AssertNotNull(firstArgument);
			AssertNotNull(secondArgument);
			AssertNotNull(thirdArgument);
			AssertNotNull(fourthArgument);
			AssertNotNull(fifthArgument);
			AssertNotNull(sixthArgument);
			AssertNotNull(seventhArgument);
			AssertNotNull(eighthArgument);
			AssertNotNull(ninthArgument);
			AssertNotNull(tenthArgument);
			AssertNotNull(eleventhArgument);
			#endif
		}

		/// <summary>
		/// Method that can be overridden and used to validate the initialization arguments that were received by this object.
		/// <para>
		/// You can use the <see cref="ThrowIfNull{TArgument}"/> method to throw an <see cref="ArgumentNullException"/>
		/// if an argument is <see cref="Null">null</see>.
		/// <example>
		/// <code>
		/// protected override void ValidateArguments(IInputManager inputManager, Camera camera)
		/// {
		///		ThrowIfNull(inputManager);
		///		ThrowIfNull(camera);
		/// }
		/// </code>
		/// </example>
		/// </para>
		/// <para>
		/// You can use the <see cref="AssertNotNull{TArgument}"/> method to log an assertion to the Console
		/// if an argument is <see cref="Null">null</see>.
		/// <example>
		/// <code>
		/// protected override void ValidateArguments(IInputManager inputManager, Camera camera)
		/// {
		///		AssertNotNull(inputManager);
		///		AssertNotNull(camera);
		/// }
		/// </code>
		/// </example>
		/// </para>
		/// <para>
		/// Calls to this method are ignored in non-development builds.
		/// </para>
		/// </summary>
		/// <param name="firstArgument"> The first received argument to validate. </param>
		/// <param name="secondArgument"> The second received argument to validate. </param>
		/// <param name="thirdArgument"> The third received argument to validate. </param>
		/// <param name="fourthArgument"> The fourth received argument to validate. </param>
		/// <param name="fifthArgument"> The fifth received argument to validate. </param>
		/// <param name="sixthArgument"> The sixth received argument to validate. </param>
		/// <param name="seventhArgument"> The seventh received argument to validate. </param>
		/// <param name="eighthArgument"> The eighth received argument to validate. </param>
		/// <param name="ninthArgument"> The ninth received argument to validate. </param>
		/// <param name="tenthArgument"> The tenth received argument to validate. </param>
		/// <param name="eleventhArgument"> The eleventh received argument to validate. </param>
		/// <param name="twelfthArgument"> The twelfth received argument to validate. </param>
		[Conditional("DEBUG"), Conditional("INIT_ARGS_SAFE_MODE"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual void ValidateArguments<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument, TEighthArgument, TNinthArgument, TTenthArgument, TEleventhArgument, TTwelfthArgument>(TFirstArgument firstArgument, TSecondArgument secondArgument, TThirdArgument thirdArgument, TFourthArgument fourthArgument, TFifthArgument fifthArgument, TSixthArgument sixthArgument, TSeventhArgument seventhArgument, TEighthArgument eighthArgument, TNinthArgument ninthArgument, TTenthArgument tenthArgument, TEleventhArgument eleventhArgument, TTwelfthArgument twelfthArgument)
		{
			#if DEBUG || INIT_ARGS_SAFE_MODE
			AssertNotNull(firstArgument);
			AssertNotNull(secondArgument);
			AssertNotNull(thirdArgument);
			AssertNotNull(fourthArgument);
			AssertNotNull(fifthArgument);
			AssertNotNull(sixthArgument);
			AssertNotNull(seventhArgument);
			AssertNotNull(eighthArgument);
			AssertNotNull(ninthArgument);
			AssertNotNull(tenthArgument);
			AssertNotNull(eleventhArgument);
			AssertNotNull(twelfthArgument);
			#endif
		}
		
		/// <summary>
		/// Handles validating the initialization argument that was received by this component, if Play Mode is active,
		/// and Null Argument Guard has been enabled for the component. 
		/// <para>
		/// Calls to this method are ignored in builds.
		/// </para>
		/// </summary>
		/// <param name="argument"> The received argument to validate. </param>
		/// <param name="context"> Initialization phase during which the method is being called. </param>
		[Conditional("UNITY_EDITOR"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		#if UNITY_EDITOR
		async
		#endif
		protected void HandleValidate<TArgument>(TArgument argument, Context context = Context.Default)
		{
			#if UNITY_EDITOR
			if(context.TryDetermineIsEditMode(out bool editMode))
			{
				if(editMode)
				{
					return;
				}

				if(!context.IsUnitySafeContext())
				{
					await Until.UnitySafeContext();
				}
			}
			else
			{
				await Until.UnitySafeContext();

				if(!Application.isPlaying)
				{
					return;
				}
			}

			if(ShouldSelfGuardAgainstNull(this))
			{
				ValidateArgument(argument);
			}
			#endif
		}
		
		/// <summary>
		/// Handles validating the initialization arguments that were received by this component, if Play Mode is active,
		/// and Null Argument Guard has been enabled for the component. 
		/// <para>
		/// Calls to this method are ignored in builds.
		/// </para>
		/// </summary>
		/// <param name="firstArgument"> The first received argument to validate. </param>
		/// <param name="secondArgument"> The second received argument to validate. </param>
		/// <param name="context"> Initialization phase during which the method is being called. </param>
		[Conditional("UNITY_EDITOR"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		#if UNITY_EDITOR
		async
		#endif
		protected void HandleValidate<TFirstArgument, TSecondArgument>(TFirstArgument firstArgument, TSecondArgument secondArgument, Context context = Context.Default)
		{
			#if UNITY_EDITOR
			if(context.TryDetermineIsEditMode(out bool editMode))
			{
				if(editMode)
				{
					return;
				}

				if(!context.IsUnitySafeContext())
				{
					await Until.UnitySafeContext();
				}
			}
			else
			{
				await Until.UnitySafeContext();

				if(!Application.isPlaying)
				{
					return;
				}
			}

			if(ShouldSelfGuardAgainstNull(this))
			{
				ValidateArguments(firstArgument, secondArgument);
			}
			#endif
		}
		
		/// <summary>
		/// Handles validating the initialization arguments that were received by this component, if Play Mode is active,
		/// and Null Argument Guard has been enabled for the component. 
		/// <para>
		/// Calls to this method are ignored in builds.
		/// </para>
		/// </summary>
		/// <param name="firstArgument"> The first received argument to validate. </param>
		/// <param name="secondArgument"> The second received argument to validate. </param>
		/// <param name="thirdArgument"> The third received argument to validate. </param>
		/// <param name="context"> Initialization phase during which the method is being called. </param>
		[Conditional("UNITY_EDITOR"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		#if UNITY_EDITOR
		async
		#endif
		protected void HandleValidate<TFirstArgument, TSecondArgument, TThirdArgument>(TFirstArgument firstArgument, TSecondArgument secondArgument, TThirdArgument thirdArgument, Context context = Context.Default)
		{
			#if UNITY_EDITOR
			if(context.TryDetermineIsEditMode(out bool editMode))
			{
				if(editMode)
				{
					return;
				}

				if(!context.IsUnitySafeContext())
				{
					await Until.UnitySafeContext();
				}
			}
			else
			{
				await Until.UnitySafeContext();

				if(!Application.isPlaying)
				{
					return;
				}
			}

			if(ShouldSelfGuardAgainstNull(this))
			{
				ValidateArguments(firstArgument, secondArgument, thirdArgument);
			}
			#endif
		}
		
		/// <summary>
		/// Handles validating the initialization arguments that were received by this component, if Play Mode is active,
		/// and Null Argument Guard has been enabled for the component. 
		/// <para>
		/// Calls to this method are ignored in builds.
		/// </para>
		/// </summary>
		/// <param name="firstArgument"> The first received argument to validate. </param>
		/// <param name="secondArgument"> The second received argument to validate. </param>
		/// <param name="thirdArgument"> The third received argument to validate. </param>
		/// <param name="fourthArgument"> The fourth received argument to validate. </param>
		/// <param name="context"> Initialization phase during which the method is being called. </param>
		[Conditional("UNITY_EDITOR"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		#if UNITY_EDITOR
		async
		#endif
		protected void HandleValidate<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument>(TFirstArgument firstArgument, TSecondArgument secondArgument, TThirdArgument thirdArgument, TFourthArgument fourthArgument, Context context = Context.Default)
		{
			#if UNITY_EDITOR
			if(context.TryDetermineIsEditMode(out bool editMode))
			{
				if(editMode)
				{
					return;
				}

				if(!context.IsUnitySafeContext())
				{
					await Until.UnitySafeContext();
				}
			}
			else
			{
				await Until.UnitySafeContext();

				if(!Application.isPlaying)
				{
					return;
				}
			}

			if(ShouldSelfGuardAgainstNull(this))
			{
				ValidateArguments(firstArgument, secondArgument, thirdArgument, fourthArgument);
			}
			#endif
		}
		
		/// <summary>
		/// Handles validating the initialization arguments that were received by this component, if Play Mode is active,
		/// and Null Argument Guard has been enabled for the component. 
		/// <para>
		/// Calls to this method are ignored in builds.
		/// </para>
		/// </summary>
		/// <param name="firstArgument"> The first received argument to validate. </param>
		/// <param name="secondArgument"> The second received argument to validate. </param>
		/// <param name="thirdArgument"> The third received argument to validate. </param>
		/// <param name="fourthArgument"> The fourth received argument to validate. </param>
		/// <param name="fifthArgument"> The fifth received argument to validate. </param>
		/// <param name="context"> Initialization phase during which the method is being called. </param>
		[Conditional("UNITY_EDITOR"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		#if UNITY_EDITOR
		async
		#endif
		protected void HandleValidate<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument>(TFirstArgument firstArgument, TSecondArgument secondArgument, TThirdArgument thirdArgument, TFourthArgument fourthArgument, TFifthArgument fifthArgument, Context context = Context.Default)
		{
			#if UNITY_EDITOR
			if(context.TryDetermineIsEditMode(out bool editMode))
			{
				if(editMode)
				{
					return;
				}

				if(!context.IsUnitySafeContext())
				{
					await Until.UnitySafeContext();
				}
			}
			else
			{
				await Until.UnitySafeContext();

				if(!Application.isPlaying)
				{
					return;
				}
			}

			if(ShouldSelfGuardAgainstNull(this))
			{
				ValidateArguments(firstArgument, secondArgument, thirdArgument, fourthArgument, fifthArgument);
			}
			#endif
		}
		
		/// <summary>
		/// Handles validating the initialization arguments that were received by this component, if Play Mode is active,
		/// and Null Argument Guard has been enabled for the component. 
		/// <para>
		/// Calls to this method are ignored in builds.
		/// </para>
		/// </summary>
		/// <param name="firstArgument"> The first received argument to validate. </param>
		/// <param name="secondArgument"> The second received argument to validate. </param>
		/// <param name="thirdArgument"> The third received argument to validate. </param>
		/// <param name="fourthArgument"> The fourth received argument to validate. </param>
		/// <param name="fifthArgument"> The fifth received argument to validate. </param>
		/// <param name="sixthArgument"> The sixth received argument to validate. </param>
		/// <param name="context"> Initialization phase during which the method is being called. </param>
		[Conditional("UNITY_EDITOR"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		#if UNITY_EDITOR
		async
		#endif
		protected void HandleValidate<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument>(TFirstArgument firstArgument, TSecondArgument secondArgument, TThirdArgument thirdArgument, TFourthArgument fourthArgument, TFifthArgument fifthArgument, TSixthArgument sixthArgument, Context context = Context.Default)
		{
			#if UNITY_EDITOR
			if(context.TryDetermineIsEditMode(out bool editMode))
			{
				if(editMode)
				{
					return;
				}

				if(!context.IsUnitySafeContext())
				{
					await Until.UnitySafeContext();
				}
			}
			else
			{
				await Until.UnitySafeContext();

				if(!Application.isPlaying)
				{
					return;
				}
			}

			if(ShouldSelfGuardAgainstNull(this))
			{
				ValidateArguments(firstArgument, secondArgument, thirdArgument, fourthArgument, fifthArgument, sixthArgument);
			}
			#endif
		}
		
		/// <summary>
		/// Handles validating the initialization arguments that were received by this component, if Play Mode is active,
		/// and Null Argument Guard has been enabled for the component. 
		/// <para>
		/// Calls to this method are ignored in builds.
		/// </para>
		/// </summary>
		/// <param name="firstArgument"> The first received argument to validate. </param>
		/// <param name="secondArgument"> The second received argument to validate. </param>
		/// <param name="thirdArgument"> The third received argument to validate. </param>
		/// <param name="fourthArgument"> The fourth received argument to validate. </param>
		/// <param name="fifthArgument"> The fifth received argument to validate. </param>
		/// <param name="sixthArgument"> The sixth received argument to validate. </param>
		/// <param name="seventhArgument"> The seventh received argument to validate. </param>
		/// <param name="context"> Initialization phase during which the method is being called. </param>
		[Conditional("UNITY_EDITOR"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		#if UNITY_EDITOR
		async
		#endif
		protected void HandleValidate<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument>(TFirstArgument firstArgument, TSecondArgument secondArgument, TThirdArgument thirdArgument, TFourthArgument fourthArgument, TFifthArgument fifthArgument, TSixthArgument sixthArgument, TSeventhArgument seventhArgument, Context context = Context.Default)
		{
			#if UNITY_EDITOR
			if(context.TryDetermineIsEditMode(out bool editMode))
			{
				if(editMode)
				{
					return;
				}

				if(!context.IsUnitySafeContext())
				{
					await Until.UnitySafeContext();
				}
			}
			else
			{
				await Until.UnitySafeContext();

				if(!Application.isPlaying)
				{
					return;
				}
			}

			if(ShouldSelfGuardAgainstNull(this))
			{
				ValidateArguments(firstArgument, secondArgument, thirdArgument, fourthArgument, fifthArgument, sixthArgument, seventhArgument);
			}
			#endif
		}
		
		/// <summary>
		/// Handles validating the initialization arguments that were received by this component, if Play Mode is active,
		/// and Null Argument Guard has been enabled for the component. 
		/// <para>
		/// Calls to this method are ignored in builds.
		/// </para>
		/// </summary>
		/// <param name="firstArgument"> The first received argument to validate. </param>
		/// <param name="secondArgument"> The second received argument to validate. </param>
		/// <param name="thirdArgument"> The third received argument to validate. </param>
		/// <param name="fourthArgument"> The fourth received argument to validate. </param>
		/// <param name="fifthArgument"> The fifth received argument to validate. </param>
		/// <param name="sixthArgument"> The sixth received argument to validate. </param>
		/// <param name="seventhArgument"> The seventh received argument to validate. </param>
		/// <param name="eighthArgument"> The eighth received argument to validate. </param>
		/// <param name="context"> Initialization phase during which the method is being called. </param>
		[Conditional("UNITY_EDITOR"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		#if UNITY_EDITOR
		async
		#endif
		protected void HandleValidate<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument, TEighthArgument>(TFirstArgument firstArgument, TSecondArgument secondArgument, TThirdArgument thirdArgument, TFourthArgument fourthArgument, TFifthArgument fifthArgument, TSixthArgument sixthArgument, TSeventhArgument seventhArgument, TEighthArgument eighthArgument, Context context = Context.Default)
		{
			#if UNITY_EDITOR
			if(context.TryDetermineIsEditMode(out bool editMode))
			{
				if(editMode)
				{
					return;
				}

				if(!context.IsUnitySafeContext())
				{
					await Until.UnitySafeContext();
				}
			}
			else
			{
				await Until.UnitySafeContext();

				if(!Application.isPlaying)
				{
					return;
				}
			}

			if(ShouldSelfGuardAgainstNull(this))
			{
				ValidateArguments(firstArgument, secondArgument, thirdArgument, fourthArgument, fifthArgument, sixthArgument, seventhArgument, eighthArgument);
			}
			#endif
		}
		
		/// <summary>
		/// Handles validating the initialization arguments that were received by this component, if Play Mode is active,
		/// and Null Argument Guard has been enabled for the component. 
		/// <para>
		/// Calls to this method are ignored in builds.
		/// </para>
		/// </summary>
		/// <param name="firstArgument"> The first received argument to validate. </param>
		/// <param name="secondArgument"> The second received argument to validate. </param>
		/// <param name="thirdArgument"> The third received argument to validate. </param>
		/// <param name="fourthArgument"> The fourth received argument to validate. </param>
		/// <param name="fifthArgument"> The fifth received argument to validate. </param>
		/// <param name="sixthArgument"> The sixth received argument to validate. </param>
		/// <param name="seventhArgument"> The seventh received argument to validate. </param>
		/// <param name="eighthArgument"> The eighth received argument to validate. </param>
		/// <param name="ninthArgument"> The ninth received argument to validate. </param>
		/// <param name="context"> Initialization phase during which the method is being called. </param>
		[Conditional("UNITY_EDITOR"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		#if UNITY_EDITOR
		async
		#endif
		protected void HandleValidate<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument, TEighthArgument, TNinthArgument>(TFirstArgument firstArgument, TSecondArgument secondArgument, TThirdArgument thirdArgument, TFourthArgument fourthArgument, TFifthArgument fifthArgument, TSixthArgument sixthArgument, TSeventhArgument seventhArgument, TEighthArgument eighthArgument, TNinthArgument ninthArgument, Context context = Context.Default)
		{
			#if UNITY_EDITOR
			if(context.TryDetermineIsEditMode(out bool editMode))
			{
				if(editMode)
				{
					return;
				}

				if(!context.IsUnitySafeContext())
				{
					await Until.UnitySafeContext();
				}
			}
			else
			{
				await Until.UnitySafeContext();

				if(!Application.isPlaying)
				{
					return;
				}
			}

			if(ShouldSelfGuardAgainstNull(this))
			{
				ValidateArguments(firstArgument, secondArgument, thirdArgument, fourthArgument, fifthArgument, sixthArgument, seventhArgument, eighthArgument, ninthArgument);
			}
			#endif
		}
		
		/// <summary>
		/// Handles validating the initialization arguments that were received by this component, if Play Mode is active,
		/// and Null Argument Guard has been enabled for the component. 
		/// <para>
		/// Calls to this method are ignored in builds.
		/// </para>
		/// </summary>
		/// <param name="firstArgument"> The first received argument to validate. </param>
		/// <param name="secondArgument"> The second received argument to validate. </param>
		/// <param name="thirdArgument"> The third received argument to validate. </param>
		/// <param name="fourthArgument"> The fourth received argument to validate. </param>
		/// <param name="fifthArgument"> The fifth received argument to validate. </param>
		/// <param name="sixthArgument"> The sixth received argument to validate. </param>
		/// <param name="seventhArgument"> The seventh received argument to validate. </param>
		/// <param name="eighthArgument"> The eighth received argument to validate. </param>
		/// <param name="ninthArgument"> The ninth received argument to validate. </param>
		/// <param name="tenthArgument"> The tenth received argument to validate. </param>
		/// <param name="context"> Initialization phase during which the method is being called. </param>
		[Conditional("UNITY_EDITOR"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		#if UNITY_EDITOR
		async
		#endif
		protected void HandleValidate<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument, TEighthArgument, TNinthArgument, TTenthArgument>(TFirstArgument firstArgument, TSecondArgument secondArgument, TThirdArgument thirdArgument, TFourthArgument fourthArgument, TFifthArgument fifthArgument, TSixthArgument sixthArgument, TSeventhArgument seventhArgument, TEighthArgument eighthArgument, TNinthArgument ninthArgument, TTenthArgument tenthArgument, Context context = Context.Default)
		{
			#if UNITY_EDITOR
			if(context.TryDetermineIsEditMode(out bool editMode))
			{
				if(editMode)
				{
					return;
				}

				if(!context.IsUnitySafeContext())
				{
					await Until.UnitySafeContext();
				}
			}
			else
			{
				await Until.UnitySafeContext();

				if(!Application.isPlaying)
				{
					return;
				}
			}

			if(ShouldSelfGuardAgainstNull(this))
			{
				ValidateArguments(firstArgument, secondArgument, thirdArgument, fourthArgument, fifthArgument, sixthArgument, seventhArgument, eighthArgument, ninthArgument, tenthArgument);
			}
			#endif
		}

		/// <summary>
		/// Handles validating the initialization arguments that were received by this component, if Play Mode is active,
		/// and Null Argument Guard has been enabled for the component. 
		/// <para>
		/// Calls to this method are ignored in builds.
		/// </para>
		/// </summary>
		/// <param name="firstArgument"> The first received argument to validate. </param>
		/// <param name="secondArgument"> The second received argument to validate. </param>
		/// <param name="thirdArgument"> The third received argument to validate. </param>
		/// <param name="fourthArgument"> The fourth received argument to validate. </param>
		/// <param name="fifthArgument"> The fifth received argument to validate. </param>
		/// <param name="sixthArgument"> The sixth received argument to validate. </param>
		/// <param name="seventhArgument"> The seventh received argument to validate. </param>
		/// <param name="eighthArgument"> The eighth received argument to validate. </param>
		/// <param name="ninthArgument"> The ninth received argument to validate. </param>
		/// <param name="tenthArgument"> The tenth received argument to validate. </param>
		/// <param name="eleventhArgument"> The eleventh received argument to validate. </param>
		/// <param name="context"> Initialization phase during which the method is being called. </param>
		[Conditional("UNITY_EDITOR"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		#if UNITY_EDITOR
		async
		#endif
		protected void HandleValidate<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument, TEighthArgument, TNinthArgument, TTenthArgument, TEleventhArgument>(TFirstArgument firstArgument, TSecondArgument secondArgument, TThirdArgument thirdArgument, TFourthArgument fourthArgument, TFifthArgument fifthArgument, TSixthArgument sixthArgument, TSeventhArgument seventhArgument, TEighthArgument eighthArgument, TNinthArgument ninthArgument, TTenthArgument tenthArgument, TEleventhArgument eleventhArgument, Context context = Context.Default)
		{
			#if UNITY_EDITOR
			if(context.TryDetermineIsEditMode(out bool editMode))
			{
				if(editMode)
				{
					return;
				}

				if(!context.IsUnitySafeContext())
				{
					await Until.UnitySafeContext();
				}
			}
			else
			{
				await Until.UnitySafeContext();

				if(!Application.isPlaying)
				{
					return;
				}
			}

			if(ShouldSelfGuardAgainstNull(this))
			{
				ValidateArguments(firstArgument, secondArgument, thirdArgument, fourthArgument, fifthArgument, sixthArgument, seventhArgument, eighthArgument, ninthArgument, tenthArgument, eleventhArgument);
			}
			#endif
		}

		/// <summary>
		/// Handles validating the initialization arguments that were received by this component, if Play Mode is active,
		/// and Null Argument Guard has been enabled for the component. 
		/// <para>
		/// Calls to this method are ignored in builds.
		/// </para>
		/// </summary>
		/// <param name="firstArgument"> The first received argument to validate. </param>
		/// <param name="secondArgument"> The second received argument to validate. </param>
		/// <param name="thirdArgument"> The third received argument to validate. </param>
		/// <param name="fourthArgument"> The fourth received argument to validate. </param>
		/// <param name="fifthArgument"> The fifth received argument to validate. </param>
		/// <param name="sixthArgument"> The sixth received argument to validate. </param>
		/// <param name="seventhArgument"> The seventh received argument to validate. </param>
		/// <param name="eighthArgument"> The eighth received argument to validate. </param>
		/// <param name="ninthArgument"> The ninth received argument to validate. </param>
		/// <param name="tenthArgument"> The tenth received argument to validate. </param>
		/// <param name="eleventhArgument"> The eleventh received argument to validate. </param>
		/// <param name="twelfthArgument"> The twelfth received argument to validate. </param>
		/// <param name="context"> Initialization phase during which the method is being called. </param>
		[Conditional("UNITY_EDITOR"), MethodImpl(MethodImplOptions.AggressiveInlining)]
		#if UNITY_EDITOR
		async
		#endif
		protected void HandleValidate<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument, TEighthArgument, TNinthArgument, TTenthArgument, TEleventhArgument, TTwelfthArgument>(TFirstArgument firstArgument, TSecondArgument secondArgument, TThirdArgument thirdArgument, TFourthArgument fourthArgument, TFifthArgument fifthArgument, TSixthArgument sixthArgument, TSeventhArgument seventhArgument, TEighthArgument eighthArgument, TNinthArgument ninthArgument, TTenthArgument tenthArgument, TEleventhArgument eleventhArgument, TTwelfthArgument twelfthArgument, Context context = Context.Default)
		{
			#if UNITY_EDITOR
			if(context.TryDetermineIsEditMode(out bool editMode))
			{
				if(editMode)
				{
					return;
				}

				if(!context.IsUnitySafeContext())
				{
					await Until.UnitySafeContext();
				}
			}
			else
			{
				await Until.UnitySafeContext();

				if(!Application.isPlaying)
				{
					return;
				}
			}

			if(ShouldSelfGuardAgainstNull(this))
			{
				ValidateArguments(firstArgument, secondArgument, thirdArgument, fourthArgument, fifthArgument, sixthArgument, seventhArgument, eighthArgument, ninthArgument, tenthArgument, eleventhArgument, twelfthArgument);
			}
			#endif
		}
		
		#if UNITY_EDITOR
		IInitializer IInitializableEditorOnly.Initializer { get => TryGetInitializer(this, out IInitializer initializer) ? initializer : null; set { if(value != Null) value.Target = this; else if(TryGetInitializer(this, out IInitializer initializer)) initializer.Target = null; } }
		bool IInitializableEditorOnly.CanInitSelfWhenInactive => false;
		InitState IInitializableEditorOnly.InitState => initState;
		#endif
	}
}