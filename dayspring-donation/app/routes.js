 // app/routes.js

// grab the models
var UserRequest = require('./models/UserRequests');
var GeoIp = require('./models/GeoIps');

// grab helper files
var validator = require('./helpers/validate');
var Recaptcha = require('recaptcha').Recaptcha;
var https     = require('https');
var dns       = require('dns');
// config files
var constants = require('../config/constants');

    module.exports = function(app) {

        // server routes ===========================================================
        // handle things like api calls
        // authentication routes

        // sample api route
        /*app.get('/api/listings', function(req, res) {
            // use mongoose to get all listings in the database
            Listing.find(function(err, listings) {

                // if there is an error retrieving, send the error. 
                                // nothing after res.send(err) will execute
                if (err)
                    res.send(err);

                res.json(listings); // return all listings in JSON format
            });
        });

        // route to handle creating goes here (app.post)
        app.post('/api/listings', function(req, res) {
            var listing = new Listing(); // create new instance of Listing model
            listing.name = req.body.name;
            listing.description = req.body.description;
            listing.quality = req.body.quality;
            listing.url = req.body.url;
            listing.tags = [];

            // use mongoose to post listing in the database
            listing.save(function(err) {

                // if there is an error posting, send the error. 
                                // nothing after res.send(err) will execute
                if (err)
                    res.send(err);

                res.json({message: 'Listing created!'});
            });
        });*/

        app.post('/api/validate', function(req, res) {
            var toValidate = req.body.nric;
            var returnMsg = new Object();
            var sessionUsageCount = req.session.usageCount ? parseInt(req.session.usageCount) : 0;
            
            var userRequest = new UserRequest(); // create new instance of UserRequest model
            userRequest.ip = req.ip;
            userRequest.requestContent = JSON.stringify(req.body.nric);
            userRequest.numValidations = req.body.nric.length;
            userRequest._session_id = req.sessionID;
            userRequest.isCsv = req.body.isCsv ? req.body.isCsv : false;
            userRequest.save();

            // get hostname using DNS lookup
            dns.reverse(req.ip, function(err, hostnames) {
                if (!err) {
                    if (hostnames.length > 0) {
                        userRequest.hostname = hostnames[0];
                        userRequest.save();
                    }
                }
            });

            // determine if captcha verification is needed
            var requireCaptcha = false;
            // 1. check if input data length is more than max requests required before captcha
            if (Object.prototype.toString.call( toValidate ) === '[object Array]' && toValidate.length > constants.max_captcha_requests) {
                requireCaptcha = true;
            }
            // 2. check if session usage count exceeds the max requests required before captcha
            else if (sessionUsageCount % constants.max_captcha_requests == 0) {
                requireCaptcha = true;
            }

            returnMsg.requireCaptcha = requireCaptcha;

            if (requireCaptcha) {
                // if challenge and response is empty, immediately send respond
                if (!req.body.hasOwnProperty('response')) {
                    // return captcha false to the client
                    returnMsg.captcha = false;
                    returnMsg.isValid = false;
                    returnMsg.captchaEmpty = true;

                    // response
                    res.json(returnMsg);
                }

                else {
                    // create new Recaptcha object to verify
                    /*var data = {
                        remoteip:  req.connection.remoteAddress,
                        challenge: req.body.challenge,
                        response:  req.body.response
                    };*/
                    //var recaptcha = new Recaptcha('6Lf3jgkTAAAAADONuJf3wojb8cPJEAlDh76HVBli', '6Lf3jgkTAAAAAHeG6ppVzOlDYityoL7cz2E7l9xn', data);
                    // var recaptcha = new Recaptcha('6LeIxAcTAAAAAJcZVRqyHh71UMIEGNQ_MXjiZKhI', '6LeIxAcTAAAAAGG-vFI1TnRWxMZNFuojJ4WifJWe', data); // test key

                    // log the captcha response
                    userRequest.captchaResponse = req.body.response;
                    userRequest.save();

                    //recaptcha.verify(function(success, error_code) {
                    validator.validateRecaptcha(req.body.response, function(success) {
                        if (success) {
                            // continue to validate
                            returnMsg = validator.validateBulkNRIC(toValidate);
                            returnMsg.captcha = true;

                            if (returnMsg.isValid) {
                                // log the request as valid
                                userRequest.isValid = true;
                                userRequest.save();

                                // increment the session usage count
                                req.session.usageCount = sessionUsageCount + toValidate.length;                
                            }

                            // response
                            res.json(returnMsg);
                        }
                        else {
                            // return captcha false to the client
                            returnMsg.captcha = false;
                            returnMsg.isValid = false;
                            returnMsg.captchaEmpty = false;

                            // response
                            res.json(returnMsg);
                        }
                    });
                }
            }

            // no captcha required, just validate
            else {
                // continue to validate
                returnMsg = validator.validateBulkNRIC(toValidate);

                if (returnMsg.isValid) {
                    // log the request as valid
                    userRequest.isValid = true;
                    userRequest.save();

                    // increment the session usage count
                    req.session.usageCount = sessionUsageCount + toValidate.length;                
                }

                // response
                res.json(returnMsg);
            }
            
        });

        app.post('/api/captcha', function(req, res) {

            var data = {
                remoteip:  req.connection.remoteAddress,
                challenge: req.body.challenge,
                response:  req.body.response
            };
            var recaptcha = new Recaptcha('6Lex7AkTAAAAACFAMWrzjDquN-RS5S5A0NDWGI1i', '6Lex7AkTAAAAALE3jWhJ7LTcB21hK5cWZ2DH_h9r', data);

            recaptcha.verify(function(success, error_code) {
                if (success) {
                    res.send('Recaptcha response valid.');
                }
                else {
                    res.send('Recaptcha response invalid.');
                }
            });

        });

        app.get('/api/sessionUsageCount', function(req, res) {
            var usageCount = (req.session.usageCount ? req.session.usageCount : '0');
            res.send(usageCount);
        });

        app.get('/api/total_validations', function(req, res, next) {
            var result = UserRequest.aggregate([
                { $group: {
                    _id: null,
                    totalValidations: { $sum: "$numValidations"  }
                }}
            ], function (err, result) {
                if (err) {
                    res.send(err);
                }
                // bump up number by 10000
                result[0].totalValidations += 10000;
                res.send(result);
            });
        });


        // route to handle delete goes here (app.delete)

        // frontend routes =========================================================
        // route to handle all angular requests
        app.get('*', function(req, res) {
            res.sendfile('./public/index.html'); // load our public/index.html file
        });

    };