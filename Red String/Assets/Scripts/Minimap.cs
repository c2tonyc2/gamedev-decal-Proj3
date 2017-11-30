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

	private float world2UI;

	// Use this for initialization
	void Start () {
		float realDistance = Mathf.Abs (GoalPost1.transform.position.x) + Mathf.Abs (GoalPost2.transform.position.x);
		float UIdistance = Mathf.Abs (Goal1UI.anchoredPosition.x) + Mathf.Abs (Goal2UI.anchoredPosition.x);
		world2UI = UIdistance / realDistance;
	}

	// Update is called once per frame
	void Update () {
		Player1UI.anchoredPosition = new Vector2 (Player1.transform.position.x * world2UI, Goal1UI.anchoredPosition.y);
		Player2UI.anchoredPosition = new Vector2 (Player2.transform.position.x * world2UI, Goal2UI.anchoredPosition.y);
	}
}
