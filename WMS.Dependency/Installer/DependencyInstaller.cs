using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NHibernate;
using System.Reflection;
using Castle.Core;
using System.Configuration;
using WMS.Dependency.UnitOfWork;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using WMS.NHibernateBase.Mapping;
using WMS.NHibernateBase.Repositories;
using WMS.Core.Services.Implementation;
using WMS.Webservice.Services;


namespace WMS.Dependency.Installer
{
    public class DependencyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Kernel.ComponentRegistered += Kernel_ComponentRegistered;

            //Register all components
            

            container.Register(

                //Nhibernate session factory
                Component.For<ISessionFactory>().UsingFactoryMethod(CreateNhSessionFactory).LifeStyle.Singleton,

                //Unitofwork interceptor
                Component.For<NHUnitOfWorkInterceptor>().LifeStyle.Transient,
              
                //All repositories
                Classes.FromAssembly(Assembly.GetAssembly(typeof(NHProcessRepository))).InSameNamespaceAs<NHProcessRepository>().WithService.DefaultInterfaces().LifestyleTransient(),
            
                //All Services
                Classes.FromAssembly(Assembly.GetAssembly(typeof(ProcessService))).InSameNamespaceAs<ProcessService>().WithService.DefaultInterfaces().LifestyleTransient(),
  
                //All Web Serivices
                 Classes.FromAssembly(Assembly.GetAssembly(typeof(RoleService))).InSameNamespaceAs<RoleService>().WithService.DefaultInterfaces().LifestyleTransient()

                );

            //logger
           // container.AddFacility<LoggingFacility>(f => f.UseLog4Net());
        }

        /// <summary>
        /// Creates NHibernate Session Factory.
        /// </summary>
        /// <returns>NHibernate Session Factory</returns>
        /// register both custom membership mapping and default nhibernatebase mapping
        private static ISessionFactory CreateNhSessionFactory()
        {
            var connStr = ConfigurationManager.ConnectionStrings["WorkflowConn"].ConnectionString;
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(connStr))
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetAssembly(typeof(ProcessMap))))
                .BuildSessionFactory();
        }

        void Kernel_ComponentRegistered(string key, Castle.MicroKernel.IHandler handler)
        {
            //Intercept all methods of all repositories.
            if (UnitOfWorkHelper.IsRepositoryClass(handler.ComponentModel.Implementation))
            {
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(NHUnitOfWorkInterceptor)));
            }

            //Intercept all methods of classes those have at least one method that has UnitOfWork attribute.
            foreach (var method in handler.ComponentModel.Implementation.GetMethods())
            {
                if (UnitOfWorkHelper.HasUnitOfWorkAttribute(method))
                {
                    handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(NHUnitOfWorkInterceptor)));
                    return;
                }
            }
        }
    }
}
