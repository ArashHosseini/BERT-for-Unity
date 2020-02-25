using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using HuggingFace;

public class HUGGINGFACE_BERT : MonoBehaviour {

	string flask_url = "http://localhost:5000";
	
	Pipeline transformers = new Pipeline();
	
	ArrayList feature_extraction_queue = new ArrayList ();
	ArrayList sentiment_analysis_queue = new ArrayList ();
	ArrayList q_a_queue = new ArrayList ();
	ArrayList next_sentence_queue = new ArrayList ();

	void Start () {
		// // Generates a tensor representation for the input sequence
		// StartCoroutine(transformers.task("feature_extraction",
		// 								"i love you",
		// 								flask_url,
		// 								feature_extraction_queue));

		// Gives the polarity (positive / negative) of the whole input sequence.
		//StartCoroutine(transformers.task("sentiment_analysis","i love you",flask_url,sentiment_analysis_queue));

		// Provided some context and a question refering to the context, it will extract the answer to the question in the context.
		StartCoroutine(transformers.task("question_answering","Who was Jim Henson?#Jim Henson was a nice puppet",flask_url,q_a_queue));

		// // Provide the next N sentences for the input sequence, it will consider the return as input
		// StartCoroutine(transformers.task("next_sentence",
		// 								"Please take the next train to#3",
		// 								flask_url,
		// 								next_sentence_queue));
	}
	
	void Update () {
		// if (feature_extraction_queue.Count >= 1){
		// 	string feature_extraction_payload = (string)feature_extraction_queue [0];
		// 	feature_extraction_queue.RemoveRange (0, 1);
		// 	float[] tensor = transformers.convert_to_tensor(feature_extraction_payload);
		// 	Debug.Log(tensor[0]);
		// }

		// if (sentiment_analysis_queue.Count >= 1){
		// 	Dictionary<string, string> payload_dict;
		// 	string sentiment_analysis_payload = (string)sentiment_analysis_queue [0];
		// 	sentiment_analysis_queue.RemoveRange (0, 1);
		// 	payload_dict = transformers.convert_to_dict(sentiment_analysis_payload);
		// 	Debug.Log(payload_dict["label"]);
		// 	Debug.Log(payload_dict["score"]);
		// }

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

		// if (next_sentence_queue.Count >= 1){
		// 	Dictionary<string, string> next_sentence_payload_dict;
		// 	string next_sentence_payload = (string)next_sentence_queue [0];
		// 	next_sentence_queue.RemoveRange (0, 1);
		// 	next_sentence_payload_dict = transformers.convert_to_dict(next_sentence_payload);
		// 	Debug.Log(next_sentence_payload_dict["next_sentence"]);
		// }
	}

}
