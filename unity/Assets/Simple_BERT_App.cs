using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using HuggingFace;

public class Simple_BERT_App : MonoBehaviour {

	public string flask_url = "http://localhost:5000";

	public Button next_sentence_btn;
	public InputField init_sentence;
	public Text output;
	public Slider n_sentences_slider;
	public Text slider_value;

	Pipeline transformers = new Pipeline();
	
	ArrayList next_sentence_queue = new ArrayList ();

	void Start () {
		next_sentence_btn.onClick.AddListener(next_sentence);
		n_sentences_slider.onValueChanged.AddListener(delegate {frame_change();});

	}

	public void next_sentence(){
		string input_sentence = init_sentence.text;
		string n_sentences_repeat = n_sentences_slider.value.ToString();
		// Provide the next N sentences for the input sequence, it will consider the return as the new input during iteration.
		StartCoroutine(transformers.task("next_sentence",input_sentence+" #"+n_sentences_repeat,flask_url,next_sentence_queue));
	}

	public void frame_change(){
		slider_value.text = n_sentences_slider.value.ToString() + " sentences";
	}

	void Update () {

		if (next_sentence_queue.Count >= 1){
			Dictionary<string, string> next_sentence_payload_dict;
			string next_sentence_payload = (string)next_sentence_queue [0];
			next_sentence_queue.RemoveRange (0, 1);
			next_sentence_payload_dict = transformers.convert_to_dict(next_sentence_payload);
			output.text = "BERT: " + next_sentence_payload_dict["next_sentence"];
			Debug.Log(next_sentence_payload_dict["next_sentence"]);

		}

	}

}
