// server.js

// modules =================================================
var express        = require('express');
var app            = express();
var bodyParser     = require('body-parser');
var methodOverride = require('method-override');
//var mongoose       = require('mongoose');
var cookieParser   = require('cookie-parser');
var session 	   = require('express-session');
//var MongoStore     = require('connect-mongo')(session);

// configuration ===========================================
    
// config files
var db = require('./config/db');
var constants = require('./config/constants');

// set our port
var port = process.env.PORT || 3000; 

// connect to our mongoDB database 
// (uncomment after you enter in your own credentials in config/db.js)
//mongoose.connect(db.url); 

// get all data/stuff of the body (POST) parameters
// parse application/json 
app.use(bodyParser.json()); 

// parse application/vnd.api+json as json
app.use(bodyParser.json({ type: 'application/vnd.api+json' })); 

// parse application/x-www-form-urlencoded
app.use(bodyParser.urlencoded({ extended: true })); 

// override with the X-HTTP-Method-Override header in the request. simulate DELETE/PUT
app.use(methodOverride('X-HTTP-Method-Override')); 

// set the static files location /public/img will be /img for users
app.use(express.static(__dirname + '/public'));

// session-related setup
app.use(cookieParser());
/*app.use(session({
	secret: constants.session_secret,
	store: new MongoStore({ mongooseConnection: mongoose.connection }),
	saveUninitialized: true, // create session even if nothing is stored
    resave: true, // save session if unmodified
    cookie: {
    	maxAge: constants.session_maxAge
    }
}));*/

// routes ==================================================
require('./app/routes')(app); // configure our routes

app.enable('trust proxy'); // trust behind proxy

// start app ===============================================
// startup our app at http://localhost:8080
app.listen(port);               

// shoutout to the user                     
console.log('Magic happens on port ' + port);

// expose app           
exports = module.exports = app;                         
