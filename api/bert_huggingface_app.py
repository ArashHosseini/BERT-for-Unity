# -*- coding: utf-8 -*-
import os
import sys
from flask import Flask, request, jsonify, send_file, current_app, make_response, redirect, url_for
from flask_cors import CORS
from . import utilities
from .config import API_TITLE,\
nlp_sentiment_analysis,\
nlp_q_a,\
nlp_feature_extraction,\
gpt_tokenizer,\
gpt_model,\
device,\
nlp_fill_mask
import logging
import ast
import time
from datetime import timedelta
from functools import update_wrapper
import torch
import random

logging.basicConfig(format='%(asctime)s - %(message)s', level=logging.INFO)
# creates a Flask application, named app
app = Flask(__name__, static_folder="static")
app.config['SECRET_KEY'] = 'secret'
app.config['CORS_HEADERS'] = 'Content-Type'

cors = CORS(app, resources={r"/action": {"origins": "http://localhost:5000"},
							r"/predict": {"origins": "http://localhost:5000"}})
#set application config
utilities.setup_config(app)
#set application name
app.config.update(
	APPNAME=API_TITLE,
)
logging.info("CORS support")

def crossdomain(origin=None, methods=None, headers=None,
				max_age=21600, attach_to_all=True,
				automatic_options=True):
	if methods is not None:
		methods = ', '.join(sorted(x.upper() for x in methods))
	if headers is not None and not isinstance(headers, str):
		headers = ', '.join(x.upper() for x in headers)
	if not isinstance(origin, str):
		origin = ', '.join(origin)
	if isinstance(max_age, timedelta):
		max_age = max_age.total_seconds()

	def get_methods():
		if methods is not None:
			return methods

		options_resp = current_app.make_default_options_response()
		return options_resp.headers['allow']

	def decorator(f):
		def wrapped_function(*args, **kwargs):
			if automatic_options and request.method == 'OPTIONS':
				resp = current_app.make_default_options_response()
			else:
				resp = make_response(f(*args, **kwargs))
			if not attach_to_all and request.method != 'OPTIONS':
				return resp

			h = resp.headers

			h['Access-Control-Allow-Origin'] = origin
			h['Access-Control-Allow-Methods'] = get_methods()
			h['Access-Control-Max-Age'] = str(max_age)
			if headers is not None:
				h['Access-Control-Allow-Headers'] = headers
			return resp

		f.provide_automatic_options = False
		return update_wrapper(wrapped_function, f)
	return decorator

def to_dict(request):
	query_parameters = request.form
	if not query_parameters:
		query_parameters = ast.literal_eval(request.data.decode("utf-8"))
	return query_parameters

def generate_sentence(input):
	sentence = input
	context_tokens = gpt_tokenizer.encode(sentence, add_special_tokens=False)
	context = torch.tensor(context_tokens, dtype=torch.long)
	context = context.to(device)
	num_samples = 1
	context = context.unsqueeze(0).repeat(num_samples, 1)
	generated = context
	length = random.randint(10,20)
	temperature = random.uniform(0.7,0.9)
	with torch.no_grad():
		for jj in range(2):
			for _ in range(length):
				outputs = gpt_model(generated)
				next_token_logits = outputs[0][:, -1, :] / (temperature if temperature > 0 else 1.)
				next_token = torch.multinomial(torch.nn.functional.softmax(next_token_logits, dim=-1), num_samples=1)
				generated = torch.cat((generated, next_token), dim=1)

	out = generated
	out = out[:, len(context_tokens):].tolist()

	for o in out:
		_text = gpt_tokenizer.decode(o, clean_up_tokenization_spaces=True)
		try:
			index = _text.index(".")
			text = _text[:index]
			if _text.index(".") == 0:
				#find next dot
				index = _text[1:].index(".")
				text = _text[1:index]
		except ValueError as e:
			pass
		else:
			txt = sentence + text.replace("\n", "") + "."
			return txt

@app.before_first_request
def warm_up_networks():
	print ("warmup")

@app.route("/feature_extraction", methods=['POST', 'GET','OPTIONS'])
@crossdomain(origin='*', headers=['access-control-allow-origin','Content-Type'])
def feature_extraction():
	if request.method == 'POST':
		query_parameters = to_dict(request)
		sentence = query_parameters["input_sentence"]
		tensor = nlp_feature_extraction(sentence)
		response = jsonify({"t0":",".join(str(i) for i in tensor[0][0]),
							"t1":",".join(str(i) for i in tensor[0][1]),
							"t2":",".join(str(i) for i in tensor[0][2])})
		logging.info(response)
		return response

@app.route("/sentiment_analysis", methods=['POST', 'GET','OPTIONS'])
@crossdomain(origin='*', headers=['access-control-allow-origin','Content-Type'])
def sentiment_analysis():
	if request.method == 'POST':
		query_parameters = to_dict(request)
		sentence = query_parameters["input_sentence"]
		sentiment_analysis = nlp_sentiment_analysis(sentence)
		response = jsonify({"score":str(sentiment_analysis[0]["score"]), "label":str(sentiment_analysis[0]["label"])})
		logging.info(response)
		return response

@app.route("/question_answering", methods=['POST', 'GET','OPTIONS'])
@crossdomain(origin='*', headers=['access-control-allow-origin','Content-Type'])
def question_answering():
	if request.method == 'POST':
		query_parameters = to_dict(request)
		answer_payload = nlp_q_a(query_parameters)
		response = jsonify(answer_payload)
		logging.info(response)
		return response

@app.route("/next_sentence", methods=['POST', 'GET','OPTIONS'])
@crossdomain(origin='*', headers=['access-control-allow-origin','Content-Type'])
def next_sentence():
	if request.method == 'POST':
		query_parameters = to_dict(request)
		input_sentence = query_parameters["input_sentence"]
		iter_range = query_parameters["iter"]
		for _ in range(int(iter_range)):
			input_sentence = generate_sentence(str(input_sentence))
		response = jsonify({"next_sentence":input_sentence})
		logging.info(response)
		return response


@app.route("/fill_mask", methods=['POST', 'GET','OPTIONS'])
@crossdomain(origin='*', headers=['access-control-allow-origin','Content-Type'])
def fill_mask():
	if request.method == 'POST':
		query_parameters = to_dict(request)
		input_sentence = query_parameters["input_sentence"]
		answer_payload = nlp_fill_mask(str(input_sentence))
		response = jsonify(answer_payload[0])
		logging.info(response)
		return response 