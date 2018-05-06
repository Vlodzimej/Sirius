using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.Helpers
{
    public class Types
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
            public static class Template
            {
                private static Guid _id = new Guid("8E8FE3F6-3F03-46DD-95F5-4CE024FB8CFF");
                /// <summary>
                /// Идентификатор для шаблона (шаблоны используются для составления содержания услуг)
                /// </summary>
                public static Guid Id { get { return _id; } }
            }
        }
        public static class SettingsTypes
        {
            public static class Invoice
            {
                private static Guid _id = new Guid("6C1A7963-0EB3-4FE1-9F22-EA0E9CAD8DCC");
                /// <summary>
                /// Идентификатор настроек накладных
                /// </summary>
                public static Guid Id { get { return _id; } }

                public static class Prefix
                {
                    private static Guid _id = new Guid("26E7180D-6ACC-4B4A-B2DF-7D8190234090");
                    /// <summary>
                    /// Идентификатор настроек накладных
                    /// </summary>
                    public static Guid Id { get { return _id; } }
                }
            }
        }
    }
}
