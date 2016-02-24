using UnityEngine;
using System.Collections;
using System;

public class GuiManager : MonoBehaviour
{
	public GUIStyle style;
	public bool displayTweak;
	
	// Use this for initialization
	void Start ()
	{

        Cursor.visible = false;

		displayTweak = false;
		// Define style
		style = new GUIStyle ();
		style.normal.textColor = Color.black;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyUp (KeyCode.T)) {
			displayTweak = !displayTweak;
		}
	}
	
	void OnGUI ()
	{	
		/*
		 * Comments GUI Display
		if(GameManager.Instance.CurrentGameState == GameState.InGame) {
		
			if (displayTweak) {
				
				GUI.Label (new Rect (10, 10, 150, 20), "Heigth: " + Motor.Instance.transform.position.y, style);
				GUI.Label (new Rect (10, 30, 150, 20), "Speed: " + Motor.Instance.MotionSpeed.magnitude, style);
				GUI.Label (new Rect (10, 50, 150, 20), "Rpm: " + Motor.Instance.Rpm, style);
				GUI.Label (new Rect (10, 70, 150, 20), "RotorForce: " + Motor.Instance.FrRpm.magnitude, style);
		
				GUI.Label (new Rect (10, 90, 100, 30), "KRotorSurfaceForceFactor: " + Motor.Instance.KRotorSurfaceForceFactor, style);
				Motor.Instance.KRotorSurfaceForceFactor = 
					GUI.HorizontalSlider (new Rect (10, 105, 100, 30), Motor.Instance.KRotorSurfaceForceFactor, 10.0f, 0.0f);
				
				GUI.Label (new Rect (10, 130, 100, 30), "StrafeRotationAccelFactor: " + Motor.Instance.StrafeRotationAccelFactor, style);
				Motor.Instance.StrafeRotationAccelFactor = 
					GUI.HorizontalSlider (new Rect (10, 145, 100, 30), Motor.Instance.StrafeRotationAccelFactor, 10.0f, 0.0f);
				
				GUI.Label (new Rect (10, 170, 100, 30), "YawRotationAccelFactor: " + Motor.Instance.YawRotationAccelFactor, style);
				Motor.Instance.YawRotationAccelFactor = 
					GUI.HorizontalSlider (new Rect (10, 185, 100, 30), Motor.Instance.YawRotationAccelFactor, 10.0f, 0.0f);
				
				GUI.Label (new Rect (10, 210, 100, 30), "VariationMotionSpeedFactor: " + Motor.Instance.VariationMotionSpeedFactor, style);
				Motor.Instance.VariationMotionSpeedFactor = 
					GUI.HorizontalSlider (new Rect (10, 225, 100, 30), Motor.Instance.VariationMotionSpeedFactor, 10.0f, 0.0f);
				
				GUI.Label (new Rect (10, 250, 100, 30), "ClampSpeed: " + Motor.Instance.ClampSpeed, style);
				Motor.Instance.ClampSpeed = 
					GUI.HorizontalSlider (new Rect (10, 265, 100, 30), Motor.Instance.ClampSpeed, 150.0f, 50.0f);
				
			} else {
				
				GUI.Label (new Rect (10, 10, 300, 20), "Strafe [Arrows or mouse]\t|\tThrottle [Z/S]\t|\tCameras [F1] : " + CameraManager.Instance.CurrentCamState.ToString()  + " \t|\tTweak [T]\t|\tReset [R]\t|\tQuit [Escape]", style);
				GUI.Label (new Rect (10, 30, 600, 20), "Current RPMs [" + Motor.Instance.Rpm.ToString("0.00") + "]\t|\tSpeed [" + Motor.Instance.MotionSpeed.magnitude.ToString("0.00") + "]\t|\tHeight [" + Motor.Instance.FloorInfos.distance.ToString("0.00") + "]", style);
				
				
				float liftRatio = Motor.Instance.FrRpm.y / Motor.Instance.P.magnitude;			
				
				GUI.Label (new Rect (10, 50, 600, 20), "Kopter Lift Ratio [" + liftRatio.ToString("0.00") + "]\t|\tLifting [" + ((liftRatio >= 1) ? "UP" : "DOWN") + "]\t|\tGrounded [" + (!Motor.Instance.IsFlying).ToString() + "]", style);
			}
		}
		*/
	}
}
