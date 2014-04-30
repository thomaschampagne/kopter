using UnityEngine;
using System.Collections;

public class Rotor : MonoBehaviour
{
	
	public static Rotor Instance { get; set; }
	
	private GameObject  MainRotor, Stator;
	
	void Awake ()
	{
		Instance = this;
		MainRotor = GameObject.Find ("MainRotor");
		Stator = GameObject.Find ("HeckRotor");
	}
	
	// Update is called once per frame
	public void UpdateRotor (float rpm)
	{
		float ratePerSecond = rpm / 60;
		
		MainRotor.transform.Rotate(Vector3.up * ratePerSecond * 360 * Time.deltaTime);
		Stator.transform.Rotate(Vector3.up * ratePerSecond * 720 * Time.deltaTime);
		
	}
	
	
}
