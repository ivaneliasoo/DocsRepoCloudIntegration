using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DocsRepoCloudIntegration
{
    class FileSystemDriver : IStorageDriver
    {
        public Task CopyFile(string source, string target, bool ovewrite = true)
        {
            if (File.Exists(source))
                File.Copy(source, target, ovewrite);

            return Task.CompletedTask;
        }

        public Task CreatFolderIfNotExists(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            return Task.CompletedTask;
        }

        public Task DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            return Task.CompletedTask;
        }

        public ValueTask<bool> DeleteFileInFolder(string path, string name)
        {
            //Buscar el archivo fisico para eliminarlo.

            try
            {
                var files = Directory.GetFiles(path, name);

                foreach (string file in files)
                {
                    File.Delete(file);
                }

                return ValueTask.FromResult(true);
            }
            catch (Exception)
            {
                return ValueTask.FromResult(false);
            }
        }

        public Task DeleteFilesInFolder(string path)
        {
            if (Directory.Exists(path))
            {
                var infoDir = new DirectoryInfo(path);
                foreach (var file in infoDir.GetFiles())
                {
                    file.Delete();
                }
            }
            return Task.CompletedTask;
        }

        public Task DeleteFolder(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path);

            return Task.CompletedTask;
        }

        public Task DeleteFolder(string path, bool recursive = true)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> FileExists(string filePath)
        {
            var result = File.Exists(filePath);
            return ValueTask.FromResult(result);
        }

        public string GenerateFilePath(string path, string fileName, bool useUniqueString = false)
        {
            //Reemplazar con underscore los caranteres invalidos en el path
            fileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));

            if (useUniqueString)
                return Path.Combine(path, fileName.Insert(fileName.LastIndexOf('.'), DateTime.Now.Ticks.ToString()));
            else
                return Path.Combine(path, fileName);
        }

        public string[] GetFilesInFolder(string folder)
        {
            try
            {
                return Directory.GetFiles(folder);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MemoryStream> GetMemoryStreamFromFile(string fullPath)
        {
            //COMPLETED: Crear metodo en StorageHelper para Leer archivos 
            MemoryStream msResult;//lo cambio a MemoryStream porque a FileStream se debe cerrar y se le debe hacer Dispose, en cambio MemoryStream es un Objeto totalmente administrado y sera desechado por GC
            if (File.Exists(fullPath))
            {
                using (var file = File.OpenRead(fullPath))//nos aseguramos de hacer dispose del recurso que lee el archivo
                {
                    msResult = new MemoryStream();
                    await file.CopyToAsync(msResult).ConfigureAwait(false);
                    file.Close();//cerramos el archivo
                }
            }
            else
                throw new FileNotFoundException("El Archivo no existe", fullPath);

            return Task.FromResult(msResult);

        }

        public Task<IEnumerable<string>> ListAsync(string path = "")
        {
            throw new NotImplementedException();
        }

        public Task Move(string sourceFileName, string pathDest)
        {
            if (File.Exists(sourceFileName))
            {
                try
                {
                    var fileName = Path.GetFileName(sourceFileName);

                    File.Move(sourceFileName, $"{pathDest}{fileName}");
                }
                catch (Exception e)
                {
                    throw new Exception("No se ha podido mover el archivo: " + e.Message);
                }
            }
            else
            {
                throw new FileNotFoundException("El archivo no existe", sourceFileName);
            }
        }

        public string Save(byte[] fileContent, string path, string fileName, bool useUniqueString)
        {
            throw new NotImplementedException();
        }

        public string Save(Stream fileStream, string path, string fileName, bool useUniqueString)
        {
            throw new NotImplementedException();
        }

        public string Save(string path, string fileName, bool useUniqueString, out string savedFileName)
        {
            throw new NotImplementedException();
        }

        public Task<string> SaveAsync(Stream fileStream, string path, string fileName, bool useUniqueString)
        {
            throw new NotImplementedException();
        }

        public string SaveOnTempFolder(byte[] fileContent, string fileName, bool useUniqueString)
        {
            throw new NotImplementedException();
        }

        public string SaveOnTempFolder(Stream fileContent, string fileName, bool useUniqueString)
        {
            throw new NotImplementedException();
        }

        public Task<string> SaveOnTempFolderAsync(Stream fileContent, string fileName, bool useUniqueString)
        {
            throw new NotImplementedException();
        }
    }

}
