using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DataBaseProvider;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class DataBaseService : IDataBaseService
    {
        /// <summary>
        /// Провайдер к тестовой базе данных с мок объектами.
        /// </summary>
        private DataBaseProviderBase _mockDataBaseProvider = new LinqToDbProvider();

        /// <summary>
        /// Провайдер к базе данных.
        /// </summary>
        private DataBaseProviderBase _dataBaseProvider = new LinqToDbProvider();

        /// <summary>
        /// Возвращает нужный провайдер к базе данных.
        /// </summary>
        /// <returns>Возвращает нужный провайдер к базе данных.</returns>
        public DataBaseProviderBase GetProvider()
        {
#if (!DEBUG)
            return _dataBaseProvider;
#endif
            return _mockDataBaseProvider;
        }
    }
}
