(function () {
    'use strict';
    App.factory('AuthInterceptorService', ['$q', '$location', 'localStorageService', function ($q, $location, localStorageService) {

        var AuthInterceptorServiceFactory = {};

        var _request = function (config) {

            config.headers = config.headers || {};

            var authData = localStorageService.get('authorizationData');
            if (authData) {
                config.headers.Authorization = 'Bearer ' + authData.token;
            }

            return config;
        };

        var _responseError = function (rejection) {
            if (rejection.status === 401) {
                $location.path('/login');
            }
            return $q.reject(rejection);
        };

        AuthInterceptorServiceFactory.request = _request;
        AuthInterceptorServiceFactory.responseError = _responseError;

        return AuthInterceptorServiceFactory;
    }]);
}());