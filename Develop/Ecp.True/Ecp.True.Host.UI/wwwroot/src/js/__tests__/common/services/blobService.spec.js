import blobService from '../../../common/services/blobService';
import * as azure from '../../../scripts/azure-storage.blob.min.js';


jest.mock('../../../scripts/azure-storage.blob.min.js', () => ({
    createBlobServiceWithSas: jest.fn().mockImplementation(() => ({
        getUrl: jest.fn(() => { return 'http://someUrl.com'; }),
        createBlockBlobFromBrowserFile: jest.fn().mockImplementation(() => ({
            getCompletePercent: 100
        }))
    }))
}));


describe('blob Service',

    () => {
        it('should call initialize blobService',
            () => {
                blobService.initialize('testAccountName');
            });

        it('should call uploadToBlob blobService',
            () => {
                const mock = blobService.uploadToBlob = jest.fn(() => 'test sasToken', 'test containerName', 'test blobName', 'test file');
                blobService.uploadToBlob('test sasToken', 'test containerName', 'test blobName', 'test file');
                expect(mock.mock.calls.length).toBe(1);
            });

        it('should call getDownloadLink blobService and get the url.', () => {
            blobService.initialize('testAccountName');
            const result = blobService.getDownloadLink('test sasToken', 'test containerName', 'test blobName');
            expect(azure.createBlobServiceWithSas.mock.calls.length).toBe(1);
            expect(result).toBe('http://someUrl.com');
        });

        it('should call upload to upload the blob.', () => {
            blobService.initialize('testAccountName');
            const result = blobService.downloadFile('test sasToken', 'test containerName', 'test blobName', 'file name');
            expect(result).toBeUndefined();
        });

        it('should download blob thought the promise when result fetch is true', async () => {
            blobService.initialize('testAccountName');
            global.URL.createObjectURL = jest.fn(() => (''));
            global.fetch = jest.fn().mockResolvedValue({ ok: true, blob: jest.fn(() => Promise.resolve(new Blob())) });
            return expect(blobService.downloadSecureFile('test sasToken', 'test containerName', 'test blobName', 'file name')).resolves.toBeUndefined();
        });

        it('should return an error thought the promise when result fetch is false', () => {
            blobService.initialize('testAccountName');
            global.fetch = jest.fn().mockResolvedValue({ ok: false, status: 500, statusText: '' });
            return expect(blobService.downloadSecureFile('test sasToken', 'test containerName', 'test blobName', 'file name'))
            .rejects.toEqual({ ok: false, status: 500, statusText: '' });
        });
    });
