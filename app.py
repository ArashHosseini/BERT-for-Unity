
import argparse
#import production server
from waitress import serve

if __name__ == "__main__":

	parser = argparse.ArgumentParser()
	parser.add_argument('--w', "-webgl",dest = "webgl_app", required=False, type=bool, default=False)
	parsed_args = parser.parse_args()

	if parsed_args.webgl_app:
		from api.bert_huggingface_app_webgl import app as flask_app
	else:
		from api.bert_huggingface_app import app as flask_app
		
	#run production WSGI "waitress" server with default port 5000
	serve(flask_app, host='localhost', port=5000)
