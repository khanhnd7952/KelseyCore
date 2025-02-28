﻿using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Sisus.Init.Internal;
using UnityEngine;
using static Sisus.Init.Internal.InitializerUtility;
using static Sisus.Init.Reflection.InjectionUtility;

namespace Sisus.Init
{
	/// <summary>
	/// A base class for <see cref="MonoBehaviour">MonoBehaviours</see> that can be
	/// <see cref="InstantiateExtensions.Instantiate{TComponent, TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument> : MonoBehaviour, IInitializable<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument}">instantiated</see>
	/// or <see cref="AddComponentExtensions.AddComponent{TComponent, TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument> : MonoBehaviour, IInitializable<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument}">added</see>
	/// to a <see cref="GameObject"/> with seven arguments passed to the <see cref="Init"/> function of the created instance.
	/// <para>
	/// If the object depends exclusively on classes that have the <see cref="ServiceAttribute"/> then
	/// it will receive them in its <see cref="Init"/> function automatically during initialization.
	/// </para>
	/// <para>
	/// If the component is part of a scene or a prefab, add depends on any classes that don't have the <see cref="ServiceAttribute"/>, then an
	/// <see cref="Initializer{TComponent, TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument> : MonoBehaviour, IInitializable<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument}"/>
	/// can be used to specify its initialization arguments.
	/// </para>
	/// <para>
	/// Instances of classes inheriting from <see cref="MonoBehaviour{TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument> : MonoBehaviour, IInitializable<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument}"/>
	/// receive the arguments via the <see cref="Init"/> method where they can be assigned to member fields or properties.
	/// </para>
	/// </summary>
	/// <typeparam name="TFirstArgument"> Type of the first argument received in the <see cref="Init"/> function. </typeparam>
	/// <typeparam name="TSecondArgument"> Type of the second argument received in the <see cref="Init"/> function. </typeparam>
	/// <typeparam name="TThirdArgument"> Type of the third argument received in the <see cref="Init"/> function. </typeparam>
	/// <typeparam name="TFourthArgument"> Type of the fourth argument received in the <see cref="Init"/> function. </typeparam>
	/// <typeparam name="TFifthArgument"> Type of the fifth argument received in the <see cref="Init"/> function. </typeparam>
	/// <typeparam name="TSixthArgument"> Type of the sixth argument received in the <see cref="Init"/> function. </typeparam>
	/// <typeparam name="TSeventhArgument"> Type of the seventh argument received in the <see cref="Init"/> function. </typeparam>
	public abstract class MonoBehaviour<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument> : InitializableBaseInternal, IInitializable<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument>
	{
		/// <summary>
		/// Provides the <see cref="Component"/> with the objects that it depends on.
		/// <para>
		/// You can think of the <see cref="Init"/> function as a parameterized constructor alternative for the component.
		/// </para>
		/// <para>
		/// <see cref="Init"/> get called when the script is being loaded, before the <see cref="OnAwake"/>, OnEnable and Start events when
		/// the component is created using <see cref="InstantiateExtensions.Instantiate{TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument}"/> or
		/// <see cref="AddComponentExtensions.AddComponent{TComponent, TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument}"/>.
		/// </para>
		/// <para>
		/// In edit mode <see cref="Init"/> can also get called during the <see cref="Reset"/> event when the script is added to a <see cref="GameObject"/>,
		/// if the component has the <see cref="InitOnResetAttribute"/> or <see cref="RequireComponent"/> attribute for each argument.
		/// </para>
		/// <para>
		/// Note that <see cref="Init"/> gets called immediately following object creation even if the <see cref="GameObject"/> to which the component
		/// was added is <see cref="GameObject.activeInHierarchy">inactive</see> unlike some other initialization event functions such as Awake and OnEnable.
		/// </para>
		/// </summary>
		/// <param name="firstArgument"> The first object that this component depends on. </param>
		/// <param name="secondArgument"> The second object that this component depends on. </param>
		/// <param name="thirdArgument"> The third object that this component depends on. </param>
		/// <param name="fourthArgument"> The fourth object that this component depends on. </param>
		/// <param name="fifthArgument"> The fifth object that this component depends on. </param>
		/// <param name="sixthArgument"> The sixth object that this component depends on. </param>
		/// <param name="seventhArgument"> The seventh object that this component depends on. </param>
		protected abstract void Init(TFirstArgument firstArgument, TSecondArgument secondArgument, TThirdArgument thirdArgument, TFourthArgument fourthArgument, TFifthArgument fifthArgument, TSixthArgument sixthArgument, TSeventhArgument seventhArgument);

		/// <summary>
		/// Assigns an argument received during initialization to a field or property by the <paramref name="memberName">given name</paramref>.
		/// <para>
		/// Because reflection is used to set the value it is possible to use this to assign to init only fields and properties.
		/// Properties that do not have a set accessor and are not auto-implemented are not supported however.
		/// </para>
		/// </summary>
		/// <param name="memberName"> Name of the field or property to which to assign the value. </param>
		/// <exception cref="InvalidOperationException">
		/// Thrown if this method is called outside of the context of the client object being <see cref="Init">initialized</see>.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// Thrown if the provided <paramref name="memberName"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="MissingMemberException">
		/// Thrown if no field or property by the provided name is found or if property by given name is not auto-implemented
		/// and does not have a set accessor.
		/// </exception>
		protected object this[[DisallowNull] string memberName]
		{
			set
			{
				#if DEBUG || INIT_ARGS_SAFE_MODE
				if(initState is InitState.Initialized) throw new InvalidOperationException($"Unable to assign to member {GetType().Name}.{memberName}: Values can only be injected during initialization.");
				#endif

				Inject<MonoBehaviour<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument>, TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument>(this, memberName, value);
			}
		}

		/// <summary>
		/// Reset state to default values.
		/// <para>
		/// <see cref="OnReset"/> is called when the user selects Reset in the Inspector context menu or when adding the component the first time.
		/// </para>
		/// <para>
		/// This function is only called in edit mode.
		/// </para>
		/// </summary>
		protected virtual void OnReset() { }

		#if UNITY_EDITOR
		private protected void Reset()
		{
			InitInternal(Context.Reset);
			OnInitializableReset(this);
			OnReset();
		}
		#endif

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override bool Init(Context context)
		{
			if(!InitArgs.TryGet(context, this, out TFirstArgument firstArgument, out TSecondArgument secondArgument, out TThirdArgument thirdArgument, out TFourthArgument fourthArgument, out TFifthArgument fifthArgument, out TSixthArgument sixthArgument, out TSeventhArgument seventhArgument))
			{
				return false;
			}

			HandleValidate(firstArgument, secondArgument, thirdArgument, fourthArgument, fifthArgument, sixthArgument, seventhArgument, context);
			Init(firstArgument, secondArgument, thirdArgument, fourthArgument, fifthArgument, sixthArgument, seventhArgument);
			return true;
		}

		/// <inheritdoc/>
		void IInitializable<TFirstArgument, TSecondArgument, TThirdArgument, TFourthArgument, TFifthArgument, TSixthArgument, TSeventhArgument>
			.Init(TFirstArgument firstArgument, TSecondArgument secondArgument, TThirdArgument thirdArgument, TFourthArgument fourthArgument, TFifthArgument fifthArgument, TSixthArgument sixthArgument, TSeventhArgument seventhArgument)
		{
			initState = InitState.Initializing;
			HandleValidate(firstArgument, secondArgument, thirdArgument, fourthArgument, fifthArgument, sixthArgument, seventhArgument, Context.MainThread);

			Init(firstArgument, secondArgument, thirdArgument, fourthArgument, fifthArgument, sixthArgument, seventhArgument);

			initState = InitState.Initialized;
		}

		/// <summary>
		/// Identical to <see cref="Init(TArgument)"/>, but non-virtual, so slightly faster.
		/// </summary>
		internal void InitInternal(TFirstArgument firstArgument, TSecondArgument secondArgument, TThirdArgument thirdArgument, TFourthArgument fourthArgument, TFifthArgument fifthArgument, TSixthArgument sixthArgument, TSeventhArgument seventhArgument)
		{
			initState = InitState.Initializing;
			HandleValidate(firstArgument, secondArgument, thirdArgument, fourthArgument, fifthArgument, sixthArgument, seventhArgument, Context.MainThread);

			Init(firstArgument, secondArgument, thirdArgument, fourthArgument, fifthArgument, sixthArgument, seventhArgument);

			initState = InitState.Initialized;
		}
	}
}