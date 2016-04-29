angular.module('MainCtrl', ['ui.bootstrap', 'countTo', 'ui.bootstrap.validation'])
.controller('MainController', function($scope, $location, $http, $timeout, $rootScope, $modal, Validator, CSVConverter) {
    $scope.dateNow = new Date();

    $scope.validation = [];
    $scope.oldInputArray = [];
    $scope.oldInput = "";
    $scope.toValidate = []; // stores the current values to validate with api
    $scope.toValidateIndex = []; // stores the index of the values to be validated with api
    $scope.maxValidations = 50;
    $scope.totalValidations = 10000; // the total validations made
    $scope.totalValidationString = "10,000"; // the output string for total validations
    $scope.isCsv = false; // track whether current request is using a csv
    $scope.model = {};
    $scope.isIOS = ( navigator.userAgent.match(/iPad|iPhone|iPod/g) ? true : false );

    $scope.max = 1000000;

    var amt = 500000;
  
    $scope.countTo = amt;
    $scope.countFrom = 0;
    $scope.showTaxRelief = false;

    $scope.taxReliefOptions = "noTaxRelief";
    $scope.idType = "1";
  
    $timeout(function(){
        $scope.dynamic = amt;
    }, 200);

    $scope.changeTaxRelief = function(value) {
         if (value == "noTaxRelief") {
            $scope.showTaxRelief = false;
        }
        else {
            $scope.showTaxRelief = true;
        }
    };

    $scope.parseIdType = function(value) {
        switch (value) {
            case "1":
                return "NRIC";
            case "2":
                return "FIN (Foreign Identification Number)";
            case "5":
                return "UEN-Business (Business Registration Number)";
            case "6":
                return "UEN-Local Company (Local company registartion Number)";
            case "8":
                return "ASGD (Tax Reference Number issued by IRAS)";
            case "10":
                return "ITR (Income Tax reference Number issued by IRAS)";
            case "35":
                return "UEN-Others (Unique Entity Number with TyyPQnnnnX format)";
            default:
                return "NRIC";
        }
    };

    $scope.submitDonate = function() {
        var postObj = {
            "IdType": parseInt($scope.idType),
            "IdNumber": $scope.model.idNumber,
            "FirstName": $scope.model.firstName,
            "LastName": $scope.model.lastName,
            "Email": $scope.model.email,
            "Phone": $scope.model.phoneNumber,
            "AddressLine1": $scope.model.add1,
            "AddressLine2": $scope.model.add2,
            "AddressLine3": $scope.model.add3,
            "PostalCode": $scope.model.postalCode,
            "DonationAmount": $scope.model.donationAmt
        };

        /*$http.post('http://172.22.117.244/api/donation', postObj)
        .success(function(res) {
            console.log(res);
        })
        .error(function(res) {
            console.log(res);
        });*/
        /*debugger;
        $.param(postObj);*/

        $http({
            url: "http://172.22.117.244/api/donation",
            method: "POST",
            data: postObj,
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'
            }
        })
        .success(function(res) {
            console.log(res);
        })
        .error(function(res) {
            console.log("error");
            console.log(res);
        });
    };

    /*$scope.validateUserInput = function(theList) {
        // compare and check for changes to input
        changedIndex = Validator.compare($scope.oldInputArray, theList);

        $scope.toValidate = []; 
        $scope.toValidateIndex = [];

        for (var i = 0; i < changedIndex.length; i++) {

            // check length of the input and update validation message
            if (theList[changedIndex[i]].length == 0) {
                $scope.validation[changedIndex[i]] = {
                    "isValid": false,
                    "message": "NRIC is empty"
                }
            }
            else if (theList[changedIndex[i]].length != 9) {
                $scope.validation[changedIndex[i]] = {
                    "isValid": false,
                    "message": "Incorrect NRIC length"
                }
            }
            else {
                // only validate string with correct length
                $scope.toValidate.push(theList[changedIndex[i]]);
                $scope.toValidateIndex.push(changedIndex[i]);
            }
        }

        // remove validation messages for removed input
        if ($scope.oldInputArray.length > theList.length) {
            $scope.validation.splice(theList.length, ($scope.oldInputArray.length - theList.length));
        }
        
        $scope.oldInputArray = theList;

        // there is something to validate
        if ($scope.toValidate.length > 0) {
            $scope.validateWithServer();
        }
    };

    $scope.validateWithServer = function(response) {
        // make sure challenge and response fields are not undefined
        //sendChallenge = (typeof(challenge) !== 'undefined' ? challenge : "");
        //sendResponse = (typeof(response) !== 'undefined' ? response : "");

        // send data and captcha details (if any) to server for verification
        Validator.validate($scope.toValidate, $scope.isCsv, response)
        .success(function(res) {
            // data successfully verified, update results
            if (res.isValid) {
                for (var i = 0; i < res.result.length; i++) {
                    $scope.validation[$scope.toValidateIndex[i]] = res.result[i];
                }
                $scope.isCsv = false; // reset the isCsv flag to false for the next set of verification
            }

            // verification failed, check reason
            else {
                // captcha was required and was empty
                if (res.requireCaptcha && res.captchaEmpty) {
                    $scope.triggerModal();
                }
                // captcha was required and was wrong
                else if (res.requireCaptcha && !res.captcha) {
                    $scope.triggerModal(true);
                }
                // TODO: capture other errors like over max allowed length
            }
        })
        .error(function(res) {
            console.log(res);
        });
    };*/

    // handles the modal captcha response
    $scope.handleModalResponse = function(response) {
        // Captcha validation wasn't done
        if (/*typeof(challenge) === 'undefined' || */typeof(response) === 'undefined') {
            for (var i = 0; i < $scope.toValidateIndex.length; i++) {
                $scope.validation[$scope.toValidateIndex[i]] = {
                    'isValid': false,
                    'message': 'Please complete Captcha validation.'
                }
            }
        }
        else {
            // send to server for verification
            $scope.validateWithServer(response);
        }
    };

    $scope.triggerModal = function(failed) {
        // set a message to the modal if captcha had failed for the first / subsequent time
        var message = "";
        if (typeof(failed) !== 'undefined' && failed) {
            message = "Captcha verification failed, please try again.";
        }

        var modalInstance = $modal.open({
            animation: true,
            templateUrl: 'views/modalContent.html',
            controller: 'ModalInstanceCtrl',
            backdrop: 'static', // make the backdrop unclickable to dismiss
            resolve: {
                message: function() {
                    return message;
                }
            }
        });

        modalInstance.result.then(function(output) {
            // modal "ok" pressed
            $scope.handleModalResponse(output.response);
        }, function () {
            // modal dismissed
            $scope.handleModalResponse();
        });
    };
    


});