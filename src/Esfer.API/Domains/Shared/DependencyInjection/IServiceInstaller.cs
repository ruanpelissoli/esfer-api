namespace Esfer.API.Domains.Shared.DependencyInjection;

public interface IServiceInstaller
{
    void Install(IServiceCollection services, IConfiguration configuration);
}
