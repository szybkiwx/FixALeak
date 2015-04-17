(function () {
    App.factory('TokenService', ['$resource', 'apiUrl', function ($resource, apiUrl) {
        return $resource(apiUrl + '/token');
    }]);
}());