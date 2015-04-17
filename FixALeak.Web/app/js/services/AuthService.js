(function () {
    'use strict';
    App.factory('AuthService', ['localStorageService', 'AccountResource', 'TokenService', function (localStorageService, AccountResource, TokenService) {
        var authentication = {
            isAuth: false,
            userName: ""
        };

        var logOut = function () {
            localStorageService.remove('authorizationData');
            authentication.isAuth = false;
            authentication.userName = "";
        };

        return {
            saveRegistration: function (registration) {
                logOut();
                return AccountResource
                    .save({}, registration)
                    .$promise
                    .then(function (response) {
                        return response;
                    });
            },
            login: function (loginData) {
                var data = {
                    grant_type: "password",
                    username: loginData.userName,
                    password: loginData.password
                };

                TokenService.save({}, data)
                    .$promise
                    .then(function (response) {
                        localStorageService.set('authorizationData',
                            {
                                token: response.access_token,
                                userName: loginData.userName
                            });
                        authentication.isAuth = true;
                        authentication.userName = loginData.userName;
                    })
                    .catch(logOut);
            },
            logOut: logOut,
            fillAuthData: function () {
                var authData = localStorageService.get('authorizationData');
                if (authData) {
                    authentication.isAuth = true;
                    authentication.userName = authData.userName;
                }
            },
            authentication: authentication,
            getAuthentication: function () {
                return authentication;
            }
        };
    }]);
}());