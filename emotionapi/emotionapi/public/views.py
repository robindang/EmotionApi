# -*- coding: utf-8 -*-
"""Public section, including homepage and signup."""
from flask import Blueprint, flash, redirect, render_template, request, url_for, jsonify
from flask_login import login_required, login_user, logout_user

from emotionapi.extensions import login_manager
from emotionapi.public.forms import LoginForm
from emotionapi.user.forms import RegisterForm
from emotionapi.user.models import User
from emotionapi.utils import flash_errors
from emotionapi.emotion_analyzer import EmotionAnalyzer
import os.path

blueprint = Blueprint('public', __name__, static_folder='../static')


@login_manager.user_loader
def load_user(user_id):
    """Load user by ID."""
    return User.get_by_id(int(user_id))


@blueprint.route('/', methods=['GET', 'POST'])
def home():
    """Home page."""
    form = LoginForm(request.form)
    # Handle logging in
    if request.method == 'POST':
        if form.validate_on_submit():
            login_user(form.user)
            flash('You are logged in.', 'success')
            redirect_url = request.args.get('next') or url_for('user.members')
            return redirect(redirect_url)
        else:
            flash_errors(form)
    return render_template('public/home.html', form=form)


@blueprint.route('/logout/')
@login_required
def logout():
    """Logout."""
    logout_user()
    flash('You are logged out.', 'info')
    return redirect(url_for('public.home'))


@blueprint.route('/register/', methods=['GET', 'POST'])
def register():
    """Register new user."""
    form = RegisterForm(request.form)
    if form.validate_on_submit():
        User.create(username=form.username.data, email=form.email.data, password=form.password.data, active=True)
        flash('Thank you for registering. You can now log in.', 'success')
        return redirect(url_for('public.home'))
    else:
        flash_errors(form)
    return render_template('public/register.html', form=form)


@blueprint.route('/about/')
def about():
    """About page."""
    form = LoginForm(request.form)
    return render_template('public/about.html', form=form)


@blueprint.route('/api/emotions', methods=['GET', 'POST'])
def get_emotions():
    model_json = get_path('model.json')
    model_h5 = get_path('model.h5')
    cascade = get_path('haarcascade_frontalface_default.xml')
    emotion_analyzer = EmotionAnalyzer(model_json, model_h5, cascade)
    return jsonify({'emotions': emotion_analyzer.get_emotions()})
    # return jsonify({'tasks': ''})

def get_path(file_name):
    return os.path.dirname(__file__) + '/../' + file_name

