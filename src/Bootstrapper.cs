using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace IISAdministration
{
    public class Bootstrap
    {
        public static IContainer Components()
        {
            var builder = new ContainerBuilder();
            builder.Register(c => new IisManager()).As<IIisManager>();

            return builder.Build();
        }
    }
}
