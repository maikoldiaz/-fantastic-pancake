import * as azure from '../../scripts/azure-storage.blob.min.js';
import { utilities } from './utilities';

const blobService = (function () {
    let accountName;
    let onUploadSuccess;
    let onUploadError;
    let onProgressUpdate;
    let finishedOrError;

    function buildUri() {
        return `https://${accountName}.blob.core.windows.net`;
    }

    function init(name, uploadErrorHandler, uploadSuccessHandler, progressUpdateHandler) {
        accountName = name;
        onUploadError = uploadErrorHandler;
        onUploadSuccess = uploadSuccessHandler;
        onProgressUpdate = progressUpdateHandler;
    }

    function getDownloadLink(sasToken, containerName, blobName) {
        const service = azure.createBlobServiceWithSas(buildUri(), sasToken);
        return service.getUrl(containerName, blobName, service.sasToken);
    }

    function upload(sasToken, containerName, blobName, file) {
        finishedOrError = false;
        const service = azure.createBlobServiceWithSas(buildUri(), sasToken);
        blobService.parallelOperationThreadCount = 1;
        const speedSummary = service.createBlockBlobFromBrowserFile(containerName, blobName, file, error => {
            finishedOrError = true;
            if (error && onUploadError) {
                onUploadError(error);
            } else if (onUploadSuccess) {
                onUploadSuccess(true);
            }
        });

        function refreshProgress() {
            setTimeout(function () {
                if (!finishedOrError && onProgressUpdate) {
                    const progress = speedSummary.getCompletePercent();
                    onProgressUpdate(progress);
                    refreshProgress();
                }
            }, 2000);
        }

        refreshProgress();
    }

    function downloadFile(sasToken, containerName, fileName, name) {
        const downLoadLink = getDownloadLink(sasToken, containerName, fileName);
        const link = document.createElement('a');
        if (utilities.isIE()) {
            window.open(downLoadLink);
        } else if (link.download === '') {
            link.setAttribute('href', downLoadLink);
            link.setAttribute('download', name);
            link.style.visibility = 'hidden';

            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        }
    }

    // eslint-disable-next-line valid-jsdoc
    /**
     * Securely download a file, in order to display a user-friendly message to the user when there is an error.
     * @param {string} sasToken sasToken
     * @param {string} containerName containerName
     * @param {string} fileName fileName
     * @param {string} name name
     * @returns Promise, when has an error, user could see an error message
     */
    function downloadSecureFile(sasToken, containerName, fileName, name) {
        const downLoadLink = getDownloadLink(sasToken, containerName, fileName);
        return new Promise(function (resolve, reject) {
            fetch(downLoadLink).then(response => {
                if (!response.ok) {
                    throw response;
                }
                response.blob().then(blob => {
                    if (utilities.isIE()) {
                        window.open(downLoadLink);
                    } else {
                        const d = document.createElement('a');
                        d.className = 'download';
                        d.download = name;
                        d.href = URL.createObjectURL(blob);
                        document.body.appendChild(d);
                        d.click();
                        d.parentElement.removeChild(d);
                    }
                    resolve();
                });
            }).catch(err => {
                console.error(err.status + ' - ' + err.statusText + '\ndownload of ' + downLoadLink + ' error');
                reject(err);
            });
        });
    }

    return {
        initialize: function (name, uploadErrorHandler, uploadSuccessHandler) {
            init(name, uploadErrorHandler, uploadSuccessHandler);
        },
        uploadToBlob: function (sasToken, containerName, blobName, file) {
            upload(sasToken, containerName, blobName, file);
        },
        getDownloadLink: function (sasToken, containerName, blobName) {
            return getDownloadLink(sasToken, containerName, blobName);
        },
        downloadFile: function (sasToken, containerName, fileName, name) {
            return downloadFile(sasToken, containerName, fileName, name);
        },
        downloadSecureFile: function (sasToken, containerName, fileName, name) {
            return downloadSecureFile(sasToken, containerName, fileName, name);
        }
    };
}());

export default blobService;
