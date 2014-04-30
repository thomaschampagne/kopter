using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{

	public static Controller Instance { get; set; }
	
	private float throttle;
	
	// Use this for initialization
	void Awake ()
	{
		Instance = this;
	}
	
	// Use this for initialization
	void Start ()
	{
		throttle = 0.0f;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
		
		// Computing rotor Throttle
		float throttleRotorIn = 0.0f;
		if (Input.GetKey (KeyCode.Z)) {	throttleRotorIn = 1.0f;	}
		if (Input.GetKey (KeyCode.S)) {	throttleRotorIn = -1.0f; }
		lerpingThrottle(ref throttle, ref throttleRotorIn);
		Motor.Instance.ThrottleInput = throttleRotorIn;
		
		if(Motor.Instance.R.magnitude == 0) {
		
			// Computing copter plate from ARROWS and mouse
			float mouseXin = 0f;
			float mouseYin = 0f;
			
			// If not left click dont change mouse inputs value: This is used for orbital camera
			if (!Input.GetMouseButton (0)) {
				mouseXin = Input.GetAxis ("mouseX");
				mouseYin = Input.GetAxis ("mouseY");
			}
			
			float leftRightPlateInput = Input.GetAxis ("Horizontal") + mouseXin;
			float frontBackPlateInput = Input.GetAxis ("Vertical") + mouseYin;
			Motor.Instance.StrafingVectorInput = (new Vector3 (-leftRightPlateInput, 0, frontBackPlateInput));
			
			// Find self copter rotation	
			float selfCopterRotation = 0.0f;
			if (Input.GetKey (KeyCode.Q)) {	selfCopterRotation = -1.0f;	}
			if (Input.GetKey (KeyCode.D)) {	selfCopterRotation = 1.0f;	}
			Motor.Instance.SelfRotationInput = selfCopterRotation;
		}
		
		// Update
		Motor.Instance.UpdateMotor ();
		
		// Reset
		if(Input.GetKeyUp (KeyCode.R)) {
			Motor.Instance.Reset();
		}
		
		// Change camera
		if (Input.GetKeyDown (KeyCode.F1)) {
			CameraManager.Instance.SwitchNextCamera();
		}
		
		// Quit game
		if(Input.GetKeyUp (KeyCode.Escape)) {
			Application.Quit();	
		}
		
		// Screenshot
		if(Input.GetKeyUp (KeyCode.F12)) {
			Application.CaptureScreenshot("Screenshot_" + Time.time + ".png");
		}
		
	}
	
	void lerpingThrottle (ref float throttle, ref float throttleRotorIn)
	{
		if (Mathf.Abs (throttleRotorIn) > 0.0f) { // local yaw rotation wanted
			throttle = Mathf.Lerp (throttle, throttleRotorIn, Time.deltaTime * 3);
		} else { // Lerping yaw angular speed to zero
			throttle = Mathf.Lerp (throttle, 0.0f, Time.deltaTime * 3);
		}
	}
}


