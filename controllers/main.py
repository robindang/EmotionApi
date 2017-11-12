from weppy import response

from EmotionApi import app, auth


@app.route("/")
def welcome():
    response.meta.title = "Emotion Api"
    return dict()


@app.route('/account(/<str:f>)?(/<str:k>)?')
def account(f, k):
    response.meta.title = "Emotion Api | Account"
    form = auth(f, k)
    return dict(req=f, form=form)
