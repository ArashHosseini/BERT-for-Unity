using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.
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

namespace Network_Base {
    public class Rest {		
		public IEnumerator post (string bodyJsonString, string flask_route, ArrayList queue) {
			Debug.Log(bodyJsonString);
			return post_request(bodyJsonString, flask_route, queue);
		}

	    IEnumerator post_request(string bodyJsonString, string flask_route, ArrayList queue)
		    {
		        var request = new UnityWebRequest(flask_route, "POST");
		        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
		        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
		        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
		        request.SetRequestHeader("Content-Type", "application/json");
		        request.SetRequestHeader("Accept", "application/json");
		        yield return request.SendWebRequest();
		        Debug.Log("Status Code: " + request.responseCode);
		        string res = request.downloadHandler.text;
		        queue.Add(res);
		    }

    }
}