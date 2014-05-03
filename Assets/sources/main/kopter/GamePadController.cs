using UnityEngine;
using System.Collections;

public class GamePadController : MonoBehaviour {

	protected ControlMessage mControlsMessage = null, mPreviousControlsMessage = null;

	// Use this for initialization
	void Start () {
		mControlsMessage = new ControlMessage();
	}
	
	// Update is called once per frame
	void Update () {
	
		// Getting user input data
		float throttle = Input.GetAxis("Y axis");
		float selfRotation = Input.GetAxis("X axis") * -1;
		float xStraffing = Input.GetAxis("2nd X axis");
		float yStraffing = Input.GetAxis("2nd Y axis");

		// Prepare control message
		mControlsMessage.ThrottleInput = throttle;
		mControlsMessage.SelfRotationInput = selfRotation;
		mControlsMessage.StrafingVectorInput = new Vector3 (-xStraffing, 0, yStraffing);

		// Send that message
		SendMessage(Motor.controlsMessageReceiverMethodName, mControlsMessage);

	}

}
