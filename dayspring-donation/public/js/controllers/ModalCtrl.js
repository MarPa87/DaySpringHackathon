angular.module('ModalCtrl', ['ui.bootstrap']).controller('ModalInstanceCtrl', function ($scope, $modalInstance, message, reCAPTCHA, vcRecaptchaService) {
    //reCAPTCHA.setPublicKey('6Lf3jgkTAAAAADONuJf3wojb8cPJEAlDh76HVBli');
    //reCAPTCHA.create('responsive_recaptcha');

    $scope.message = message;
    $scope.widgetId = null;
    $scope.response = "";

    // reload recaptcha on open
    if ($scope.widgetId != null) {
    	vcRecaptchaService.reload($scope.widgetId);
    }

    $scope.setWidgetId = function (widgetId) {
        $scope.widgetId = widgetId;
    };

    $scope.setResponse = function (response) {
        $scope.response = response;
    };

    $scope.handleResponse = function (response) {
        $scope.setResponse(response);
        $scope.ok();
    };

    $scope.ok = function () {
        /*$modalInstance.close({"challenge": reCAPTCHA.challenge(), "response": reCAPTCHA.response()});*/
        $modalInstance.close({"response": $scope.response});
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
});