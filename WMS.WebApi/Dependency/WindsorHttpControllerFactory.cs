using Castle.MicroKernel;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Routing;

namespace WMS.WebApi.Dependency
{

    public class WindsorDependencyScope : IDependencyScope
    {
        private IWindsorContainer container;

        public WindsorDependencyScope(IWindsorContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            if (container == null)
                throw new ObjectDisposedException("this", "This scope has already been disposed.");

            if (!container.Kernel.HasComponent(serviceType))
                return null;

            return container.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (container == null)
                throw new ObjectDisposedException("this", "This scope has already been disposed.");

            if (!container.Kernel.HasComponent(serviceType))
                return Enumerable.Empty<object>();

            return container.ResolveAll(serviceType).Cast<object>();
        }

        public void Dispose()
        {
            container.Dispose();
            container = null;
        }
    }

    public class WindsorResolver : WindsorDependencyScope, Castle.MicroKernel.IDependencyResolver
    {
        private readonly IWindsorContainer container;

        public WindsorResolver(IWindsorContainer container)
            : base(container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            this.container = container;
        }

        public IDependencyScope BeginScope()
        {
            var scope = new WindsorContainer();
            container.AddChildContainer(container);
            return new WindsorDependencyScope(scope);
        }
    }
}
