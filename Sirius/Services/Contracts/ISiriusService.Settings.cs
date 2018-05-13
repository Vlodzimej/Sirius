using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.Services
{
    partial interface ISiriusService
    {
        /// <summary>
        /// Получение значение настройки по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetSettingValueById(Guid id);

        /// <summary>
        ///  Получение значение настройки по идентификатору типа и алиасу
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        string GetSettingValueByTypeIdAndAlias(Guid typeId, string alias);

        /// <summary>
        /// Откат базы данных
        /// </summary>
        void RollbackDatabase();
    }
}
