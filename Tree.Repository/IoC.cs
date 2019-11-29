using Autofac;
using Tree.Repository.Interfaces;

namespace Tree.Repository
{
    public static class IoC
    {
        public static void RegisterRepositories(this ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(INodeRepository).Assembly)
                .Where(w => w.Namespace.Contains("Interfaces") || w.Namespace.Contains("Repositories"))
                .AsImplementedInterfaces();
        }
    }
}
