(function ()
{
    var FSManagerController = function ($scope, $http)
    {
        $scope.rootLevel = 0;
        var rootLevel = 0;
        $scope.currentDir = "";
        $scope.prevDir = "";
        var currentDir = "";
        var prevDir = "";

        var getDirInfo = function ()
        {
            $('#spinner').addClass('spinner');
            var service = "/api/FSItems/"
            $http.get(service + currentDir.replace(":",""))
            .then(function (response)
            {
                $scope.DirInfo = response.data;
                $('#spinner').removeClass('spinner');

                if ($scope.DirInfo.Error != null)
                {
                    $('#alertButton').click();

                    currentDir = $scope.currentDir;
                    prevDir = $scope.prevDir;
                    rootLevel = $scope.rootLevel;

                    $scope.setCurrentDir($scope.prevDir);
                }
                else
                {
                    $scope.rootLevel = rootLevel;
                    $scope.currentDir = currentDir;
                    $scope.prevDir = prevDir;
                }
            });
        };

        $scope.setCurrentDir = function (dir)
        {
            if (dir == "..")
            {
                currentDir = $scope.prevDir;
                var findBSlash = $scope.prevDir.lastIndexOf('\\');
                if (findBSlash == 2)
                {
                    prevDir = "";
                    rootLevel--;
                }
                else
                {
                    prevDir = $scope.prevDir.substr(0, findBSlash);
                    findBSlash = prevDir.lastIndexOf('\\') + 1;
                    prevDir = prevDir.substr(0, findBSlash);
                    rootLevel--;
                }
            }
            else
            {
                if (currentDir != "")
                {
                    prevDir = currentDir;
                }
                if ($scope.DirInfo.Error == null)
                {
                    currentDir = currentDir + dir + "\\";
                    rootLevel++;
                }
            }

            getDirInfo();
        }

        getDirInfo();
    };

    fsManagerApp.controller("FSManagerController", ["$scope", "$http", FSManagerController])
}());