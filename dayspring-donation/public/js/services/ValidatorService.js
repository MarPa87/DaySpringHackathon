// public/js/services/ValidatorService.js
angular.module('ValidatorService', []).factory('Validator', ['$http', function($http) {

    return {
        // sends the validation request
        validate: function(nricList, isCsv, response) {
            var postObj = {
                "nric": nricList
            };
            if (typeof(response) !== 'undefined') {
                postObj.response = response;
            }
            if (typeof(isCsv) !== 'undefined') {
                postObj.isCsv = isCsv;
            }
            return $http.post('/api/validate', postObj);
        },
        // compares new input and current input to identify changes
        compare: function(oldInput, newInput) {
            var changedIndex = [];
                
            // values were removed
            // ignore the removed values because validation is done on the new input array
            if (oldInput.length > newInput.length) {
                for (var i = 0; i < newInput.length; i++) {
                    if (oldInput[i] != newInput[i]) {
                        changedIndex.push(i);
                    }
                }
            }

            // number of values remain the same or were added
            else {
                for (var i = 0; i < oldInput.length; i++) {
                    if (oldInput[i] != newInput[i]) {
                        changedIndex.push(i);
                    }
                }
                // add the new values index
                for (var j = oldInput.length; j < newInput.length; j++) {
                    changedIndex.push(j);
                }
            }
            
            return changedIndex; 
        },
        // get the current session's usage count to determine if captcha validation is required
        getSessionUsageCount: function() {
            return $http.get('/api/sessionUsageCount');
        },
        getTotalValidations: function() {
            return $http.get('/api/total_validations');
        }
    }       

}]);