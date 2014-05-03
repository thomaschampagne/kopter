using System;
using UnityEngine;

public class MotorControlsMessage
{

	// ThrottleInput
	protected float mThrottleInput = 0;

	// StrafingVectorInput
	protected Vector3 mStrafingVectorInput = Vector3.zero;

	// SelfRotationInput
	protected float mSelfRotationInput = 0;

	protected bool mSwitchCamera = false;


	public float ThrottleInput {
		get {
			return this.mThrottleInput;
		}
		set {
			mThrottleInput = value;
		}
	}

	public Vector3 StrafingVectorInput {
		get {
			return this.mStrafingVectorInput;
		}
		set {
			mStrafingVectorInput = value;
		}
	}

	public float SelfRotationInput {
		get {
			return this.mSelfRotationInput;
		}
		set {
			mSelfRotationInput = value;
		}
	}

	bool SwitchCamera {
		get {
			return this.mSwitchCamera;
		}
		set {
			mSwitchCamera = value;
		}
	}

	public override int GetHashCode ()
	{
		unchecked {
			return mThrottleInput.GetHashCode () ^ mStrafingVectorInput.GetHashCode () ^ mSelfRotationInput.GetHashCode ();
		}
	}

	public override string ToString ()
	{
		return string.Format ("[ControlMessage: mThrottleInput={0}, mStrafingVectorInput={1}, mSelfRotationInput={2}]", mThrottleInput, mStrafingVectorInput, mSelfRotationInput);
	}
}


