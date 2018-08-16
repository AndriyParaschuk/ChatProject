(function () {
    var app = angular.module('app', ['ui.bootstrap', 'dialogs', 'luegg.directives'])
        .controller('AccountCtrl', ['$scope', '$http', '$dialogs', '$rootScope', '$interval', '$filter', '$window', function ($scope, $http, $dialogs, $rootScope, $interval, $filter, $window) {
            $scope.selectedUser = {};
            $scope.user = {};
            $scope.token = {};
            $scope.userFriends = {};
            $scope.userRequests = {};
            $scope.requestsToUser = {};
            $scope.findedUsers = {};
            $scope.chatMessages = {};
            $scope.searchParam;
            $scope.message;
            $scope.canSend = true;

            $scope.chatHub = $.connection.chatHub;

            $scope.getUser = function () {
                $http.get('/Account/GetUser').then(
                    function (successResponse) {
                        $scope.user = successResponse.data.user;
                        $rootScope.user = successResponse.data.user;
                        $scope.checkIfItFirstLogin();
                        $scope.getUsersFriends();
                        $scope.getUserRequests();
                        $scope.GetRequestsToUser();
                        $scope.UserEnter();
                        $.connection.hub.start().then(
                            function (successResponse) {
                                $scope.chatHub.server.enter($scope.user);
                            });
                    },
                    function (errorResponse) {
                        // handle errors here
                    }
                );
            };

            $scope.getToken = function () {
                $http.get('/Account/Token').then(
                    function (successResponse) {
                        $scope.token = successResponse.data.token;
                    },
                    function (errorResponse) {
                        // handle errors here
                    });
            };

            $scope.UpdateUserProfile = function () {
                var post = $http({
                    method: "POST",
                    url: "/Account/UpdateUserProfile",
                    dataType: 'json',
                    data: { user: $scope.user },
                    headers: { "Content-Type": "application/json" }
                });
            }

            $scope.launch = function (which) {
                var dlg = null;
                switch (which) {
                    case 'create':
                        dlg = $dialogs.create('/dialogs/whatsyourname.html', 'UserProfileCtrl', { $user: $scope.user }, { key: false, back: 'static' });
                        dlg.result.then(function (user) {
                            $scope.user.FirstName = user.FirstName;
                            $scope.user.LastName = user.LastName;
                            $scope.user.Image = user.Image;
                            $scope.UpdateUserProfile();
                        }, function () {
                        });

                        break;
                };
            };

            $scope.getUsersFriends = function () {
                $http({
                    url: '/UserFriends/GetUserFriends',
                    method: "GET",
                    params: { userId: $scope.user.Id }
                }).then(
                    function (successResponse) {
                        $scope.userFriends = successResponse.data.userFriends;
                        //console.log("friend");
                    },
                    function (errorResponse) {
                        // handle errors here
                    });
            };

            $scope.getUserRequests = function () {
                $http({
                    url: '/UserFriends/GetUserRequests',
                    method: "GET",
                    params: { userId: $scope.user.Id }
                }).then(
                    function (successResponse) {
                        $scope.userRequests = successResponse.data.userRequests;
                    },
                    function (errorResponse) {
                        // handle errors here
                    });
            };

            $scope.GetRequestsToUser = function () {
                $http({
                    url: '/UserFriends/GetRequestsToUser',
                    method: "GET",
                    params: { userId: $scope.user.Id }
                }).then(
                    function (successResponse) {
                        $scope.requestsToUser = successResponse.data.toUserRequests;
                        //console.log($scope.requestsToUser);
                    },
                    function (errorResponse) {
                        // handle errors here
                    });
            };

            $scope.setSelectedUser = function (selectedUser) {
                $scope.selectedUser = selectedUser;
                let indexRequestToUser = $scope.userFriends.findIndex(item => item.Id === selectedUser.Id);
                if (indexRequestToUser != -1) {
                    //console.log("false");
                    $scope.canSend = false;
                }
                else {
                    //console.log("true");
                    $scope.canSend = true;
                }
                $scope.GetMessage();
            }

            $scope.GetMessage = function () {
                $http({
                    url: '/UserFriends/GetMessage',
                    method: "GET",
                    params: { userId: $scope.user.Id, withWhomId: $scope.selectedUser.Id }
                }).then(
                    function (successResponse) {
                        $scope.chatMessages = successResponse.data.chatMessages;
                    },
                    function (errorResponse) {
                        // handle errors here
                    });
            };

            $scope.PostMessage = function () {
                if ($scope.message) {
                    var post = $http({
                        method: "POST",
                        url: "/UserFriends/PostMessage",
                        dataType: 'json',
                        data: { message: $scope.message, userId: $scope.user.Id, toWhomId: $scope.selectedUser.Id },
                        headers: { "Content-Type": "application/json" }
                    }).then(
                        function (successResponse) {
                            $scope.message = null;
                            if (successResponse.data.oneMessage != null) {
                                $scope.oneMessage = successResponse.data.oneMessage;
                                $scope.chatHub.server.sendMessage($scope.oneMessage, $scope.user.Id, $scope.selectedUser.Id);
                            }
                        },
                        function (errorResponse) {
                            // handle errors here
                        });
                }
            };

            $scope.chatHub.client.addMessage = function (message) {
                $scope.chatMessages.push(message);
                $scope.$apply();
            };

            $scope.AcceptRequest = function () {
                var post = $http({
                    method: "POST",
                    url: "/UserFriends/AcceptRequest",
                    dataType: 'json',
                    data: { userId: $scope.user.Id, withWhomId: $scope.selectedUser.Id },
                    headers: { "Content-Type": "application/json" }
                }).then(
                    function (successResponse) {
                        $scope.chatHub.server.acceptRequest(successResponse.data.user, successResponse.data.userFriend);
                    },
                    function (errorResponse) {
                        // handle errors here
                    });
            };

            $scope.chatHub.client.updateAcceptedRequest = function (user, userFriend) {
                let indexRequestToUser = $scope.requestsToUser.findIndex(item => item.Id === userFriend.Id);
                if (indexRequestToUser != -1) {
                    $scope.requestsToUser.splice(indexRequestToUser, 1);
                    $scope.userFriends.push(userFriend);
                }
                let indexUserRequest = $scope.userRequests.findIndex(item => item.Id === user.Id);
                if (indexUserRequest != -1) {
                    $scope.userRequests.splice(indexUserRequest, 1);
                    $scope.userFriends.push(user);
                }
                $scope.canSend = false;
                $scope.$apply();
            };

            $scope.DeclineRequest = function () {
                var post = $http({
                    method: "POST",
                    url: "/UserFriends/DeclineRequest",
                    dataType: 'json',
                    data: { userId: $scope.user.Id, withWhomId: $scope.selectedUser.Id },
                    headers: { "Content-Type": "application/json" }
                }).then(
                    function (successResponse) {
                        $scope.chatHub.server.declineRequest(successResponse.data.user, successResponse.data.userFriend);
                    },
                    function (errorResponse) {
                        // handle errors here
                    });
            };

            $scope.chatHub.client.updateDeclineRequest = function (user, userFriend) {
                let indexRequestToUser = $scope.requestsToUser.findIndex(item => item.Id === userFriend.Id);
                if (indexRequestToUser != -1) {
                    $scope.requestsToUser.splice(indexRequestToUser, 1);
                }
                let indexUserRequest = $scope.userRequests.findIndex(item => item.Id === user.Id);
                if (indexUserRequest != -1) {
                    $scope.userRequests.splice(indexUserRequest, 1);
                }
                $scope.$apply();
            };

            $scope.CreateRequest = function () {
                var post = $http({
                    method: "POST",
                    url: "/UserFriends/CreateRequest",
                    dataType: 'json',
                    data: { userId: $scope.user.Id, toWhomId: $scope.selectedUser.Id },
                    headers: { "Content-Type": "application/json" }
                }).then(
                    function (successResponse) {
                        $scope.chatHub.server.createRequest(successResponse.data.user, successResponse.data.userToWhomSendRequest);
                    },
                    function (errorResponse) {
                        // handle errors here
                    });
            };

            $scope.chatHub.client.createNewRequest = function (user, userToWhomSendRequest) {
                if ($scope.selectedUser.Id != userToWhomSendRequest.Id) {
                    $scope.requestsToUser.push(user);
                }
                else {
                    $scope.userRequests.push(userToWhomSendRequest);
                }
                $scope.searchParam = "";
                $scope.findedUsers = {};
                $scope.$apply();
            };

            $scope.checkIfItFirstLogin = function () {
                if ($scope.user.Image == null) {
                    $scope.launch('create');
                }
            };

            $scope.setSearchParam = function (keyEvent) {
                $scope.findedUsers = {};
                $scope.searchUser();
            }

            $scope.searchUser = function () {
                $http({
                    url: '/UserFriends/SearchUser',
                    method: "GET",
                    params: { userName: $scope.searchParam, userId: $scope.user.Id }
                }).then(
                    function (successResponse) {
                        $scope.findedUsers = successResponse.data.findedUsers;
                    },
                    function (errorResponse) {
                        // handle errors here
                    });
            };

            $scope.UserEnter = function () {
                var post = $http({
                    method: "POST",
                    url: "/Account/UserEnter",
                    dataType: 'json',
                    data: { userId: $scope.user.Id },
                    headers: { "Content-Type": "application/json" }
                });
            }

            $scope.chatHub.client.enterUser = function (user) {
                let indexUser = $scope.userFriends.findIndex(item => item.Id === user.Id);
                if (indexUser != -1) {
                    $scope.userFriends[indexUser].MarkedAsLoggedIn = true;
                }
                $scope.$apply();
            };

            $scope.onExit = function () {
                var post = $http({
                    method: "POST",
                    url: "/Account/OfflineUser",
                    dataType: 'json',
                    data: { userId: $scope.user.Id },
                    headers: { "Content-Type": "application/json" }
                }).then(
                    function (successResponse) {
                        $scope.chatHub.server.exit(successResponse.data.user);
                    },
                    function (errorResponse) {
                        // handle errors here
                    });
            };

            $scope.chatHub.client.exitUser = function (user) {
                let indexUser = $scope.userFriends.findIndex(item => item.Id === user.Id);
                if (indexUser != -1) {
                    $scope.userFriends[indexUser].MarkedAsLoggedIn = false;
                }
                $scope.$apply();
            };

            $window.onbeforeunload = $scope.onExit;

            $scope.getUser();
            $scope.getToken();
        }])
        .controller('UserProfileCtrl', function ($scope, $modalInstance, $rootScope, $http) {
            $scope.user = { user: '' };

            $scope.user = $rootScope.user;

            $scope.cancel = function () {
                $modalInstance.dismiss('canceled');
            };

            $scope.save = function () {
                $modalInstance.close($scope.user);
            }; // end save

            $scope.hitEnter = function (evt) {
                if (angular.equals(evt.keyCode, 13) && !(angular.equals($scope.user, null) || angular.equals($scope.user, '')))
                    $scope.save();
            };
        })
        .run(['$templateCache', function ($templateCache) {
            $templateCache.put('/dialogs/whatsyourname.html', '<div class="modal"><div class="modal-dialog"><div class="modal-content"><div class="modal-header"><h4 class="modal-title">User\'s Profile</h4></div><div class="modal-body"><ng-form name="nameDialog" novalidate role="form"><div class="form-group input-group-lg" ng-class="{true: \'has-error\'}[nameDialog.username.$dirty && nameDialog.username.$invalid]"><label class="control-label" for="username">UserName:</label><input type ="text" class="form-control" name="username" id="username" ng-model="user.UserName" ng-keyup="hitEnter($event)" required><label class="control-label" for="username">FirstName:</label><input type ="text" class="form-control" name="username" id="username" ng-model="user.FirstName" ng-keyup="hitEnter($event)" required><label class="control-label" for="username">LastName:</label><input type="text" class="form-control" name="username" id="username" ng-model="user.LastName" ng-keyup="hitEnter($event)" required><label class="control-label" for="username">Photo:</label><input type="text" class="form-control" name="username" id="username" ng-model="user.Image" ng-keyup="hitEnter($event)" required></div></ng-form></div><div class="modal-footer"><button type="button" class="btn btn-default" ng-click="cancel()">Cancel</button><button type="button" class="btn btn-primary" ng-click="save(); UpdateUserProfile()" ng-disabled="(nameDialog.$dirty && nameDialog.$invalid) || nameDialog.$pristine">Save</button></div></div></div></div>');
        }])
})();





//function readURL(input) {
//    if (input.files && input.files[0]) {
//        var reader = new FileReader();

//        reader.onload = function (e) {
//            $('#imageUpload')
//                .attr('src', e.target.result)
//                .width(100)
//                .height(100);
//        };

//        reader.readAsDataURL(input.files[0]);
//    }
//}