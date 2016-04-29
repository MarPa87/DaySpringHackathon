// public/js/appRoutes.js
    angular.module('appRoutes', []).config(['$routeProvider', '$locationProvider', function($routeProvider, $locationProvider) {

    $routeProvider

        // home page
        .when('/', {
            templateUrl: 'views/home.html',
            controller: 'MainController'
        })

        // term of use
        .when('/terms', {
            templateUrl: 'views/terms.html'
        });
        
    $locationProvider.html5Mode(true);

}]);