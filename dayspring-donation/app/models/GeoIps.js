// app/models/GeoIps.js
// grab the mongoose module
var mongoose = require('mongoose');
var Schema = mongoose.Schema;
var createdModifiedPlugin = require('mongoose-createdmodified').createdModifiedPlugin;

// define our GeoIp model
// module.exports allows us to pass this to other files when it is called
var GeoIpSchema   = new Schema({
    ip: {
        type: String,
        unique: true,
        default: ''
    },
    country_code: {
        type: String,
        default: ''
    },
    country_name: {
        type: String,
        default: ''
    },
    region_code: {
        type: String,
        default: ''
    },
    region_name: {
        type: String,
        default: ''
    },
    city: {
        type: String,
        default: ''
    },
    zip_code: {
        type: String,
        default: ''
    },
    time_zone: {
        type: String,
        default: ''
    },
    latitude: {
        type: Number,
        default: 0
    },
    longitude: {
        type: Number,
        default: 0
    },
    metro_code: {
        type: Number,
        default: 0
    }
});

GeoIpSchema.plugin(createdModifiedPlugin, {index: true});

module.exports = mongoose.model('GeoIp', GeoIpSchema);