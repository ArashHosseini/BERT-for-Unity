import re
import requests
import os
import uuid
import logging
logging.basicConfig(format='%(asctime)s - %(message)s', level=logging.INFO)

from .config import DEBUG, DOWNLOAD_BASE, PREDICT_BASE, WARMUP_BASE, UPLOAD_FOLDER, ANIMATION_LIBRARY

def setup_config(app):
	#set application config
	app.config['DEBUG'] = DEBUG
	app.config['DOWNLOAD_FOLDER'] = DOWNLOAD_BASE
	app.config['PREDICT_FOLDER'] = PREDICT_BASE
	app.config['WARMUP_BASE'] = WARMUP_BASE
	app.config['UPLOAD_FOLDER'] = UPLOAD_FOLDER
	app.config['ANIMATION_LIBRARY'] = ANIMATION_LIBRARY
