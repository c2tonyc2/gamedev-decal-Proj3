using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap2 : MonoBehaviour {
	
	public GameObject Player1;
	public GameObject Player2;
	public RectTransform Player1UI;
	public RectTransform Player2UI;
	public GameObject GoalPost;
	public RectTransform GoalUI;

	private float P1world2UI;
	private float P2world2UI;

	// Use this for initialization
	void Start () {
		float dist1 = GoalPost.transform.position.x - Player1.transform.position.x;
		float dist2 = GoalPost.transform.position.x - Player2.transform.position.x;
		P1world2UI = Player1UI.anchoredPosition.x / Mathf.Abs (dist1);
		P2world2UI = Player2UI.anchoredPosition.x / Mathf.Abs (dist2);
	}

	// Update is called once per frame
	void Update () {
		float currP1dist = GoalPost.transform.position.x - Player1.transform.position.x;
		float currP2dist = Player2.transform.position.x - GoalPost.transform.position.x;
		Player1UI.anchoredPosition = new Vector2 (currP1dist * P1world2UI, Player1UI.anchoredPosition.y);
		Player2UI.anchoredPosition = new Vector2 (currP2dist * P2world2UI, Player2UI.anchoredPosition.y);
	}
}
