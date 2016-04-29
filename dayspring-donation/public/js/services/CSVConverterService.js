// public/js/services/CSVConverterService.js
angular.module('CSVConverterService', []).factory('CSVConverter', [function() {

    return {
        convertToObject: function(csvString) {
            try {
                csvArray = CSV.parse(csvString);
                return csvArray;
            }
            catch (error) {
                alert("Oops! We were unable to read your file. The error was:\n\n" + error);
                return false;
            }
        }
    }       

}]);