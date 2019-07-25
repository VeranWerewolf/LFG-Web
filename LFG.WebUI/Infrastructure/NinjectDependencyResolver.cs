using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Moq;
using Ninject;
using LFG.Domain.Abstract;
using LFG.Domain.Entities;
using LFG.Domain.Concrete;
using Ninject.Web.Common;

namespace LFG.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }


        // Здесь размещаются привязки
        private void AddBindings()
        {
            //kernel.Bind<IActivityRepository>().ToSelf().InRequestScope();
            //kernel.Bind<IActivityRepository>().ToSelf().InRequestScope();
            kernel.Bind<EFDbContext>().ToSelf().InRequestScope();
            kernel.Bind<IActivityRepository>().To<EFActivityRepository>().InRequestScope();
            kernel.Bind<IActivityTypeRepository>().To<EFActivityTypeRepository>().InRequestScope();
        }
    }
}