namespace ElectronicDiary.Web.Api.Other
{
    public interface IController
    {
        Task<string?> GetAll(long id = -1);
        Task<string?> GetById(long id);
        Task<string?> Add(object request);
        Task<string?> Edit(object request);
        Task<string?> Delete(long id);

        Task<string?> AddImage(long id, FileResult image);
    }
}
