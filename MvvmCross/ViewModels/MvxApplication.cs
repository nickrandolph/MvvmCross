// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using MvvmCross.IoC;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin;

namespace MvvmCross.ViewModels
{
    public abstract class MvxApplication : IMvxApplication
    {
        private IMvxViewModelLocator _defaultLocator;

        private IMvxViewModelLocator DefaultLocator
        {
            get
            {
                _defaultLocator = _defaultLocator ?? CreateDefaultViewModelLocator();
                return _defaultLocator;
            }
        }

        protected virtual IMvxViewModelLocator CreateDefaultViewModelLocator()
        {
            return new MvxDefaultViewModelLocator();
        }

        public virtual void LoadPlugins(IMvxPluginManager pluginManager)
        {
            // do nothing
        }

        /// <summary>
        /// Any initialization steps that can be done in the background
        /// </summary>
        public virtual void Initialize()
        {
            // do nothing
        }

        /// <summary>
        /// Any initialization steps that need to be done on the UI thread
        /// </summary>
        public virtual Task Startup()
        {
            MvxLog.Instance.Trace("AppStart: Application Startup - On UI thread");
            return Task.CompletedTask;
        }

        /// <summary>
        /// If the application is restarted (eg primary activity on Android 
        /// can be restarted) this method will be called before Startup
        /// is called again
        /// </summary>
        public virtual void Reset()
        {
            // do nothing
        }

        public IMvxViewModelLocator FindViewModelLocator(MvxViewModelRequest request)
        {
            return DefaultLocator;
        }

        protected void RegisterCustomAppStart<TMvxAppStart>()
            where TMvxAppStart : class, IMvxAppStart
        {
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IMvxAppStart, TMvxAppStart>();
        }

        protected void RegisterAppStart<TViewModel>()
            where TViewModel : IMvxViewModel
        {
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IMvxAppStart, MvxAppStart<TViewModel>>();
        }

        protected void RegisterAppStart(IMvxAppStart appStart)
        {
            Mvx.IoCProvider.RegisterSingleton(appStart);
        }

        protected virtual void RegisterAppStart<TViewModel, TParameter>()
          where TViewModel : IMvxViewModel<TParameter>
        {
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IMvxAppStart, MvxAppStart<TViewModel, TParameter>>();
        }

        protected IEnumerable<Type> CreatableTypes()
        {
            return CreatableTypes(GetType().GetTypeInfo().Assembly);
        }

        protected IEnumerable<Type> CreatableTypes(Assembly assembly)
        {
            return assembly.CreatableTypes();
        }


        protected virtual void RegisterNavigation<TCurrentViewModel, TNewViewModel>(Func<TCurrentViewModel, bool> shouldNavigate = null)
            where TCurrentViewModel : class, IMvxViewModelCompleted, IMvxViewModel
            where TNewViewModel : IMvxViewModel
        {
            var navService = Mvx.IoCProvider.Resolve<IMvxNavigationService>() as MvxNavigationService;
            var newDefinition = new Action<IMvxViewModel, IMvxNavigationService>((obj, nav) =>
            {
                var vm = obj as TCurrentViewModel;
                if (vm != null)
                {
                    vm.OnCompleted = () =>
                    {
                        if (shouldNavigate?.Invoke(vm) ?? true)
                            return nav.Navigate<TNewViewModel>();

                        return Task.CompletedTask;
                    };
                }
            });
            navService.AddNavigation<TCurrentViewModel>(newDefinition);
        }

        protected virtual void RegisterCompletion<TCurrentViewModel>(Func<TCurrentViewModel, bool> shouldComplete = null)
            where TCurrentViewModel : class, IMvxViewModelCompleted, IMvxViewModel
        {
            var navService = Mvx.IoCProvider.Resolve<IMvxNavigationService>() as MvxNavigationService;
            var newDefinition = new Action<IMvxViewModel, IMvxNavigationService>((obj, nav) =>
            {
                var vm = obj as TCurrentViewModel;
                if (vm != null)
                {
                    vm.OnCompleted = () =>
                    {
                        if (shouldComplete?.Invoke(vm) ?? true)
                            return nav.Close(vm);

                        return Task.CompletedTask;
                    };
                }
            });
            navService.AddNavigation<TCurrentViewModel>(newDefinition);
        }

        protected virtual void RegisterNext<TCurrentViewModel, TNewViewModel, TNextViewModel>(TNextViewModel nextAction, Func<TCurrentViewModel, bool> shouldNavigate = null)
           where TCurrentViewModel : class, IMvxNextViewModel<TNextViewModel>, IMvxViewModel
           where TNewViewModel : IMvxViewModel
            where TNextViewModel : struct
        {
            var navService = Mvx.IoCProvider.Resolve<IMvxNavigationService>() as MvxNavigationService;
            var expectedNext = nextAction;
            var newDefinition = new Action<IMvxViewModel, IMvxNavigationService>((obj, nav) =>
            {
                var vm = obj as TCurrentViewModel;
                if (vm != null)
                {
                    vm.OnNext += (next) =>
                    {
                        if (expectedNext.Equals(next) && (shouldNavigate?.Invoke(vm) ?? true))
                            return nav.Navigate<TNewViewModel>();

                        return Task.CompletedTask;
                    };
                }
            });
            navService.AddNavigation<TCurrentViewModel>(newDefinition);
        }

        protected virtual void RegisterNextCompletion<TCurrentViewModel, TNextViewModel>(TNextViewModel nextAction, Func<TCurrentViewModel, bool> shouldComplete = null)
            where TCurrentViewModel : class, IMvxNextViewModel<TNextViewModel>, IMvxViewModel
            where TNextViewModel: struct
        {
            var navService = Mvx.IoCProvider.Resolve<IMvxNavigationService>() as MvxNavigationService;
            var expectedNext = nextAction;
            var newDefinition = new Action<IMvxViewModel, IMvxNavigationService>((obj, nav) =>
            {
                var vm = obj as TCurrentViewModel;
                if (vm != null)
                {
                    vm.OnNext += (next) =>
                    {
                        if (expectedNext.Equals(next) && (shouldComplete?.Invoke(vm) ?? true))
                            return nav.Close(vm);

                        return Task.CompletedTask;
                    };
                }
            });
            navService.AddNavigation<TCurrentViewModel>(newDefinition);
        }
    }

    public class MvxApplication<TParameter> : MvxApplication, IMvxApplication<TParameter>
    {
        public virtual TParameter Startup(TParameter parameter)
        {
            // do nothing, so just return the original hint
            return parameter;
        }
    }
}
