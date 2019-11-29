using Autofac;
using Tree.Service.Interfaces;

namespace Tree.Service
{
    public static class IoC
    {
        public static void RegisterServices(this ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(INodeService).Assembly)
                .Where(w => w.Namespace.Contains("Interfaces") || w.Namespace.Contains("Services"))
                .AsImplementedInterfaces();
        }
    }
}
