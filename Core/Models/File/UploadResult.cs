namespace Core.Models.File
{
    public class UploadResult
    {
        public bool Success { get; set; }
        public string StorageFileName { get; set; }
        public string Uri { get; set; }
    }
}
