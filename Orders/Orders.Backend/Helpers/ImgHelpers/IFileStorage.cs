using System.Threading.Tasks;

namespace Orders.Backend.Helpers.ImgHelpers
{
    public interface IFileStorage
    {
        // Interface para guardar imagenes en un blob
        Task<string> SaveFileAsync(byte[] content, string extention, string containerName);

        Task RemoveFileAsync(string path, string containerName);

        async Task<string> EditFileAsync(byte[] content, string extention, string containerName, string path)
        {
            if (path is not null)
            {
                await RemoveFileAsync(path, containerName);
            }

            return await SaveFileAsync(content, extention, containerName);
        }

    }
}
