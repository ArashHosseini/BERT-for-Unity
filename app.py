
from api.bert_huggingface_app import app as flask_app
#import production server
from waitress import serve
if __name__ == "__main__":  
    #run production WSGI "waitress" server with default port 5000
    serve(flask_app, host='localhost', port=5000)
