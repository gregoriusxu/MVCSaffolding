using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Helper
{
    public class RepositoryRegister
    {
        public static IDictionary<Type, Type> GetRepositotyListForDbContext(Type interfaceType, Type implemenType, Type dbContextType)
        {
            IDictionary<Type, Type> repositorylist = new Dictionary<Type, Type>();

            foreach (var entityType in dbContextType.GetEntityTypes())
            {
                var genericRepositoryType = interfaceType.MakeGenericType(entityType);
                var implementationType = implemenType;

                var implType = implementationType.GetGenericArguments().Length == 1
                        ? implementationType.MakeGenericType(entityType)
                        : implementationType.MakeGenericType(dbContextType, entityType);

                repositorylist.Add(genericRepositoryType, implType);
            }

            return repositorylist;
        }


    }
}
