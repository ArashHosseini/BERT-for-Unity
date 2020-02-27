# BERT-for-Unity

<p align="center">
    <img src="/src/usage.gif">
</p>


Bidirectional Encoder Representations from Transformers technique for Unity game engine using [huggingface implementation](https://github.com/huggingface/transformers). This is a server-based interfaces for huggingface transformers "Pipeline" objects. Pipeline are high-level objects which automatically handle tokenization, running your data through a transformers model and outputting the result in a structured object.

# Install 

1. setup virtualenv and activate your environment
2. install [transformers](https://github.com/huggingface/transformers#installation)
3. clone this repository and install the dependencies

```bash
pip install flask
pip install flask_cors
pip install waitress
```

# Usage 

## Server

start app

```bash
cd BERT-for-Unity/
python3 app.py
```
server is tested on Python 3.5+, PyTorch 1.0.0+


## Unity

### Supported pipeline objects 


 - `next-sentence` : Provide the next N sentences for the input sequence, it will consider the return as the new input during iteration.
 - `fill-mask` : Takes an input sequence containing a masked token (e.g. <mask>) and return list of most probable filled sequences, with their probabilities.
 - `question-answering` : Provided some context and a question refering to the context, it will extract the answer to the question in the context.
 - `sentiment-analysis` : Gives the polarity (positive / negative) of the whole input sequence.
 - `feature-extraction` : Generates a tensor representation for the input sequence


```c#

StartCoroutine(transformers.task("next_sentence","I never thought it would be this hard to create #3",flask_url,next_sentence_queue));

StartCoroutine(transformers.task("fill_mask","I never thought it would be this <mask> to build a house",flask_url,next_sentence_queue));

StartCoroutine(transformers.task("question_answering","Who was Jim Henson?#Jim Henson was a nice puppet",flask_url,q_a_queue));

StartCoroutine(transformers.task("sentiment_analysis","i love you",flask_url,sentiment_analysis_queue));

StartCoroutine(transformers.task("feature_extraction","i love you",flask_url,feature_extraction_queue));

```

for more details see `unity/Assets/Simple_BERT_Usage.cs`. Open `unity/Assets/bert_example_scene.unity` to use the example scene. 


### Example BERT webgl app WIP

<p align="left">
    <img src="/src/webgl.gif">
</p>

```bash
cd BERT-for-Unity/
python3 app.py -webgl true
```
and visit `http://localhost:5000`

### Current status, February 25th:
this is the first protype so there are no tests available.