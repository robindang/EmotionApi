# -*- coding: utf-8 -*-
"""Public section, including homepage and signup."""
from flask import Blueprint, jsonify

from .emotion_analyzer import EmotionAnalyzer
from .speech_recognizer import SpeechRecognizer
import os.path

blueprint = Blueprint('api', __name__, static_folder='../static')

@blueprint.route('/api/emotions', methods=['GET', 'POST'])
def get_emotions():
    model_json = get_path('model.json')
    model_h5 = get_path('model.h5')
    cascade = get_path('haarcascade_frontalface_default.xml')
    emotion_analyzer = EmotionAnalyzer(model_json, model_h5, cascade)
    emotions = emotion_analyzer.get_emotions()
    print('ads')
    print(emotions)
    return jsonify(emotions)

@blueprint.route('/api/speech', methods=['GET', 'POST'])
def get_speech():
    speech_recognizer = SpeechRecognizer()
    speech = speech_recognizer.getEmotions()
    return jsonify({"text": speech[0], "emotions":speech[1]})

def get_path(file_name):
    return os.path.dirname(__file__) + '/../' + file_name
