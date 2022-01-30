using System.IO;

namespace file_drop_sample.Models
{
    public class FileModel
    {
        public FileModel(string? file)
        {
            this.File = file;
            this.FileName = Path.GetFileName(file);
        }

        public string? FileName { get; set; }
        public string? File { get; set; }
    }
}