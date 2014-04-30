using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HttpRest
{
	public enum SendMethod
	{
		POST,
		GET,
		NONE
	}
	
	private WWW httpResponse;
	private String url;
	private IDictionary<String, String> parameters;
	private SendMethod method;

	public HttpRest ()
	{
		//Debug.Log ("cons");
		this.method = SendMethod.NONE;
	}
	
	public IEnumerator call ()
	{	
		if (method == SendMethod.NONE) {
			string message = "No method mode have been setup: POST/GET. Use HttpRest.Method for that.";
			throw new UnityException (message);	
		}
		
		switch (method) {
		case SendMethod.GET:
			handleGetCall ();
			break;
		case SendMethod.POST:
			handlePostCall ();
			break;
		}
		
		yield return httpResponse; // Wait for http response
		
		if (httpResponse.error != null) {
			Debug.LogError (httpResponse.error);
		}
	}

	void handleGetCall ()
	{
		if (parameters.Count > 0) {
			url += "?";
			foreach (KeyValuePair<String, String> pair in parameters) {
				url += pair.Key + "=" + pair.Value + "&";
			}
		}
		
		httpResponse = new WWW (url);
	}
	
	void handlePostCall ()
	{
		
		WWWForm form = new WWWForm ();
		
		foreach (KeyValuePair<String, String> pair in parameters) {
			form.AddField (pair.Key, pair.Value);
		}
		httpResponse = new WWW (url, form);
	}
	
	public bool isSuccess ()
	{
		return (httpResponse.error == null);
	}
	
	public WWW HttpResponse {
		get {
			if (httpResponse.isDone) {
				return this.httpResponse;
			}
			return null;
		}
		set {
			httpResponse = value;
		}
	}

	public IDictionary<String, String> Parameters {
		get {
			return this.parameters;
		}
		set {
			parameters = value;
		}
	}

	public String Url {
		get {
			return this.url;
		}
		set {
			url = value;
		}
	}

	public SendMethod Method {
		get {
			return this.method;
		}
		set {
			method = value;
		}
	}
}
