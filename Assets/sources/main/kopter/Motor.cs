using UnityEngine;
using System.Collections;

public class Motor : MonoBehaviour
{
	
	public static Motor Instance { get; set; }
	public static string controlsMessageReceiverMethodName = "controlsReceiver";
	
	// Factors and Tweaking
	public float yawRotationAccelFactor = 5f;
	public float yawRotationDeccelFactor = 1.0f;
	public float strafeRotationAccelFactor = 1.5f;
	public float throttleAccelerationFactor = 100.0f;
	public float kRpmLift = 250;
	public float kRpmMax = 600;
	public float kRotorSurfaceForceFactor = 1.0f;
	public float mass = 1.0f;
	public float variationMotionSpeedFactor = 3.0f;
	public float clampSpeed = 110.0f;
	private const float groundValue = 0.35f;
	private const float landingMagnitudeValue = 18.0f;
	private const float landingAngleMax = 30.0f;
	
	// User Input From Controller manager
	private Vector3 strafingVectorInput;
	private float selfRotationInput;
	private float throttleInput;
	
	// Motion and forces
	bool isFlying;
	public float yawRotation;
	private Vector3 gravity = Physics.gravity; // Gravity applied to Kopter
	private Quaternion strafeRotation; // Current Kopter Orientation
	public Vector3 R; // Resistance force from ground  depending on rotor RPMs
	public Vector3 FrRpm; // Rotor Force depending on rotor RPMs
	public float rpm; // Rotor RPMs rate
	public Vector3 p; // Newton weigth const force, mass * gravity
	public Vector3 variationMotionSpeed; // Speed variation vector, acceleration by dTime => dV = (Sum(Kopter Forces) / mass) * dT
	public Vector3 motionSpeed; // Force motion speed
	public RaycastHit floorInfos;
	public Vector3 deltaPositionAdjust; // position adjustement for next frame. Get back to V.zero on use
	
	/// <summary>
	/// On this game object Awake 
	/// </summary>
	void Awake ()
	{
		Instance = this;
	}
	
	/// <summary>
	/// On this game object Start
	/// </summary>
	void Start ()
	{
		// Memeber initialization
		p = mass * gravity;
		R = Vector3.zero;
		FrRpm = Vector3.zero;
		rpm = 50.0f;
		motionSpeed = Vector3.zero;
		strafeRotation = Quaternion.identity;
		deltaPositionAdjust = Vector3.zero;
	}

	/// <summary>
	/// ControlsMessage Receiver from GamePad, keyboard, mouse...
	/// </summary>
	/// <param name="controlMessage">Control message.</param>
	public void controlsReceiver (ControlMessage controlMessage) {
		Debug.Log("Message received " + controlMessage.ToString());
		this.ThrottleInput = controlMessage.ThrottleInput;

	}


	/// <summary>
	/// Updates the motor.
	/// </summary>
	public void Update ()
	{
		// Getting information from floor. distance, hit normal surface, ...
		// Is Kopter in air ?
		getFloorInfos ();
		
		// Orienting Kopter based on User inputs
		listenForOrientationUpdates ();
		
		// Compute Rpms
		rpm = rpm + throttleInput * throttleAccelerationFactor * Time.deltaTime;
		rpm = Mathf.Clamp (rpm, 0, kRpmMax); // Clamping:  0 <= rpm <= kRpmMax
		
		//Uncomment this lerping down the RPMs, for gameplay purpose....
		//lerpingDownRPMs ();
		
		// Compute Force initiated by Rotor
		FrRpm = (((-1 * p) * rpm) / kRpmLift);
		FrRpm = Quaternion.FromToRotation (Vector3.up, transform.up) * FrRpm; // Set rotor force colinnear to self Vector3 up
		FrRpm = FrRpm * kRotorSurfaceForceFactor;
		
		// Ground resistance
		int isRApplied = (p.magnitude <= FrRpm.y || isFlying) ? 0 : 1;
		R = -1 * (FrRpm + p) * isRApplied;
		
		// Speed variation
		variationMotionSpeed = ((R + FrRpm) / mass 
									+ gravity
								) * Time.deltaTime;

		// Apply speed variation to current speed to find next speed
		motionSpeed = motionSpeed + variationMotionSpeed * variationMotionSpeedFactor;
	
		// Clamp speed
		motionSpeed = Vector3.ClampMagnitude (motionSpeed, clampSpeed);
		
		// Seeking for position variation. Because dP = Speed * dT;
		Vector3 deltaPosition = motionSpeed * Time.deltaTime;
		
		
		
		// Now move
		transform.position += deltaPosition + deltaPositionAdjust; // Equals to -> transform.Translate (deltaPosition, Space.World); 
		
		// Cleaning position adjust for the next frame
		if(deltaPositionAdjust.sqrMagnitude > 0) deltaPositionAdjust = Vector3.zero;

		// Update rotor speed
		Rotor.Instance.UpdateRotor (rpm);
		
	}
	
	/// <summary>
	/// Gets the floor infos.
	/// </summary>
	void getFloorInfos ()
	{
		Vector3 raySource = transform.position + Vector3.down * 2;
		Vector3 rayDirection = -Vector3.up;
		bool hasHit = Physics.Raycast (raySource, rayDirection, out floorInfos);
		if (!hasHit) Reset ();
		isFlying = (floorInfos.distance > groundValue) ? true : false;
	}
	
	/// <summary>
	/// Listens for orientation updates.
	/// </summary>

	void listenForOrientationUpdates ()
	{
		// Find out Kopter orientation based on :
		// > strafingVectorInput wanted up/down/left/right
		// > self rotation input wanted (Q/D)
		
		// Is Kopter in air
		if (isFlying) {
			Quaternion rotationVariation = findRotationVariation ();
			transform.rotation = transform.rotation * rotationVariation; 	// Apply now all the news orientations
			
		} else {	// We have landed !
		
			
			if(	motionSpeed.magnitude > landingMagnitudeValue
				&&(Vector3.Angle(-FloorInfos.normal, motionSpeed) > landingAngleMax)) {
				Debug.Log("Wrong land @ motionSpeed.magntitude=" + motionSpeed.magnitude + " Angle= "+ Vector3.Angle(-FloorInfos.normal, motionSpeed));
				Reset();
			}
			
			motionSpeed = Vector3.zero;
			variationMotionSpeed = Vector3.zero;
			strafeRotation = Quaternion.identity;
			
			// Adjust Kopter orientation from landing surface
			Quaternion rotationVariationGoal = Quaternion.FromToRotation (	transform.up, 
																			floorInfos.normal);
			// Lerp orientation to normal orientation surface
			transform.rotation = Quaternion.Lerp (	transform.rotation,
													transform.rotation * rotationVariationGoal,
													Time.deltaTime * 25f);
			
			// compture position Ajust for regain good position from floor
			deltaPositionAdjust.y = groundValue - floorInfos.distance;
			
			// Ommit if not enough delta
			if(deltaPositionAdjust.y < 0.01f) deltaPositionAdjust = Vector3.zero;
		}
		
	}
	
	/// <summary>
	/// Finds the rotation variation.
	/// </summary>
	/// <returns>
	/// The rotation variation.
	/// </returns>
	Quaternion findRotationVariation ()
	{
	
		if (Mathf.Abs (selfRotationInput) > 0.0f) { // local yaw rotation wanted
			yawRotation = Mathf.Lerp (yawRotation, selfRotationInput * yawRotationAccelFactor, Time.deltaTime);
		} else { // Lerping yaw angular speed to zero
			yawRotation = Mathf.Lerp (yawRotation, 0.0f, Time.deltaTime / yawRotationDeccelFactor);
		}
		
		// Find out now yaw target
		Quaternion yawRotationTarget = Quaternion.Euler (0, yawRotation, 0);

		if (strafingVectorInput.sqrMagnitude > 0.0f) { // If strafing wanted
		
			strafingVectorInput *= strafeRotationAccelFactor;
		
			strafeRotation = Quaternion.Lerp (
			strafeRotation, 
			Quaternion.Euler (strafingVectorInput.z, 0, strafingVectorInput.x),
			Time.deltaTime * 2);
			
		} else { // Pilot don't want to strafe anymore...
			strafeRotation = Quaternion.Lerp (
			strafeRotation,
			Quaternion.identity,
			Time.deltaTime * 1.5f);
		}
			
		return strafeRotation * yawRotationTarget;
	}
	
	/// <summary>
	/// Reset kopter to default position and values.
	/// </summary>
	public void Reset ()
	{
		rpm = KRpmLift;
		R = Vector3.zero;
		motionSpeed = Vector3.forward * 10;
		variationMotionSpeed = Vector3.zero;
		transform.position = new Vector3 (0, 10.05f, 8f);
		transform.rotation = Quaternion.identity;
	}
	
	/// <summary>
	/// Raises the draw gizmos event.
	/// </summary>
	void OnDrawGizmos ()
	{
//		Debug.DrawRay(transform.position, FrRpm, Color.red);
//		Debug.DrawRay(transform.position, motionSpeed, Color.cyan);
	}

	void lerpingDownRPMs ()
	{
		if (throttleInput == 0.0f) {
			if (IsFlying) {
				rpm = Mathf.Lerp (rpm, kRpmLift / 2, Time.deltaTime / 3); // Slowly Lerping down RPMs in air for gameplay purpose
			} else {
				rpm = Mathf.Lerp (rpm, 0, Time.deltaTime / 5); // Slowly Lerping down RPMs on ground
			}
		}
	}

	#region Get/Set
	
	public Vector3 Gravity {
		get {
			return this.gravity;
		}
		set {
			gravity = value;
		}
	}

	public float KRotorSurfaceForceFactor {
		get {
			return this.kRotorSurfaceForceFactor;
		}
		set {
			kRotorSurfaceForceFactor = value;
		}
	}

	public float KRpmLift {
		get {
			return this.kRpmLift;
		}
		set {
			kRpmLift = value;
		}
	}

	public float KRpmMax {
		get {
			return this.kRpmMax;
		}
		set {
			kRpmMax = value;
		}
	}

	public float Mass {
		get {
			return this.mass;
		}
		set {
			mass = value;
		}
	}

	public Vector3 MotionSpeed {
		get {
			return this.motionSpeed;
		}
		set {
			motionSpeed = value;
		}
	}

	public Vector3 P {
		get {
			return this.p;
		}
		set {
			p = value;
		}
	}

	public float Rpm {
		get {
			return this.rpm;
		}
		set {
			rpm = value;
		}
	}

	public float SelfRotationInput {
		get {
			return this.selfRotationInput;
		}
		set {
			selfRotationInput = value;
		}
	}

	public Quaternion StrafeRotation {
		get {
			return this.strafeRotation;
		}
		set {
			strafeRotation = value;
		}
	}

	public float StrafeRotationAccelFactor {
		get {
			return this.strafeRotationAccelFactor;
		}
		set {
			strafeRotationAccelFactor = value;
		}
	}

	public Vector3 StrafingVectorInput {
		get {
			return this.strafingVectorInput;
		}
		set {
			strafingVectorInput = value;
		}
	}

	public float ThrottleAccelerationFactor {
		get {
			return this.throttleAccelerationFactor;
		}
		set {
			throttleAccelerationFactor = value;
		}
	}

	public float ThrottleInput {
		get {
			return this.throttleInput;
		}
		set {
			throttleInput = value;
		}
	}

	public Vector3 VariationMotionSpeed {
		get {
			return this.variationMotionSpeed;
		}
		set {
			variationMotionSpeed = value;
		}
	}

	public float VariationMotionSpeedFactor {
		get {
			return this.variationMotionSpeedFactor;
		}
		set {
			variationMotionSpeedFactor = value;
		}
	}

	public float YawRotation {
		get {
			return this.yawRotation;
		}
		set {
			yawRotation = value;
		}
	}

	public float YawRotationAccelFactor {
		get {
			return this.yawRotationAccelFactor;
		}
		set {
			yawRotationAccelFactor = value;
		}
	}

	public float YawRotationDeccelFactor {
		get {
			return this.yawRotationDeccelFactor;
		}
		set {
			yawRotationDeccelFactor = value;
		}
	}

	public float ClampSpeed {
		get {
			return this.clampSpeed;
		}
		set {
			clampSpeed = value;
		}
	}

	public bool IsFlying {
		get {
			return this.isFlying;
		}
		set {
			isFlying = value;
		}
	}

	public RaycastHit FloorInfos {
		get {
			return this.floorInfos;
		}
		set {
			floorInfos = value;
		}
	}
	#endregion
}


