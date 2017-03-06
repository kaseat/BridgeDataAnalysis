using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using Autofac;
using Bll.Abstract;
using Bll.Concrete;
using Caliburn.Micro;
using Shel.Extentions;
using Shel.ViewModels;

namespace Shel.Startup
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }
        private IContainer container;

        protected override void OnStartup(Object sender, StartupEventArgs e) => DisplayRootViewFor<ShellViewModel>();
        protected override void BuildUp(Object instance) => container.InjectProperties(instance);

        protected override void Configure()
        {
            var builder = new ContainerBuilder();

            //builder.RegisterModule<Shared.Models.ModuleInitialization.TypesRegistration>();
            //builder.RegisterModule<Shared.Materials.ModuleInitialization.TypesRegistration>();

            builder.RegisterTypes(Assembly.GetExecutingAssembly().GetViewModels());
            builder.RegisterTypes(Assembly.GetExecutingAssembly().GetViews());


            builder.Register<IShellTools>(c => new ShellTool()).InstancePerLifetimeScope();

            builder.Register<IWindowManager>(c => new WindowManager()).InstancePerLifetimeScope();
            builder.Register<IEventAggregator>(c => new EventAggregator()).InstancePerLifetimeScope();
            container = builder.Build();
        }

        protected override IEnumerable<Object> GetAllInstances(Type service) =>
            container.Resolve(typeof(IEnumerable<>).MakeGenericType(service)) as IEnumerable<Object>;

        protected override Object GetInstance(Type service, String key)
        {
            Object obj;
            if (String.IsNullOrWhiteSpace(key))
                if (container.TryResolve(service, out obj))
                    return obj;
                else if (container.TryResolveNamed(key, service, out obj))
                    return obj;

            throw new Exception($"Could not locate any instances of service {key ?? service.Name}.");
        }
    }
}