using System;

namespace Sirius.Helpers
{
    public static class DefaultValues
    {
        public static class Vendor
        {
            public static class Primary
            {
                private static Guid _id = new Guid("125017A3-5A62-404E-A8A2-1EF87F9777B9");
                /// <summary>
                /// Идентификатор для Основного поставщика
                /// </summary>
                public static Guid Id { get { return _id; } }
            }
        }
    }
}
