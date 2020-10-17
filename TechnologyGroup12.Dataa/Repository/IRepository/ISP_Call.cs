using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechnologyGroup12.DataAccess.Repository.IRepository
{
    public interface ISP_Call : IDisposable
    {
        T Single<T>(string proceduceName, DynamicParameters param = null);
        void Excute(string proceduceName, DynamicParameters param = null);
        T OneRecord<T>(string proceduceName, DynamicParameters param = null);
        IEnumerable<T> List<T>(string proceduceName, DynamicParameters param = null);
        Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string proceduceName, DynamicParameters param = null);
    }
}
