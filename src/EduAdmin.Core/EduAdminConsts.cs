using EduAdmin.Debugging;

namespace EduAdmin
{
    public class EduAdminConsts
    {
        public const string LocalizationSourceName = "EduAdmin";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "1b6737782bd84c8394e70b8e55d9b39b";
    }
}
