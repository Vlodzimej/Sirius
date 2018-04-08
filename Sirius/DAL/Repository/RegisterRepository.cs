using Sirius.Models;
using Sirius.DAL;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Sirius.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sirius.Helpers;

namespace Sirius.Models
{
    public class RegisterRepository : GenericRepository<Register>, IRegisterRepository
    {
        public RegisterRepository(SiriusContext _siriusContext) : base(_siriusContext)
        { }

        public IEnumerable<Register> GetByInvoiceId(Guid invoiceId)
        {
            return _siriusContext.Registers.Where(r => r.InvoiceId == invoiceId);
        }

        public IEnumerable<object> GetByItemId(Guid itemId)
        {
            List<Batch> result = new List<Batch>();

            var arrivalRegisters1 = _siriusContext.Registers.Include(r => r.Item)
                .Include(r => r.Invoice);
            // Получаем регистры прихода
            var registers = _siriusContext.Registers
                .Include(r => r.Item)
                .Include(r => r.Invoice)
                .Where(r => (r.ItemId == itemId) && (r.Invoice.IsFixed == true) && (r.Invoice.Factor > 0));

            List<Batch> batches = new List<Batch>();
            foreach (var register1 in registers)
            {
                decimal cost = register1.Cost;
                double amount = register1.Amount;
                foreach (var register2 in registers)
                {
                    if (register1.Id != register2.Id)
                    {
                        if (register1.Cost == register2.Cost)
                        {
                            amount += register2.Amount;
                        }
                    }
                }
                batches.Add(new Batch()
                {
                    Amount = amount,
                    Cost = cost
                });
            }
            IEnumerable<Batch> arrivalBatch = batches.Distinct();

            // Получаем регистры расхода

            batches = new List<Batch>();

            registers = _siriusContext.Registers
                .Include(r => r.Item)
                .Include(r => r.Invoice)
                .Where(r => (r.ItemId == itemId) && (r.Invoice.IsFixed == true) && (r.Invoice.Factor < 0));

            foreach (var register1 in registers)
            {
                decimal cost = register1.Cost;
                double amount = register1.Amount;
                foreach (var register2 in registers)
                {
                    if (register1.Id != register2.Id)
                    {
                        if (register1.Cost == register2.Cost)
                        {
                            amount += register2.Amount;
                        }
                    }
                }
                batches.Add(new Batch()
                {
                    Amount = amount,
                    Cost = cost
                });
            }
            IEnumerable<Batch> writeOffBatch = batches.Distinct();

            // Вычисляем остатки путём вычитания расходных значений из приходных
            foreach (var register1 in arrivalBatch)
            {
                var batch = register1;
                foreach (var register2 in writeOffBatch)
                {
                    if (register1.Cost == register2.Cost)
                    {
                        batch.Amount = batch.Amount - register2.Amount;
                    }
                }
                if (batch.Amount > 0)
                {
                    result.Add(batch);
                }
            }
            result = result.Distinct().ToList();
            return result;
        }
    }
}