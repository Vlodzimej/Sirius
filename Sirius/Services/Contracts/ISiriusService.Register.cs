using Sirius.Extends.Filters;
using Sirius.Helpers;
using Sirius.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.Services
{
    partial interface ISiriusService
    {
        /// <summary>
        /// Получение регистров по фильтру
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        object GetRegistersByFilter(MetaFilter filter);

        /// <summary>
        /// Получить запись регистра по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Register GetRegisterById(Guid id);

        /// <summary>
        /// Получить запись регистра по идентификатору
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        IEnumerable<Register> GetRegistersByInvoiceId(Guid invoiceId);

        /// <summary>
        /// Получить запись регистра по идентификатору наименования
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<Batch>> GetBatchesByItemId(Guid id);

        /// <summary>
        /// Получить регистры по алиасу типа накладной (arrival/consumption/writeoff)
        /// </summary>
        /// <param name="typeAlias"></param>
        /// <returns></returns>
        IEnumerable<object> GetRegisterByTypeAlias(string typeAlias);

        /// <summary>
        /// Получить остатки согласно критериям фильтрации
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<object> GetBatches(BatchFilter filter);

        /// <summary>
        /// Получить 1 остаток согласно критериям фильтрации
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        object GetBatch(BatchFilter filter);

        /// <summary>
        /// Получить список всех записей регистра
        /// </summary>
        /// <returns></returns>
        IEnumerable<Register> GetAllRegisters();

        /// <summary>
        /// Удалить записть регистра по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteRegisterById(Guid id);

        /// <summary>
        /// Удалить массив регистров по их идентификаторам
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DeleteRegistersById(Guid[] ids);

        /// <summary>
        /// Добавить новый регистр
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        Register AddRegister(Register register);

        /// <summary>
        /// Добавление массива регистров
        /// </summary>
        /// <param name="registers"></param>
        /// <returns></returns>
        IEnumerable<Register> AddRegisters(Register[] registers);

        /// <summary>
        /// Обновление записи регистра
        /// </summary>
        /// <param name="registerId"></param>
        /// <param name="register"></param>
        /// <returns></returns>
        Register UpdateRegister(Guid registerId, Register register);

        /// <summary>
        /// Копирование регистров в целевую накладную
        /// </summary>
        /// <param name="sourceInvoiceId"></param>
        /// <param name="destinationInvoiceId"></param>
        /// <returns></returns>
        IEnumerable<Register> CopyRegisters(Guid sourceInvoiceId, Guid destinationInvoiceId);
    }
}
