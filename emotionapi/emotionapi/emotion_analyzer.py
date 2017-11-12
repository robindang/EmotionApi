import cv2
import sys
import json
import numpy as np
from keras.models import model_from_json

class EmotionAnalyzer(object):
    emotions = ['angry', 'fear', 'happy', 'sad', 'surprise', 'neutral']

    def __init__(self, model_json, model_h5, casc_path):
        self.face_cascade = cv2.CascadeClassifier(casc_path)
        self.nose_cascade = cv2.CascadeClassifier(casc_path)

        json_file = open(model_json, 'r')
        loaded_model_json = json_file.read()
        json_file.close()
        self.model = model_from_json(loaded_model_json)
        self.model.load_weights(model_h5)
        self.video_capture = cv2.VideoCapture(0)
        ret, frame = self.video_capture.read()
        self.img_gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY, 1)

    def get_emotions(self):
        faces = self.face_cascade.detectMultiScale(
            self.img_gray,
            scaleFactor=1.1,
            minNeighbors=5,
            minSize=(30, 30),
            flags=cv2.CASCADE_SCALE_IMAGE
            )

        #only do 1st face
        for (x, y, w, h) in faces:
            face_image_gray = self.img_gray[y:y+h, x:x+w]
            return self.predict_emotion(face_image_gray)

    def predict_emotion(self, face_image_gray): # a single cropped face
        resized_img = cv2.resize(face_image_gray, (48,48), interpolation = cv2.INTER_AREA)
        # cv2.imwrite(str(index)+'.png', resized_img)
        image = resized_img.reshape(1, 1, 48, 48)
        list_of_list = self.model.predict(image, batch_size=1, verbose=1)
        angry, fear, happy, sad, surprise, neutral = [prob for lst in list_of_list for prob in lst]
        return self.choose_emotion([angry, fear, happy, sad, surprise, neutral])

    def choose_emotion(self, emotions):
        predicted_emotion_index = emotions.index(min(emotions))
        return self.emotions[predicted_emotion_index]


    def stop():
        self.video_capture.release()
        cv2.destroyAllWindows()
