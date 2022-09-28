from flask import Flask, jsonify, request, Response, Blueprint
from flask_restplus import Api, Resource, fields
import datetime
import json
from get_ownership_values import get_ownership
import os

app = Flask(__name__)
api = Api(app, version='1.0', title='TRUE Analytics API',
    description='TRUE Analytics API',
  )

# {
# 	"algorithmId": "1",
# 	"movementType": "DESPACHO A LINEA",
# 	"sourceNode": "ESTACION CHICAGUA",
# 	"sourceNodeType": "Limite",
# 	"destinationNode": "AYACUCHO - COVEÑAS 16\"",
# 	"destinationNodeType": "Oleoducto",
# 	"sourceProduct": "MEZCLA MAGDALENA BLEND",
# 	"sourceProductType": "CRUDO",
# 	"startDate": "2019-06-15",
# 	"endDate": "2019-06-18"
# }
model = api.model('TrueAnalytics',{
		'algorithmId': fields.String(required=True, description='algorithmId'),
		'movementType': fields.String(required=True, description='movementType'),
		'sourceNode': fields.String(required=True, description='sourceNode'),
		'sourceNodeType': fields.String(required=True, description='sourceNodeType'),
		'destinationNode': fields.String(required=True, description='destinationNode'),
		'destinationNodeType': fields.String(required=True, description='destinationNodeType'),
		'sourceProduct': fields.String(required=True, description='sourceProduct'),
		'sourceProductType': fields.String(required=True, description='sourceProductType'),
		'startDate': fields.String(required=True, description='startDate'),
		'endDate': fields.String(required=True, description='endDate')
		})

@api.route('/ownership')
class Analytics(Resource):

  @api.doc('ownership')
  @api.expect(model)
  def post(self):
    try:
      request_data = request.get_json()
      result = get_ownership(request_data['algorithmId'], request_data['movementType'], request_data['sourceNode'],
                            request_data['sourceNodeType'], request_data['destinationNode'], request_data['destinationNodeType'], 
                            request_data['sourceProduct'], request_data['sourceProductType'], request_data['startDate'], request_data['endDate'])
      dict_ = result.to_dict(orient='records')
      return Response(json.dumps(dict_, default=convert_timestamp), mimetype='application/json')
    except Exception as error:
      status_code = 500
      success = False
      response = {
            'success': success,
            'error': {
                'type': 'UnexpectedException',
                'message': 'An unexpected error has occurred.',
                'args': error.args
            }
        }
      return response, status_code

def convert_timestamp(item_date_object):
    if isinstance(item_date_object, (datetime.date, datetime.datetime)):
        return item_date_object.strftime("%Y-%m-%d")

# http://containertutorials.com/docker-compose/flask-simple-app.html
if __name__ == '__main__':
      app.run(debug=True,host='0.0.0.0')

			