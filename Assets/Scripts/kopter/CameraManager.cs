using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
	
	public enum CameraState
	{
		Behind,
		Inside,
		OrbitalMouseClick
	}
	
	public static CameraManager Instance { get; set; } // Provide public static access
	public CameraState CurrentCamState { get; set; }
	protected Transform cameraTracker;
	
	
	protected CameraControllerInterface currentCam;
	protected BehindCameraControllerImpl behindCamRef;
	protected OrbitalCameraControllerImpl orbitalCamRef;
	protected InsideCameraControllerImpl insideCamRef;
	
	void Awake ()
	{
		
		// Setup self instance
		if (Instance != null) {
			throw new UnityException ("CameraManager Instance already exist !");
		} else {
			Instance = this; 	
		}
		
		// Get the camera tracker and keep reference
		cameraTracker = this.transform;
		
		if (cameraTracker == null) {
			string errorMessage = "No cameraTracker founded. Add one under that GameObject !";
			Debug.LogError (errorMessage);
			throw new UnityException (errorMessage);
		}

		// Setup normal camera reference
		behindCamRef = ScriptableObject.CreateInstance<BehindCameraControllerImpl> ();
		behindCamRef.setCamera (Camera.main);
		behindCamRef.setTracker (cameraTracker);
		behindCamRef.InitCam ();
		
		// Setup inside camera reference
		insideCamRef = ScriptableObject.CreateInstance<InsideCameraControllerImpl> ();
		insideCamRef.setCamera (Camera.main);
		insideCamRef.setTracker (cameraTracker);
		insideCamRef.InitCam ();
		
		// Setup orbital camera reference
		orbitalCamRef = ScriptableObject.CreateInstance<OrbitalCameraControllerImpl> ();
		orbitalCamRef.setCamera (Camera.main);
		orbitalCamRef.setTracker (cameraTracker);
		orbitalCamRef.InitCam ();

	}
	
	// Use this for initialization
	void Start ()
	{
		CurrentCamState = CameraState.Behind;
		currentCam = behindCamRef;
	}
	
	void LateUpdate ()
	{
		currentCam.LateUpdateCam ();
	}
	
	public void SwitchNextCamera ()
	{
		
		Debug.Log ("Camera Switch");
		
		switch (CurrentCamState) {
			
		case CameraState.Behind:
			CurrentCamState = CameraState.Inside;
			currentCam = insideCamRef;
			break;
		
		case CameraState.Inside:
			CurrentCamState = CameraState.OrbitalMouseClick;
			currentCam = orbitalCamRef;
			break;
			
		case CameraState.OrbitalMouseClick:
			CurrentCamState = CameraState.Behind;
			currentCam = behindCamRef;
			break;	
		}
	}
}
