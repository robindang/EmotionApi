"""
Usage:   weppy --app=EmotionApi <command>
Example: weppy --app=EmotionApi shell
"""
from EmotionApi import app


@app.cli.command('routes')
def print_routing():
    print(app.route.routes_out)


@app.cli.command('get_users')
def print_users():
    from EmotionApi import db
    from EmotionApi.models.user import User
    rows = db(User.email).select()
    for row in rows:
        print(row)
