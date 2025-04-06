using Microsoft.AspNetCore.Http;

namespace ElectronicDiary.Web.Api.Other
{
    public interface IController
    {
        Task<string?> GetAll(long schoolId = -1);
        Task<string?> GetById(long id);
        Task<string?> Add(string json);
        Task<string?> Edit(string json);
        Task<string?> Delete(long id);

        Task<string?> AddImage(long id, FileResult  image);
    }
}
