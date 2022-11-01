using System.Threading.Tasks;

namespace Framework.Core.UserDataManagement
{
    public interface IUserDataManager
    {
        bool IsInRole(string role);
        string GetAccessibleCenters();
        Task SetDefaultActiveCenterId();
    }
}
