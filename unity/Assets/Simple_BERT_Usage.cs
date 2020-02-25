using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using HuggingFace;

public class Simple_BERT_Usage : MonoBehaviour {

	public string flask_url = "http://localhost:5000";
	public Button feature_extraction_btn;
	public Button sentiment_analysis_btn;
	public Button question_answering_btn;
	public Button next_sentence_btn;
	public Button fill_mask_btn;

	Pipeline transformers = new Pipeline();
	
	ArrayList feature_extraction_queue = new ArrayList ();
	ArrayList sentiment_analysis_queue = new ArrayList ();
	ArrayList q_a_queue = new ArrayList ();
	ArrayList next_sentence_queue = new ArrayList ();
	ArrayList fill_mask_queue = new ArrayList ();

	void Start () {
		feature_extraction_btn.onClick.AddListener(feature_extraction);
		sentiment_analysis_btn.onClick.AddListener(sentiment_analysis);
		question_answering_btn.onClick.AddListener(question_answering);
		next_sentence_btn.onClick.AddListener(next_sentence);
		fill_mask_btn.onClick.AddListener(fill_mask);
	}
	
	public void feature_extraction(){
		// Generates a tensor representation for the input sequence
		StartCoroutine(transformers.task("feature_extraction","i love you",flask_url,feature_extraction_queue));
	}

	public void sentiment_analysis(){
		// Gives the polarity (positive / negative) of the whole input sequence.
		StartCoroutine(transformers.task("sentiment_analysis","i love you",flask_url,sentiment_analysis_queue));
	}

	public void question_answering(){
		// Provided some context and a question refering to the context, it will extract the answer to the question in the context.
		StartCoroutine(transformers.task("question_answering","Who was Jim Henson?#Jim Henson was a nice puppet",flask_url,q_a_queue));	
	}

	public void next_sentence(){
		// Provide the next N sentences for the input sequence, it will consider the return as the new input during iteration.
		StartCoroutine(transformers.task("next_sentence","I never thought it would be this hard to create #3",flask_url,next_sentence_queue));
	}

	public void fill_mask(){
		//Takes an input sequence containing a masked token (e.g. <mask>) and return list of most probable filled sequences, with their probabilities.
		StartCoroutine(transformers.task("fill_mask","I never thought it would be this <mask> to build a house",flask_url,fill_mask_queue));
	}

	void Update () {
		if (feature_extraction_queue.Count >= 1){
			string feature_extraction_payload = (string)feature_extraction_queue [0];
			feature_extraction_queue.RemoveRange (0, 1);
			float[] tensor = transformers.convert_to_tensor(feature_extraction_payload);
			Debug.Log(tensor[0]);
		}

		if (sentiment_analysis_queue.Count >= 1){
			Dictionary<string, string> payload_dict;
			string sentiment_analysis_payload = (string)sentiment_analysis_queue [0];
			sentiment_analysis_queue.RemoveRange (0, 1);
			payload_dict = transformers.convert_to_dict(sentiment_analysis_payload);
			Debug.Log(payload_dict["label"]);
			Debug.Log(payload_dict["score"]);
		}

		if (q_a_queue.Count >= 1){
			Dictionary<string, string> q_a_payload_dict;
			string q_a_payload = (string)q_a_queue [0];
			q_a_queue.RemoveRange (0, 1);
			q_a_payload_dict = transformers.convert_to_dict(q_a_payload);
			Debug.Log(q_a_payload_dict["score"]);
			Debug.Log(q_a_payload_dict["start"]);
			Debug.Log(q_a_payload_dict["end"]);
			Debug.Log(q_a_payload_dict["answer"]);
		}

		if (next_sentence_queue.Count >= 1){
			Dictionary<string, string> next_sentence_payload_dict;
			string next_sentence_payload = (string)next_sentence_queue [0];
			next_sentence_queue.RemoveRange (0, 1);
			next_sentence_payload_dict = transformers.convert_to_dict(next_sentence_payload);
			Debug.Log(next_sentence_payload_dict["next_sentence"]);
		}

		if (fill_mask_queue.Count >= 1){
			Dictionary<string, string> fill_mask_payload_dict;
			string fill_mask_payload = (string)fill_mask_queue [0];
			fill_mask_queue.RemoveRange (0, 1);
			fill_mask_payload_dict = transformers.convert_to_dict(fill_mask_payload);
			Debug.Log(fill_mask_payload_dict["sequence"]);
			Debug.Log(fill_mask_payload_dict["score"]);
		}

	}

}
