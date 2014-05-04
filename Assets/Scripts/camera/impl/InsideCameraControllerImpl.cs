using UnityEngine;
using System.Collections;

public class InsideCameraControllerImpl : AbstractCameraController, CameraControllerInterface
{

	public void InitCam ()
	{

	}

	public void LateUpdateCam ()
	{
		camera.transform.position = tracker.transform.position + tracker.transform.forward * 2 - Vector3.up;
		camera.transform.rotation = Quaternion.LookRotation (tracker.transform.forward, tracker.transform.up);
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
