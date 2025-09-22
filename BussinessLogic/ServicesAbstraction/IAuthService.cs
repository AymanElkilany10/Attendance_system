using BussinessLogic.Models._EmployeeDto;
using BussinessLogic.ViewModels;
using System.Threading.Tasks;

namespace BussinessLogic.ServicesAbstraction
{
    public interface IAuthService
    {
        Task<ApiResponse<string>> Register(CreatedEmployeeDto dto);
        Task<ApiResponse<string>> Login(string username, string password);
    }
}
