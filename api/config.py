# application metadata
API_TITLE = 'BERT for Unity'
API_DESC = 'This is a prototype web-based wrapper for BERT using Huggingface implementation'

# flask arg description
# set debug False
DEBUG = True

# default assets
DOWNLOAD_BASE = 'assets/downloads'
PREDICT_BASE = 'images/predict'
WARMUP_BASE = 'assets/warmup'
UPLOAD_FOLDER = 'uploads'
ANIMATION_LIBRARY = 'library'


from transformers import pipeline
from transformers import GPT2Tokenizer, GPT2LMHeadModel
import torch

#feature
nlp_sentiment_analysis = pipeline('sentiment-analysis',model='distilbert-base-uncased-finetuned-sst-2-english',tokenizer='distilbert-base-uncased')
# q & a
nlp_q_a = pipeline('question-answering',model='distilbert-base-cased-distilled-squad',tokenizer='distilbert-base-cased', device=0)
nlp_q_a.tokenizer.pad_token = '<PAD>'
nlp_q_a.model.resize_token_embeddings(len(nlp_q_a.tokenizer))
#analysis
nlp_feature_extraction = pipeline('feature-extraction',model='distilbert-base-cased',tokenizer='distilbert-base-cased', device=0)
#next sentence
gpt_tokenizer = GPT2Tokenizer.from_pretrained('gpt2')
gpt_model = GPT2LMHeadModel.from_pretrained('gpt2')
device = torch.device("cuda" if torch.cuda.is_available() else "cpu")
gpt_model.to(device)
gpt_model.eval()
