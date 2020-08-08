using System;
using System.Threading.Tasks;
using Core.Models.File;

namespace Core.Interface
{
    public interface IFileStorage<T>
    {
        Task<T> Connect(string containerName);
        Task<UploadResult> Upload(UploadFile uploadFile);
        Task<DownloadResult> Download(Guid organisationId, Guid attachmentId, string fileName = null);
    }
}
