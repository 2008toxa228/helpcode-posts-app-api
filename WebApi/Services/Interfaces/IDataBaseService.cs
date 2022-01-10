using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DataBaseProvider;

namespace WebApi.Services.Interfaces
{
    public interface IDataBaseService
    {
        public DataBaseProviderBase GetProvider(); 
    }
}
