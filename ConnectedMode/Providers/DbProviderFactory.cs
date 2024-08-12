using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedMode.Providers
{
    internal class DbProviderFactory
    {
        public static IDbProvider CreateProvider()
        {
            return new SqlDbProvider();
        }
    }
}
