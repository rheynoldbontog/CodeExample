using System;
using SSG.Core.Configuration;
using SSG.Core.Infrastructure.DependencyManagement;

namespace SSG.Core.Infrastructure
{
    /// <summary>
    /// Classes implementing this interface can serve as a portal for the 
    /// various services composing the SSG engine. Edit functionality, modules
    /// and implementations access most SSG functionality through this 
    /// interface.
    /// </summary>
    public interface IEngine
    {
        ContainerManager ContainerManager { get; }
        
        /// <summary>
        /// Initialize components and plugins in the ssg environment.
        /// </summary>
        /// <param name="config">Config</param>
        void Initialize(SSGConfig config);

        T Resolve<T>() where T : class;

        object Resolve(Type type);

        T[] ResolveAll<T>();
    }
}
