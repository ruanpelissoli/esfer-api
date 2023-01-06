namespace Esfer.API.Shared.DependencyInjection;

public interface IServiceInstaller
{
    void Install(IServiceCollection services, IConfiguration configuration);
}
