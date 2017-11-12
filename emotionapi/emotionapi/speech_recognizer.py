#!/usr/bin/env python3

# NOTE: this example requires PyAudio because it uses the Microphone class

import time
import speech_recognition as sr
import threading
import indicoio

class SpeechRecognizer(object):

    def __init__(self):
        self.recognizer = sr.Recognizer()
        self.microphone = sr.Microphone()
        with self.microphone as source:
            self.recognizer.adjust_for_ambient_noise(source)
        self.stop_listening = self.recognizer.listen_in_background(self.microphone, self.callback)

    def getSpeechEmotions(self):
        self.stop_listening()
        self.stop_listening = self.recognizer.listen_in_background(self.microphone, self.callback)

    def getTextEmotions(self, text):
        indicoio.config.api_key = '0b5758fa7c1eadd3cac6d5d5eee7ed4d'
        emotion_scores = indicoio.emotion(text)
        return max(emotion_scores, key=emotion_scores.get)

    # this is called from the background thread
    def callback(self, recognizer, audio):
        # received audio data, now we'll recognize it using Google Speech Recognition
        try:
            # for testing purposes, we're just using the default API key
            # to use another API key, use `r.recognize_google(audio, key='GOOGLE_SPEECH_RECOGNITION_API_KEY')`
            # instead of `r.recognize_google(audio)`
            text = recognizer.recognize_google(audio)
            print(text)
            print(self.getTextEmotions(text))
        except sr.UnknownValueError:
            print('Google Speech Recognition could not understand audio')
        except sr.RequestError as e:
            print('Could not request results from Google Speech Recognition service; {0}'.format(e))

speech = SpeechRecognizer()
# do some other computation for 5 seconds, then stop listening and keep doing other computations
for _ in range(50): time.sleep(0.1)  # we're still listening even though the main thread is doing other things
speech.getSpeechEmotions()  # calling this function requests that the background listener stop listening
while True: time.sleep(0.1)