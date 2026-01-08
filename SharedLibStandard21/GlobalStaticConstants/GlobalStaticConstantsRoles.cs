////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib
{
    /// <summary>
    /// Константы
    /// </summary>
    public static partial class GlobalStaticConstantsRoles
    {
        /// <summary>
        /// Roles
        /// </summary>
        public static class Roles
        {
            /// <summary>
            /// admin
            /// </summary>
            public const string Admin = "admin";

            /// <summary>
            /// Debug
            /// </summary>
            public const string Debug = "debug";

            /// <summary>
            /// system
            /// </summary>
            public const string System = "system";

            /// <summary>
            /// GoodsManage
            /// </summary>
            public const string GoodsManage = "GoodsManage";

            #region retail
            /// <summary>
            /// RetailManage
            /// </summary>
            public const string RetailManage = "RetailManage";

            /// <summary>
            /// RetailReports
            /// </summary>
            public const string RetailReports = "RetailReports";
            #endregion

            #region HelpDesk
            static string[]? _allHelpDeskRoles = null;
            /// <summary>
            /// Все роли HelpDesk
            /// </summary>
            public static string[] AllHelpDeskRoles
            {
                get
                {
                    _allHelpDeskRoles ??= [HelpDeskManager, HelpDeskUnit, HelpDeskRubricsManage, HelpDeskChatsManage];
                    return _allHelpDeskRoles;
                }
            }

            /// <summary>
            /// Рубрики + <see cref="HelpDeskUnit"/> (таблица заявок клиентов связанных с текущим сотрудником)
            /// </summary>
            public const string HelpDeskRubricsManage = "HelpDeskRubricsManage";

            /// <summary>
            /// Чаты + <see cref="HelpDeskUnit"/> (таблица заявок клиентов связанных с текущим сотрудником)
            /// </summary>
            public const string HelpDeskChatsManage = "HelpDeskChatsManage";

            /// <summary>
            /// Консоль + <see cref="HelpDeskUnit"/> (таблица заявок клиентов связанных с текущим сотрудником)
            /// </summary>
            public const string HelpDeskManager = "HelpDeskManager";

            /// <summary>
            /// Таблица заявок клиентов связанных с текущим сотрудником
            /// </summary>
            public const string HelpDeskUnit = "HelpDeskUnit";

            /// <summary>
            /// CommerceManager
            /// </summary>
            public const string CommerceManager = "CommerceManager";

            /// <summary>
            /// CommerceClient
            /// </summary>
            public const string CommerceClient = "CommerceClient";

            /// <summary>
            /// AttendancesExecutor
            /// </summary>
            public const string AttendancesExecutor = "AttendancesExecutor";
            #endregion
        }
    }
}