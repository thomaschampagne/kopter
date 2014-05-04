using UnityEngine;
using System.Collections;

public class BehindCameraControllerImpl : AbstractCameraController, CameraControllerInterface {

	public void InitCam ()
	{

	}
	public void LateUpdateCam ()
	{
		Vector3 invertFwdOffset = tracker.transform.forward * -10;
		invertFwdOffset.y += 4;
		camera.transform.position = Vector3.Lerp (camera.transform.position, tracker.transform.position + invertFwdOffset, Time.deltaTime * 8f);
		camera.transform.LookAt (tracker.transform.position);
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
