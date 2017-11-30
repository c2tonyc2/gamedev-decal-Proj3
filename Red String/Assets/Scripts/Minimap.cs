using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour {
	
	public GameObject Player1;
	public GameObject Player2;
	public RectTransform Player1UI;
	public RectTransform Player2UI;
	public GameObject GoalPost1;
	public GameObject GoalPost2;
	public RectTransform Goal1UI;
	public RectTransform Goal2UI;

	private float P1world2UI;
	private float P2world2UI;

	// Use this for initialization
	void Start () {
		float dist1 = Mathf.Abs (Mathf.Abs (GoalPost1.transform.position.x) - Mathf.Abs (Player1.transform.position.x));
		float dist2 = Mathf.Abs (Mathf.Abs (GoalPost2.transform.position.x) - Mathf.Abs (Player2.transform.position.x));
		P1world2UI = Mathf.Abs (Goal1UI.anchoredPosition.x) / Mathf.Abs (dist1);
		P2world2UI = Mathf.Abs (Goal2UI.anchoredPosition.x) / Mathf.Abs (dist2);
	}

	// Update is called once per frame
	void Update () {
		Player1UI.anchoredPosition = new Vector2 (Player1.transform.position.x * P1world2UI, Player1UI.anchoredPosition.y);
		Player2UI.anchoredPosition = new Vector2 (Player2.transform.position.x * P2world2UI, Player2UI.anchoredPosition.y);
	}
}
