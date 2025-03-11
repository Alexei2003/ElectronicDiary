namespace ElectronicDiary.Web.Api
{
    public interface IController
    {
        Task<string?> GetAll(long schoolId = -1);
        Task<string?> GetById(long id);
        Task<string?> Add(string json);
        Task<string?> Delete(long id);
    }
}
