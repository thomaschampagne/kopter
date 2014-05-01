using System;
using UnityEngine;

public class ControlMessage
{

	// ThrottleInput
	protected float mThrottleInput = 0;

	// StrafingVectorInput
	protected Vector3 mStrafingVectorInput = Vector3.zero;

	// SelfRotationInput
	protected float mSelfRotationInput = 0;


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

	public override bool Equals (object obj)
	{
		if (obj == null)
			return false;
		if (ReferenceEquals (this, obj))
			return true;
		if (obj.GetType () != typeof(ControlMessage))
			return false;
		ControlMessage other = (ControlMessage)obj;
		return mThrottleInput == other.mThrottleInput && mStrafingVectorInput == other.mStrafingVectorInput && mSelfRotationInput == other.mSelfRotationInput;
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


