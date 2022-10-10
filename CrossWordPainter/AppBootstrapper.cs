using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Autofac;
using Caliburn.Micro;
using CrossWordPainter.Utils;
using FootballWpf;
using KeyTrigger = Microsoft.Xaml.Behaviors.Input.KeyTrigger;

#pragma warning disable CS8600
#pragma warning disable CS8618
#pragma warning disable CS8603

namespace CrossWordPainter
{
    public class AppBootstrapper : BootstrapperBase
    {
        private ILifetimeScope _container;
        public AppBootstrapper() 
        {
            Initialize();
        }

        protected override void Configure()
        {
            var defaultCreateTrigger = Parser.CreateTrigger;

            Parser.CreateTrigger = (target, triggerText) =>
            {
                if (triggerText == null)
                {
                    return defaultCreateTrigger(target, null);
                }

                var triggerDetail = triggerText
                    .Replace(@"[", string.Empty)
                    .Replace(@"]", string.Empty);

                var splits = triggerDetail.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

                switch (splits[0])
                {
                    case "Key":
                        var key = (Key)Enum.Parse(typeof(Key), splits[1], true);
                        return new KeyTrigger { Key = key };

                    case "Gesture":
                        var mkg = (MultiKeyGesture)(new MultiKeyGestureConverter()).ConvertFrom(splits[1]);
                        if (mkg == null) return null;
                        return new KeyTrigger { Modifiers = mkg.KeySequences[0].Modifiers, Key = mkg.KeySequences[0].Keys[0] };
                }

                return defaultCreateTrigger(target, triggerText);
            };
        }
        
        protected override object GetInstance(Type service, string key)
        {
            return string.IsNullOrWhiteSpace(key) ?
                _container.Resolve(service) :
                _container.ResolveNamed(key, service);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.Resolve(typeof(IEnumerable<>).MakeGenericType(service)) as IEnumerable<object>;
        }

        protected override void BuildUp(object instance)
        {
            _container.InjectProperties(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e) 
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AutofacModule>();
            _container = builder.Build();

            DisplayRootViewForAsync<IShell>();
        }
    }
}
