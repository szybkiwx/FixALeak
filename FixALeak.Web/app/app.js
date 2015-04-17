var App = angular.module('FixALeak', [
  'ngRoute', 'LocalStorageModule', 'ngResource'
]);

App.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
        when('/home', {
            templateUrl: '/app/views/home.html',
            controller: 'HomeController'
        })
        .when("/login", {
            controller: "LoginController",
            templateUrl: "/app/views/login.html"
        })
        .when("/signup", {
          controller: "SignupController",
          templateUrl: "/app/views/signup.html"
        })
        .when('/months', {
            templateUrl: 'app/views/month/list.html',
            controller: 'MonthListCtrl'
        }).
        otherwise({
            redirectTo: '/home'
        });
  }]);

App.run(['AuthService', function (AuthService) {
    AuthService.fillAuthData();
}]);


App.config(function ($httpProvider) {
    $httpProvider.interceptors.push('AuthInterceptorService');
});

App.value("apiUrl", 'http://localhost:25366');