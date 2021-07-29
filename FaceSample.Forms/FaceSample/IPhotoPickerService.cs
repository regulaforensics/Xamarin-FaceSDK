using System;
using System.IO;
using System.Threading.Tasks;

namespace FaceSample
{
    public interface IPhotoPickerService
    {
        Task<Stream> GetImageStreamAsync();
    }
}
