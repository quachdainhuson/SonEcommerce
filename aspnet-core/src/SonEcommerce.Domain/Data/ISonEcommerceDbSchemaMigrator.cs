using System.Threading.Tasks;

namespace SonEcommerce.Data;

public interface ISonEcommerceDbSchemaMigrator
{
    Task MigrateAsync();
}
