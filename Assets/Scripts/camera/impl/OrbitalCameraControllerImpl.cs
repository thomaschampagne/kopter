using UnityEngine;
using System.Collections;

public class OrbitalCameraControllerImpl : AbstractCameraController, CameraControllerInterface
{
	//The position of the cursor on the screen. Used to rotate the camera.
	private float xRot = 0.0f;
	private float yRot = 0.0f;
	
	//The speed of the camera. Control how fast the camera will rotate.
	public float mouseXSpeed = 3f;
	public float mouseYSpeed = 3f;
	
	//Distance vector.
	private Vector3 radiusVector;
	
	//The default distance of the camera from the target.
	private float radiusValue = 15.0f;
	
	//Control the speed of zooming and dezooming.
	private float zoomStep = 1.0f;
	
	// Use this for camera initialization
	public void InitCam ()
	{
        
		if (camera == null) {
			string errorMessage = "No camera founded. Use setCamera on this Object !";
			Debug.LogError (errorMessage);
			throw new UnityException (errorMessage);
		}
		
		if (tracker == null) {
			string errorMessage = "No tracker founded. Use setTacker on this Object !";
			Debug.LogError (errorMessage);
			throw new UnityException (errorMessage);
		}
		
		radiusVector = new Vector3 (0, 0, -radiusValue); // don't touch
		
		// Init orbital angle, iso view...
		xRot = 30;
		yRot = 10;
	}
	
	public void LateUpdateCam ()
	{
		this.RotateControls ();
		this.Rotate (xRot, yRot);
		this.Zoom ();
		camera.transform.LookAt (tracker.position);
	}

	void RotateControls ()
	{
			xRot += Input.GetAxis ("Turn X Orbital Camera") * mouseXSpeed;
			yRot += -Input.GetAxis ("Turn Y Orbital Camera") * mouseYSpeed;
	}
	
	void Rotate (float x, float y)
	{
		Quaternion rotation = Quaternion.Euler (y, x, 0.0f);
		Vector3 position = rotation * radiusVector + tracker.position;
		camera.transform.position = position;
	}
	
	void Zoom ()
	{
		if (Input.GetAxis ("Zoom Orbital Camera") < 0.0f) {
			this.ZoomOut ();
		} else if (Input.GetAxis ("Zoom Orbital Camera") > 0.0f) {
			this.ZoomIn ();
		}
	}
	
	/**
  	* Reduce the distance from the camera to the target and
  	* update the position of the camera (with the Rotate function).
  	*/
	void ZoomIn ()
	{
		radiusValue -= zoomStep;
		radiusVector = new Vector3 (0, 0, -radiusValue);
		this.Rotate (xRot, yRot);
	}
  
	/**
  	* Increase the distance from the camera to the target and
  	* update the position of the camera (with the Rotate function).
  	*/
	void ZoomOut ()
	{
		radiusValue += zoomStep;
		radiusVector = new Vector3 (0, 0, -radiusValue);
		this.Rotate (xRot, yRot);
	}
	
	public void setCamera (Camera camera)
	{
		this.camera = camera;
	}

	public void setTracker (Transform tracker)
	{
		this.tracker = tracker;
	}
	
}
