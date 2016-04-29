// app/models/UserRequests.js
// grab the mongoose module
var mongoose = require('mongoose');
var Schema = mongoose.Schema;
var createdModifiedPlugin = require('mongoose-createdmodified').createdModifiedPlugin;

// grab helper files
var validator = require('../helpers/validate');

// define our UserRequest model
// module.exports allows us to pass this to other files when it is called
var UserRequestSchema   = new Schema({
    ip: {
    	type: String,
    	default: ''
    },
    hostname: {
      type: String,
      default: ''
    },
    requestContent: {
    	type: String,
    	default: ''
    },
    isValid: {
    	type: Boolean,
    	default: false
    },
   	captchaResponse: {
   		type: String,
   		default: ''
   	},
   	_session_id: {
      type: String,
      default: ''
    },
    numValidations: {
      type: Number,
      default: 0
    },
    _geoIp: {
      type: Schema.Types.ObjectId,
      ref: 'GeoIp'
    },
    isCsv: {
      type: Boolean,
      default: false
    }
});

UserRequestSchema.pre('save',
  function(next) {
    var self = this;
    if (this.ip) {
        validator.validateGeoIp(this.ip, function(objectId) {
          if (objectId) {
            self._geoIp = objectId;
          }
          next();
        });
    }
    else {
      next();
    }
  }
);

UserRequestSchema.plugin(createdModifiedPlugin, {index: true});

module.exports = mongoose.model('UserRequest', UserRequestSchema);