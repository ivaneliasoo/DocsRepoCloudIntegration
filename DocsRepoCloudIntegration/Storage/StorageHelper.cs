using System;
using System.IO;
using System.Threading.Tasks;

namespace DocsRepoCloudIntegration
{
    public class StorageHelper : IStorageHelper
    {
        private readonly IOptions<Storage> _storageSettings;

        public StorageHelper(IOptions<StorageSettings> storageSettings)
        {
            _storageSettings = storageSettings ?? throw new ArgumentNullException(nameof(storageSettings));
            if (TempFolderRoute is null)
                TempFolderRoute = _storageSettings.Value.rutaTemporal;
        }

        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public string SaveOnTempFolder(byte[] fileContent, string fileName, bool useUniqueString)
        {
            return Save(fileContent, TempFolderRoute, fileName, useUniqueString);
        }

        public string SaveOnTempFolder(Stream fileContent, string fileName, bool useUniqueString)
        {
            return Save(fileContent, TempFolderRoute, fileName, useUniqueString);
        }

        public async Task<string> SaveOnTempFolderAsync(Stream fileContent, string fileName, bool useUniqueString)
        {
            return await SaveAsync(fileContent, TempFolderRoute, fileName, useUniqueString).ConfigureAwait(false);
        }

        public string Save(byte[] fileContent, string path, string fileName, bool useUniqueString)
        {
            string filePath = GenerateFilePath(path, fileName, useUniqueString);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                stream.Write(fileContent, 0, fileContent.Length);
            }
            return filePath;
        }

        /// <summary>
        /// Permite Guardar un Archivo desde un Stream de forma asincrona
        /// </summary>
        /// <param name="fileContent">InputStream del Archivo subido</param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="extension"></param>
        /// <param name="useUniqueString">indica si se generara un identificador unico para el archivo</param>
        /// <param name="savedFileName">nombre del archivo generado</param>
        /// <returns>Ruta Completa del Archivo guardado</returns>
        public async Task<string> SaveAsync(Stream fileStream, string path, string fileName, bool useUniqueString)
        {
            string filePath = GenerateFilePath(path, fileName, useUniqueString);
            CreatFolderIfNotExists(path);

            using (var stream = File.Create(filePath))
            {
                await fileStream.CopyToAsync(stream).ConfigureAwait(false);
            }
            return filePath;
        }

        /// <summary>
        /// Permite Guardar un Archivo desde un Stream
        /// </summary>
        /// <param name="fileContent">InputStream del Archivo subido</param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="extension"></param>
        /// <param name="useUniqueString">indica si se generara un identificador unico para el archivo</param>
        /// <param name="savedFileName">nombre del archivo generado</param>
        /// <returns>Ruta Completa del Archivo guardado</returns>
        public string Save(Stream fileStream, string path, string fileName, bool useUniqueString)
        {
            string filePath = GenerateFilePath(path, fileName, useUniqueString);

            CreatFolderIfNotExists(path);

            using (var stream = File.Create(filePath))
            {
                fileStream.CopyTo(stream);
            }
            return filePath;
        }

        public string Save(string path, string fileName, bool useUniqueString, out string savedFileName)
        {
            string filePath = GenerateFilePath(path, fileName, useUniqueString);
            if (!File.Exists(filePath))
                File.Create($"{filePath}");
            savedFileName = filePath.Replace(path, "");
            return filePath;
        }

        public bool DeleteFileInFolder(string path, string name)
        {
            
        }

        public void DeleteFolder(string path)
        {
            if (Directory.Exists(path))
            {
                var infoDir = new DirectoryInfo(path);
                foreach (var file in infoDir.GetFiles())
                {
                    file.Delete();
                }
            }
        }

        public void DeleteFolder(string path, bool recursive = true)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path);
            }
        }
    }

}
