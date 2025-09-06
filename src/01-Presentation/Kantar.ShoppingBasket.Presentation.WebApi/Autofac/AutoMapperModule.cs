using Autofac;
using AutoMapper;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging.Abstractions;
using System.Reflection;
using Module = Autofac.Module;

namespace Kantar.ShoppingBasket.Presentation.WebApi.Autofac
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .Register(context => new MapperConfiguration(
                    cfg =>
                    {
                        cfg.AddMaps(this.GetApplicationAssemblies());
                    },
                    new NullLoggerFactory()))
                .AsSelf()
                .SingleInstance();

            builder
                .Register(
                    ctx =>
                    {
                        var config = ctx.Resolve<MapperConfiguration>();
                        return config.CreateMapper(ctx.Resolve);
                    })
                .As<IMapper>()
                .SingleInstance();
        }

        private List<Assembly> GetApplicationAssemblies()
        {
            var appAssemblies = new List<Assembly>();

            foreach(var library in DependencyContext.Default.CompileLibraries)
            {
                if (library.Name.StartsWith("Kantar"))
                {
                    try
                    {
                        appAssemblies.Add(Assembly.Load(new AssemblyName(library.Name)));
                    }
                    catch
                    {
                    }
                }
            }

            return appAssemblies;
        }
    }
}
