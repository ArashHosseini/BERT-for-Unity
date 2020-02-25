using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using UnityEngine.Networking;
using System.Linq;
using Network_Base;

namespace HuggingFace {

    public class Pipeline {
		private Rest rest = new Rest();
    	private Dictionary<string, string> payload_dict;

    	public IEnumerator task(string task_name, string input_sentence, string flask_url, ArrayList queue){
    		if (task_name=="question_answering"){
    			string[] q_a = input_sentence.Split('#');
    			string payload = string.Format("{{'question':'{0}', 'context':'{1}'}}", q_a[0], q_a[1]);
    			string flask_route = string.Format("{0}/{1}", flask_url, task_name);
    			return rest.post(payload, flask_route, queue);    			
    		} else if (task_name=="next_sentence"){
    			string[] input_sentence_iter = input_sentence.Split('#');
	    		string payload = string.Format("{{'input_sentence':'{0}'}}", input_sentence);
	    		string flask_route = string.Format("{0}/{1}", flask_url, task_name);
    			return rest.post(payload, flask_route, queue);
    		} else {
	    		string payload = string.Format("{{'input_sentence':'{0}'}}", input_sentence);
	    		string flask_route = string.Format("{0}/{1}", flask_url, task_name);
    			return rest.post(payload, flask_route, queue);
    		}
    	}


    	public float[] convert_to_tensor(string payload){
    		payload_dict = convert_to_dict(payload);
			float[] tensor_0 = Array.ConvertAll(payload_dict["t0"].Split(','), float.Parse);
			float[] tensor_1 = Array.ConvertAll(payload_dict["t1"].Split(','), float.Parse);
			float[] tensor_2 = Array.ConvertAll(payload_dict["t2"].Split(','), float.Parse);
			return tensor_0;
    	} 

		public Dictionary<string, string> convert_to_dict(string login_response)
		{
			Dictionary<string, string> payload = JsonConvert.DeserializeObject<Dictionary<string, string>>(login_response);
			return payload;
		}

    	public ArrayList queue(){
    		return new ArrayList ();
    	}
    }
}