(function () {
    'use strict';
    App.controller('IndexController', ['$scope', '$location', 'AuthService', function ($scope, $location, AuthService) {

        $scope.logOut = function () {
            AuthService.logOut();
            $location.path('/home');
        };

        $scope.authentication = AuthService.getAuthentication();

    }]);
}());