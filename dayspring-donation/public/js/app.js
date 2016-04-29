// public/js/app.js
var myApp = angular.module('sampleApp', ['ngRoute', 'ngSanitize', 'appRoutes', 'MainCtrl', 'ModalCtrl', 'ValidatorService', 'CSVConverterService', 'ngTagsInput', 'ngFileUpload', 'monospaced.elastic', 'ngCsv', 'reCAPTCHA', 'vcRecaptcha']);
/*.config(function (reCAPTCHAProvider) {
    // optional: gets passed into the Recaptcha.create call
    reCAPTCHAProvider.setOptions({
        theme: 'custom',
        custom_theme_widget: 'responsive_recaptcha'
    });
});*/

myApp.run(function($templateCache) {
  $templateCache.put('template/tabs/tab.html', '<div ng-class="[{active: active, disabled: disabled}, classes]" class="tab nav-item"><a href ng-click="select($event)" class="nav-link" tab-heading-transclude>{{heading}}</a></div>');
});