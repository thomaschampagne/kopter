using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	protected MotorControlsMessage mMotorControlsMessage = null;

	// Use this for initialization
	void Start () {
		mMotorControlsMessage = new MotorControlsMessage();
	}
	
	// Update is called once per frame
	void Update () {
	
		// User wants to switch camera?
		if(Input.GetButtonUp("Switch Camera")) {
			CameraManager.Instance.SwitchNextCamera();
		}

		// Reset
		if(Input.GetButtonUp("Reset Kopter")) {
			Motor.Instance.Reset();
		}

		// Quit
		if(Input.GetButtonUp("Quit Game")) {
			Application.Quit();	
		}

		// Getting user motor input data
		float throttle = Input.GetAxis("Y axis");
		float selfRotation = Input.GetAxis("X axis") * -1;
		float xStraffing = Input.GetAxis("2nd X axis");
		float yStraffing = Input.GetAxis("2nd Y axis");

		// Prepare control message
		mMotorControlsMessage.ThrottleInput = throttle;
		mMotorControlsMessage.SelfRotationInput = selfRotation;
		mMotorControlsMessage.StrafingVectorInput = new Vector3 (-xStraffing, 0, yStraffing);

		// Send that message
		SendMessage(Motor.controlsMessageReceiverMethodName, mMotorControlsMessage);

	}

}
