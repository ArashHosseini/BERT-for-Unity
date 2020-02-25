# BERT-for-Unity (WIP)
Bidirectional Encoder Representations from Transformers technique for Unity game engine using [huggingface implementation](https://github.com/huggingface/transformers). This server-based work is largely based on huggingface "Pipeline" objects which are high-level objects which automatically handle tokenization, running your data through a transformers model and outputting the result in a structured object.

# Install 

This repo is tested on Python 3.5+, PyTorch 1.0.0+

1. setup virtualenv and activate your environment
2. install [transformers](https://github.com/huggingface/transformers#installation)
3. clone this repository and install the dependencies

```bash
pip install flask
pip install flask_cors
pip install waitress
```

# Usage 

```c#
// Generates a tensor representation for the input sequence
StartCoroutine(transformers.task("feature_extraction",
								"i love you",
								flask_url,
								feature_extraction_queue));
// Gives the polarity (positive / negative) of the whole input sequence.
StartCoroutine(transformers.task("sentiment_analysis",
								"i love you",
								flask_url,
								sentiment_analysis_queue));
// Provided some context and a question refering to the context, it will extract the answer to the question in the context.
StartCoroutine(transformers.task("question_answering",
								@"What is the name of the repository ?#
								Pipeline have been included in the transformers/transformers repository",
								flask_url,
								q_a_queue));
// Provide the next N sentences for the input sequence, it will consider the return as input
StartCoroutine(transformers.task("next_sentence",
								"Please take the next train to#3",
								flask_url,
								next_sentence_queue));
```
more details in `unity/Assets/HUGGINGFACE_BERT.cs`


# Example Webgl game



# Current status, February 25th:
this is the first protype so there are no tests available.