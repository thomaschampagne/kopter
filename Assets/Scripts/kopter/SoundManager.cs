using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	
	protected AudioSource audioSourceCopter;
	
	// Use this for initialization
	void Start () {
		audioSourceCopter = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		float rpmRatioPitch = (Motor.Instance.Rpm / Motor.Instance.KRpmMax) * 6.7f;
		audioSourceCopter.pitch = rpmRatioPitch; // pitch 0 - 2
	}
}

