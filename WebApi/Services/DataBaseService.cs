using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DataBaseProvider;
using WebApi.DataBaseProvider.Interfaces;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class DataBaseService : IDataBaseService
    {
        /// <summary>
        /// Провайдер к тестовой базе данных с мок объектами.
        /// </summary>
        private IDataBaseProvider _mockDataBaseProvider = new MockDataBaseProvider();

        /// <summary>
        /// Провайдер к базе данных.
        /// </summary>
        private IDataBaseProvider _dataBaseProvider = new MockDataBaseProvider();

        /// <summary>
        /// Возвращает нужный провайдер к базе данных.
        /// </summary>
        /// <returns>Возвращает нужный провайдер к базе данных.</returns>
        public IDataBaseProvider GetProvider()
        {
#if (!DEBUG)
            return _dataBaseProvider;
#endif
            return _mockDataBaseProvider;
        }
    }
}
