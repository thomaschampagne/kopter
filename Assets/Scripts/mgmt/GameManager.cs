using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; set; } // Provide public static access of GameManager
	
	public GameState CurrentGameState { get; set; }
	public Level CurrentLevel { get; set; }
	protected bool gamePaused; // Not used yet
	protected String gameSessionId = null;

	void Awake ()
	{
		Debug.Log ("GameManager Awaking");

		if (Instance != null) {
			throw new UnityException ("GameManager Instance already exist !");
		} else {
			Instance = this;        
		}
		DontDestroyOnLoad (this); // keep running as a singleton-like
	}
	
	// Use this for initialization
	void Start ()
	{
//		CurrentGameState = GameState.Splash;
//		CurrentLevel = Level.Splash;
		
		startLoadingLevel(Level.Splash);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		switch (CurrentGameState) {
			case GameState.Splash:
				handleSplash ();
				break;
			case GameState.InGame:
				handleInGame ();
				break;
		}
	}

	void handleSplash ()
	{
		// Track a new game session
		if (Application.platform != RuntimePlatform.WindowsEditor) {
//			StartCoroutine (registerGamesession ());
		}
		
		// Load main level
		startLoadingLevel (Level.main); // GameManager Ready setting first state and load scene	
	}
	
	void handleInGame ()
	{
		//... Nothing yet	
	}
	
	void OnLevelWasLoaded (int level)
	{
		Debug.Log ("Load level finished");
		
		if(CurrentLevel == Level.Splash) {
			
			Debug.Log ("Level is "+ CurrentLevel.ToString());
			CurrentGameState = GameState.Splash;
			
		} else if (CurrentLevel == Level.main) {
			CurrentGameState = GameState.InGame;
			Debug.Log ("Level is "+ CurrentLevel.ToString() + ", setting up scene.");
			prepareScene ();
		}
	}

	public void startLoadingLevel (Level lvl)
	{
		CurrentLevel = lvl;
		Application.LoadLevel (lvl.ToString ());
	}

	public void prepareScene ()
	{
		
		GameObject kopterPrefab = (GameObject)GameObject.Instantiate (Resources.Load ("prefabs/Kopter"));
		kopterPrefab.name = K.kopterName;
		GameObject spawnPoint = GameObject.Find ("SpawnPoint");
		
		if (spawnPoint != null) {
			kopterPrefab.transform.position = spawnPoint.transform.position;
			kopterPrefab.transform.rotation = spawnPoint.transform.rotation;
		}
	}
    
	void OnApplicationQuit ()
	{
//		if (Application.platform != RuntimePlatform.WindowsEditor)
//			yield return StartCoroutine(endGamesession ());
	}
	
	// Use this for initialization

//	IEnumerator registerGamesession ()
//	{
//        
//		Dictionary<String, String> parameters = new Dictionary<String, String> ();
//		/**
//        * GET Params
//        */
//		parameters.Add ("method", "newSession");
//		parameters.Add ("version", K.version.ToString());
//		parameters.Add ("unityVersion", Application.unityVersion);
//		parameters.Add ("hostname", SystemInfo.deviceName);
//		parameters.Add ("os", SystemInfo.operatingSystem);
//		parameters.Add ("platform", Application.platform.ToString());
//		parameters.Add ("resolution", Screen.width + ":" + Screen.height);
//		parameters.Add ("gc", SystemInfo.graphicsDeviceName);
//		parameters.Add ("cpu", SystemInfo.processorType);
//		parameters.Add ("devicemodel", SystemInfo.deviceModel);
//
//		HttpRest httpRest = new HttpRest ();
//		httpRest.Method = HttpRest.SendMethod.POST;
//		httpRest.Url = K.wsUrl;
//		httpRest.Parameters = parameters;
//                
//		yield return StartCoroutine(httpRest.call());   // Wait for webservice reponse
//                	
//		if (httpRest.isSuccess ()) {
//			
//			JSONObject jsonParser = new JSONObject (httpRest.HttpResponse.text);
//			JSONObject r = jsonParser.GetField ("r");
//			gameSessionId = r.ToString ().Replace ("\"", "");
//			Debug.Log ("gameSession is : " + gameSessionId);
//		} else {
//			Debug.LogError ("No gameSession, error: " + httpRest.HttpResponse.error);
//		}
//	}
//	
//	IEnumerator endGamesession ()
//	{
//		
//		Dictionary<String, String> parameters = new Dictionary<String, String> ();
//		parameters.Add ("method", "endSession");
//		parameters.Add ("sessionId", gameSessionId);
//		
//		HttpRest httpRest = new HttpRest ();
//		httpRest.Method = HttpRest.SendMethod.POST;
//		httpRest.Url = K.wsUrl;
//		httpRest.Parameters = parameters;
//                
//		yield return StartCoroutine(httpRest.call());   // Wait for webservice reponse
//                	
//		if (httpRest.isSuccess ()) {
//			Debug.Log ("END session response is : " + httpRest.HttpResponse.text);
//			
//		} else {
//			Debug.LogError ("END session error is : " + httpRest.HttpResponse.error);
//		}
//        
//	}
}


