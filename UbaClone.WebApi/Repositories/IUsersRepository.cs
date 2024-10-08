namespace UbaClone.WebApi.Repositories
{
    public interface IUsersRepository
    {
        Task<Models.UbaClone?> CreateUserAsync(Models.UbaClone user);
        Task<Models.UbaClone[]> RetrieveAllAsync();
        Task<Models.UbaClone?> RetrieveAsync(int id);
        Task<Models.UbaClone?> UpdateUserAsync(Models.UbaClone user);
        Task<bool?> DeleteUserAsync(int id);
    }
}
