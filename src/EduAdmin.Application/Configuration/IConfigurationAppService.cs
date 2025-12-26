using System.Threading.Tasks;
using EduAdmin.Configuration.Dto;

namespace EduAdmin.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
