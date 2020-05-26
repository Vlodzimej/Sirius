using System;

namespace Sirius.Models.Dtos
{
    /// <summary>
    /// Элемент отчета по приходу и расходу
    /// </summary>
    public class ReportItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Incoming { get; set; }
        public double Consumption { get; set; }
        public double Total { get; set; }
        public string Dimension { get; set; }
    }
}