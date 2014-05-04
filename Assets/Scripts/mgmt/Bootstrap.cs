using UnityEngine;
using System.Collections;


public class Bootstrap : MonoBehaviour
{

	private GameObject gameManager;
	
	void Start ()
	{
		Debug.Log ("Bootstrap loaded");
		Application.targetFrameRate = 120;
		Object gameManagerPrefab = (Object) Resources.Load("prefabs/GameManager");
		gameManager = (GameObject) Instantiate (gameManagerPrefab);
		gameManager.name = "GameManager";
		GameManager.Instance.CurrentLevel = Level.Bootstrap;
		Destroy (gameObject);
	}
}