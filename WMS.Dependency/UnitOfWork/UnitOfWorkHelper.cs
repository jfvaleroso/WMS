using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using WMS.Core.Repositories;
using WMS.Core.UnitofWork;


namespace WMS.Dependency.UnitOfWork
{
    public static class UnitOfWorkHelper
    {
        public static bool IsRepositoryMethod(MethodInfo methodInfo)
        {
            return IsRepositoryClass(methodInfo.DeclaringType);
        }

        public static bool IsRepositoryClass(Type type)
        {
            return typeof(IRepository).IsAssignableFrom(type);
        }

     

        public static bool HasUnitOfWorkAttribute(MethodInfo methodInfo)
        {
            bool hasUOW= methodInfo.IsDefined(typeof(UnitOfWorkAttribute), true);
            return hasUOW;
        }
    }
}
