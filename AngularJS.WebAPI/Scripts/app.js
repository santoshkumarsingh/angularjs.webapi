var app = angular.module("app", ['ngRoute']);

app.config(function ($routeProvider) {

    $routeProvider.when('/login', {
        templateUrl: '/Templates/login.html',
        controller: 'LoginController'
    });

    $routeProvider.when('/home', {
        templateUrl: '/Templates/home.html',
        controller: 'HomeController'
    });

    $routeProvider.otherwise({
        redirectTo: '/login'
    });

});

app.run(function ($rootScope, $location, AuthenticationService) {

    var sAuth = ['/home'];
    $rootScope.$on('$routeChangeStart', function (event, next, current) {
        if (_(sAuth).contains($location.path()) && !AuthenticationService.isLoggedIn()) {
            $location.path('/login');
        }
    });
});

//app.factory("AuthenticationService", function ($location) {
//    return {
//        login: function (credentials) {
//            if (credentials.username !== "test" || credentials.password !== "test") {
//                alert("Username must be 'ralph', password must be 'wiggum'");
//            } else {
//                $location.path('/home');
//            }
//        },
//        logout: function () {
//            $location.path('/login');
//        }
//    };
//});
app.factory("SessionService", function () {
    return {
        get: function (key) {
            return sessionStorage.getItem(key);

        },
        set: function (key, val) {
            return sessionStorage.setItem(key, val);
        },
        unset: function (key) {
            return sessionStorage.removeItem(key);
        }

    };
});
app.factory("AuthenticationService", function ($location, $http, SessionService) {
    var cacheSesion = function () {
        SessionService.set('auth', true);
    };
    var uncacheSession = function () {
        SessionService.unset('auth');

    };
    return {
        login: function (credentials) {
            var login = $http.post("/auth/Login", credentials);
            login.success(cacheSesion);
            return login;
        },
        logout: function () {
            // $location.path('/login');
            var logout = $http.get('/auth/LogOut');
            logout.success(uncacheSession);
            return logout;
        },
        isLoggedIn: function () {
            return SessionService.get('auth');
        }
    };
});

app.controller("LoginController", function ($scope, $location, AuthenticationService) {
    $scope.credentials = {
        username: "",
        password: ""
    };

    $scope.login = function () {
        AuthenticationService.login($scope.credentials).success(function () {
            $location.path("/home");
        });
    };
});


app.controller("HomeController", function ($scope, $location, AuthenticationService) {
    $scope.title = "Awesome Home";
    $scope.message = "Mouse Over these images to see a directive at work!";

    $scope.logout = function () {
        AuthenticationService.logout().success(function () {
            $location.path('/login');


        });
    };
});


app.directive("showsMessageWhenHovered", function () {
    return {
        restrict: "A", // A = Attribute, C = CSS Class, E = HTML Element, M = HTML Comment
        link: function (scope, element, attributes) {
            var originalMessage = scope.message;
            element.bind("mouseenter", function () {
                scope.message = attributes.message;
                scope.$apply();
            });
            element.bind("mouseleave", function () {
                scope.message = originalMessage;
                scope.$apply();
            });
        }
    };
});