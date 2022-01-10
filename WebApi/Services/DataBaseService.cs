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
        /// Провайдер к базе данных.
        /// </summary>
        private DataBaseProviderBase _dataBaseProvider = new LinqToDbProvider();

        /// <summary>
        /// Возвращает нужный провайдер к базе данных.
        /// </summary>
        /// <returns>Возвращает нужный провайдер к базе данных.</returns>
        public DataBaseProviderBase GetProvider()
        {
            return _dataBaseProvider;
        }
    }
}
