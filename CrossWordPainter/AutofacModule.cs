using Autofac;
using Caliburn.Micro;

namespace CrossWord
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WindowManager>().As<IWindowManager>().InstancePerLifetimeScope();
            builder.RegisterType<ShellViewModel>().As<IShell>();

        }
    }
}
