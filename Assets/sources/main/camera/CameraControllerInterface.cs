using UnityEngine;

public interface CameraControllerInterface {
	void InitCam ();
	void LateUpdateCam();
	void setCamera(Camera camera);
	void setTracker(Transform tracker);
}
