using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public Text nameText1;
	public Text dialogueText1;

	public Text nameText2;
	public Text dialogueText2;

	public Animator animator1;
	public Animator animator2;

	private Queue<string> sentences1;
	private Queue<string> sentences2;
	private Queue<string> s;
	private int box;
	private Animator a;

	// Use this for initialization
	void Start () {
		sentences1 = new Queue<string> ();
		sentences2 = new Queue<string> ();
	}

	public void StartDialogue(Dialogue dialogue)
	{



		if (dialogue.box == 1) {
			nameText1.text = dialogue.name;
			s = sentences1;
			box = 1;
			a = animator1;

		} else {
			nameText2.text = dialogue.name;
			s = sentences2;
			box = 2;
			a = animator2;
		}

		s.Clear ();
		a.SetBool ("IsOpen", true);

		foreach (string sentence in dialogue.sentences) 
		{
			s.Enqueue (sentence);
		}

		DisplayNextSentence ();
	}

	public void DisplayNextSentence() {
		if (s.Count == 0) {
			EndDialogue ();
			return;
		}

		string sentence = s.Dequeue ();

		if (box == 1) {
			dialogueText1.text = sentence;

		} else {
			dialogueText2.text = sentence;
		}

	}

	void EndDialogue()
	{
		a.SetBool ("IsOpen", false);
	}
}
