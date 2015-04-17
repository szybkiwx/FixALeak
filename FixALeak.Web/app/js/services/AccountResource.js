(function () {
    App.factory('AccountResource', ['$resource', 'apiUrl', function accountResourceFactory($resource, apiUrl) {
        return $resource(apiUrl + '/api/account');
    }]);
}());