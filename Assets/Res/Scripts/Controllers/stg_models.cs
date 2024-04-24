using System;
using UnityEngine;

public static class stg_models 
{
	#region Player

	[Serializable]
	public class PlayerSettingsModel
	{
		[Header("View Settings")]
		public float ViewXSensitivity;
		public float ViewYSensitivity;

		public bool ViewXInverted;
		public bool ViewYInverted;

		[Header("Movement Settings")]
		public bool SprintingHold;
		public float MovementSmoothing;

		[Header("Movement - Walking Settings")]

		public float WalkingForwardSpeed;
		public float WalkingBackwardSpeed;
		public float WalkingStrafeSpeed;

		[Header("Movement - Running Settings")]
		public float RunningForwardSpeed;
		public float RunningStrafeSpeed;

        [Header("Jumping")]
		public float JumpingHeight;
		public float JumpingFalloff;
    }
	#endregion

	#region Weapon

	[Serializable]
	public class WeaponSettingsModel
	{
		[Header("Sway")]
		public float SwayAmount;
		public bool SwayXInverted;
		public bool SwayYInverted;
		public float SwaySmooting;
		public float SwayResetSmoothing;
		public float SwayClampX;
		public float SwayClampY;

	}
	#endregion
}
