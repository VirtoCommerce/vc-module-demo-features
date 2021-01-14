angular.module('virtoCommerce.DemoSolutionFeaturesModule')
.controller('virtoCommerce.DemoSolutionFeaturesModule.uploadPartIconController',
    ['FileUploader', '$document', '$scope', '$timeout', 'platformWebApp.bladeNavigationService', 'virtoCommerce.catalogModule.catalogImagesFolderPathHelper', 'platformWebApp.assets.api',
    function(FileUploader, $document, $scope, $timeout, bladeNavigationService, catalogImagesFolderPathHelper, assetsApi) {
        const blade = $scope.blade;
        blade.headIcon = 'fa-picture-o';
        blade.isLoading = false;
        blade.downloadImage = false;
        blade.folderPath = catalogImagesFolderPathHelper.getImagesFolderPath(blade.configuredProduct.catalogId, blade.configuredProduct.code);

        blade.toolbarCommands = [{
            name: "platform.commands.confirm",
            icon: 'fa fa-check',
            executeMethod: () => $scope.saveChanges(),
            canExecuteMethod: () => $scope.canSave()
        }];

        function initialize () {
            let uploader = $scope.uploader = new FileUploader({
                scope: $scope,
                //headers: { Accept: 'application/json' }
                autoUpload: true,
                removeAfterUpload: true
            });

            uploader.url = getImageUrl(blade.folderPath);

            uploader.onSuccessItem = (_, images) => {
                setImageUrl(images[0].url);
            };

            uploader.onAfterAddingAll = () => {
                bladeNavigationService.setError(null, blade);
            };

            uploader.onErrorItem = (element, response, status) => {
                bladeNavigationService.setError(`${element._file.name} failed: ${response.message ? response.message : status}`, blade);
            };

            blade.currentEntity = angular.copy(blade.originalEntity);
        }

        function getFolderUrl(path) {
            return `catalog/${path}`;
        }

        function getImageUrl(path) {
            const folderUrl = getFolderUrl(path);
            return `api/platform/assets?folderUrl=${folderUrl}`;
        }

        function setImageUrl(imageUrl) {
            blade.currentEntity.imgSrc = imageUrl;
        }

        $scope.browse = () => {
            $timeout(() => {
                var element = $document[0].querySelector('#selectPartIcon');
                element.click();
            }, 0);
        }

        $scope.addImageFromUrl = () => {
            if (blade.downloadImage) {
                $scope.downloadImageFromUrl();
            } else {
                $scope.setImageUrl();
            }
        };

        $scope.downloadImageFromUrl = () => {
            if (blade.imageUrl) {
                assetsApi.uploadFromUrl({ folderUrl: getFolderUrl(blade.folderPath), url: blade.imageUrl }, x => {
                    setImageUrl(x.Url);
                    blade.imageUrl = null;
                });
            }
        };

        $scope.setImageUrl = () => {
            if (blade.imageUrl) {
                setImageUrl(blade.imageUrl);
                blade.imageUrl = null;
            }
        };

        $scope.canSave = () => {
            return blade.currentEntity.imgSrc !== blade.originalEntity.imgSrc;
        };

        $scope.saveChanges = () => {
            if (blade.onSelect) {
                blade.onSelect(blade.currentEntity);
            }
            blade.originalEntity = blade.currentEntity;
            $scope.bladeClose();
        };

        initialize();
    }]);
