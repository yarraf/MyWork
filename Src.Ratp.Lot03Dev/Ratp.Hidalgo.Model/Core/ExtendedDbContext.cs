using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratp.Hidalgo.Model.Core
{
    /// <summary>
    /// Extended Entity Framework context that support caching, logging and audit.
    /// </summary>
    /// <remarks>
    /// Creating an instance of a context costs "nothing".
    /// It is not necessary to cache an instance of the context or to use a singleton.
    /// </remarks>
    public class ExtendedDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedDbContext"/> class with the given connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ExtendedDbContext(string connectionString)
            : base(connectionString)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = true;
            this.Database.CommandTimeout = 600;
        }

        /// <summary>
        /// Gets a compatible connection string for the context.
        /// </summary>
        /// <param name="connectionStringName">The name of the connection string in the configuration file.</param>
        /// <param name="assemblyName">The name of the assembly containing the edmx resource files.</param>
        /// <param name="edmxName">The name of the edmx resource file.</param>
        /// <returns>The connection string to use.</returns>
        public static string GetConnectionString(string connectionStringName, string assemblyName, string edmxName)
        {
            var baseConnectionString = ConfigurationManager.ConnectionStrings[connectionStringName];
            string connectionString = string.Format(
                CultureInfo.InvariantCulture,
                @"metadata=res://{0}/{1}.csdl|res://{0}/{1}.ssdl|res://{0}/{1}.msl;provider={2};provider connection string='{3}'",
                assemblyName,
                edmxName,
                baseConnectionString.ProviderName,
                baseConnectionString.ConnectionString);
            return connectionString;
        }

        /// <summary>
        /// Registers an instance of <see cref="ILogger"/> to be used by the tracing connection.
        /// </summary>
        /// <param name="logger">Logger to register.</param>
        /// <exception cref="ArgumentNullException">When the argument <paramref name="logger"/> is null.</exception>
        //public void RegisterLogger(ILogger logger)
        //{
        //    if (logger == null)
        //    {
        //        throw new ArgumentNullException("logger");
        //    }

        //    this.Database.Log = s => logger.Log(TraceEventType.Verbose, s);
        //}
    }
}
