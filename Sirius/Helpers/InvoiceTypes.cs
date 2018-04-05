using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.Helpers
{
    public static class InvoiceTypes
    {
        public static class Arrival
        {
            private static Guid _id = new Guid("4C070178-29FB-40A0-ACF9-10DD83641C51");
            /// <summary>
            /// Идентификатор для приходной накладной
            /// </summary>
            public static Guid Id { get { return _id; } }
        }
        public static class Consumption
        {
            private static Guid _id = new Guid("5F726CEC-BD33-4474-817C-0C50DFDDA25C");
            /// <summary>
            /// Идентификатор для приходной накладной
            /// </summary>
            public static Guid Id { get { return _id; } }
        }
        public static class Writeoff
        {
            private static Guid _id = new Guid("EC5CE665-903C-4434-B928-0AEA8F9F45F6");
            /// <summary>
            /// Идентификатор для приходной накладной
            /// </summary>
            public static Guid Id { get { return _id; } }
        }
    }
}
