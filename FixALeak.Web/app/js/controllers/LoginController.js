(function () {
    'use strict';
    App.controller('LoginController', ['$scope', '$location', 'AuthService', function ($scope, $location, AuthService) {

        $scope.loginData = {
            userName: "",
            password: ""
        };

        $scope.message = "";

        $scope.login = function () {
            AuthService.login($scope.loginData)
                .then(function (response) {
                        $location.path('/orders');
                })
                .catch(function (err) {
                        $scope.message = err.error_description;
                });
        };
    }]);
}());