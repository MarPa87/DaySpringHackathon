// grab the models
var GeoIp = require('../models/GeoIps');

var https     = require('https');
var http      = require('http');
var constants = require('../../config/constants');

function validateNRIC(theNric){
    // initialise the reference variables
    var weight = [2, 7, 6, 5, 4, 3, 2];
    var validPrefixes = ["S", "T", "F", "G"];
    var offset = {
    	"S": 0,
    	"T": 4,
    	"F": 0,
    	"G": 4
    };
    var checkDigitSetNumber = {
    	"S": "alpha",
    	"T": "alpha",
    	"F": "beta",
    	"G": "beta"
    };
    var checkDigits = {
    	"alpha": ['J', 'Z', 'I', 'H', 'G', 'F', 'E', 'D', 'C', 'B', 'A'],
    	"beta": ['X', 'W', 'U', 'T', 'R', 'Q', 'P', 'N', 'M', 'L', 'K']
    };

    // return message
    result = {
    	"isValid": true,
    	"message": ""
    };

    // check for empty input
    if (!theNric || theNric == '') {
    	result.isValid = false;
    	result.message = "NRIC is empty";
        return result;
    }

    // check NRIC overall length
    if (theNric.length != 9) {
        result.isValid = false;
    	result.message = "Incorrect NRIC length";
        return result;
    }

    // extract the first, last and middle sections
    var first = theNric.charAt(0);
    var last = theNric.charAt(theNric.length - 1);
    var numericNric = theNric.substr(1, theNric.length - 2);

    // check for valid prefix
    if (validPrefixes.indexOf(first.toUpperCase()) < 0) {
        result.isValid = false;
    	result.message = "Invalid NRIC prefix";
        return result;
    }

    // check for valid numeric digits
    if (isNaN(numericNric)) {
        result.isValid = false;
    	result.message = "Invalid number of numeric digits";
        return result;
    }

    // get dot product of numeric NRIC and weight
    var count = 0;
    var dotProduct = 0;
    if (numericNric != null) {
        while (numericNric != 0) {
            dotProduct += (numericNric % 10) * weight[weight.length - (1 + count++)];
            numericNric /= 10;
            numericNric = Math.floor(numericNric);
        }
    }

    // get the correct checksum alphabet from the reference list
    var output =  (offset[first.toUpperCase()] + dotProduct) % 11;
    var validCheckDigit = checkDigits[checkDigitSetNumber[first.toUpperCase()]][output];

    // check if last alphabet of NRIC input is valid
    if (last.toUpperCase() != validCheckDigit) {
        result.isValid = false;
    	result.message = "Invalid NRIC. Last character should be '" + validCheckDigit + "'.";
        return result;
    }

    return result;
}
function validateBulkNRIC(data) {
    returnMsg = new Object();

    // check if input data is an array
    if (Object.prototype.toString.call( data ) !== '[object Array]') {
        returnMsg.isValid = false;
        returnMsg.message = "Input object not an array";
    }

    else {
        returnMsg.result = [];

        // test if data exceeds the max allowed validations
        if (data.length > constants.max_validations) {
            returnMsg.isValid = false;
            returnMsg.message = "Maximum 50 validations allowed";
        }

        else {
            returnMsg.isValid = true;
            for (var i = 0; i < data.length; i++) {
                returnMsg.result.push(validateNRIC(data[i]));
            } 
        }
    }

    return returnMsg;
}
function validateRecaptcha(userResponse, callback) {
    https.get("https://www.google.com/recaptcha/api/siteverify?secret=" + constants.recaptcha_secret_key + "&response=" + userResponse, function(res) {
        var data = "";
        res.on('data', function (chunk) {
            data += chunk.toString();
        });
        res.on('end', function() {
            try {
                var parsedData = JSON.parse(data);
                callback(parsedData.success);
            } catch (e) {
                callback(false);
            }
        });
    });
}
function validateGeoIp(ip, callback) {
    var thisIp  = ip;
    var thisCallback = callback;
    GeoIp.findOne({ip : thisIp}, function (err, docs) {
        if (docs) {
            thisCallback(docs._id);
        }
        else {
            http.get("http://freegeoip.net/json/" + thisIp, function(res) {
                var data = "";
                res.on('data', function (chunk) {
                    data += chunk.toString();
                });
                res.on('end', function() {
                    try {
                        var parsedData = JSON.parse(data);

                        var geoIp = new GeoIp();
                        geoIp.ip = thisIp;
                        geoIp.country_code = parsedData.country_code;
                        geoIp.country_name = parsedData.country_name;
                        geoIp.region_code = parsedData.region_code;
                        geoIp.region_name = parsedData.region_name;
                        geoIp.city = parsedData.city;
                        geoIp.zip_code = parsedData.zip_code;
                        geoIp.time_zone = parsedData.time_zone;
                        geoIp.latitude = parsedData.latitude;
                        geoIp.longitude = parsedData.longitude;
                        geoIp.metro_code = parsedData.metro_code;

                        geoIp.save(function(err, newGeoIp) {
                            if (!err) {
                                thisCallback(newGeoIp._id);
                            }
                            else {
                                thisCallback(false);
                            }
                        });
                    } catch (e) {
                        thisCallback(false);
                    }
                });
                
            });
        }
    });
}
exports.validateNRIC = validateNRIC;
exports.validateBulkNRIC = validateBulkNRIC;
exports.validateRecaptcha = validateRecaptcha;
exports.validateGeoIp = validateGeoIp;