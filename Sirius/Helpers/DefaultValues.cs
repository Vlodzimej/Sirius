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
        public static class AccessLevels
        {
            public static class Admin
            {
                private static Guid _id = new Guid("0CE9A36A-7E67-4E66-AA25-1DE6F6462E09");
                /// <summary>
                /// Уровень доступа: Администратор
                /// </summary>
                public static Guid Id { get { return _id; } }
            }
            public static class Viewer
            {
                private static Guid _id = new Guid("E1731D50-21EF-45B1-9B77-EE70AA54D6E5");
                /// <summary>
                /// Уровень доступа: Просмотр
                /// </summary>
                public static Guid Id { get { return _id; } }
            }
        }
    }
}
