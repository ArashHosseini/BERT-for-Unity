using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using HuggingFace;

public class Simple_BERT_App : MonoBehaviour {

	public string flask_url = "http://localhost:5000";

	public Button next_sentence_btn;

	Pipeline transformers = new Pipeline();
	
	ArrayList next_sentence_queue = new ArrayList ();

	void Start () {

		next_sentence_btn.onClick.AddListener(next_sentence);

	}

	public void next_sentence(){
		// Provide the next N sentences for the input sequence, it will consider the return as the new input during iteration.
		StartCoroutine(transformers.task("next_sentence","I never thought it would be this hard to create #3",flask_url,next_sentence_queue));
	}

	void Update () {

		if (next_sentence_queue.Count >= 1){
			Dictionary<string, string> next_sentence_payload_dict;
			string next_sentence_payload = (string)next_sentence_queue [0];
			next_sentence_queue.RemoveRange (0, 1);
			next_sentence_payload_dict = transformers.convert_to_dict(next_sentence_payload);
			Debug.Log(next_sentence_payload_dict["next_sentence"]);

		}

	}

}
