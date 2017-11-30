using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalPost : MonoBehaviour {
	public GameObject screenTransition;
	public string levelToLoad;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D other) {
		print (other.gameObject.tag);
		if (other.gameObject.tag == "Player") {
			screenTransition.SetActive (true);
			Invoke ("LoadNextScene", 2);
		}
	}

	void LoadNextScene () {
		SceneManager.LoadSceneAsync (levelToLoad);
	}
}
