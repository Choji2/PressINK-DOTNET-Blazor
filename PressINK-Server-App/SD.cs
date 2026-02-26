namespace PressINK_Server_App
{
    public class SD
    {

        //********************************************************************************** LDAP API
        public const string LDAP_String = "OU=something";
        public const string LDAP_Engineer = "EN";
        public const string LDAP_Engineer2 = "EN2";
        public const string LDAP_Sales = "SL";

        public List<string> LDAP
        {
            get
            {
                return new(){ "https://addr1.com", "https://addr2.com", "https://addr3.com"
             };
            }
        }
        //**********************************************************************************
        //**************************************************************** API PASS
        public const string REQ_Header = "Security_Header";
        public const string VALID = "Security_string";

        public const int Timeout = 20;
        //****************************************************************


        public const string GODEMODE = "SuperUser";
        public const string USERMODE = "Standard";

        public const string X_HUB_SECRET = "secret";
        public const string X_HUB_HEAD = "X-Hub-Signature";

        public const string Cancel_sandbox = "No changes were saved.";
        public const string Success_string = "Success";       

        public const string APIServer1 = "server1";
        public const string APIServer2 = "server2";

        public const string APIPort1 = "5443";
        public const string APIPort2 = "6443";

        public const string IIS_NAV = "/press-ink";
        public const string IIS_NAV2 = "/ press-ink";

        public const string PlantTag = "Tag";


        //********************************************************** Template flags
        public const string Status__SystemError = "SYSTEM";
        public const string Status__SystemNULL = "NULL";
        public const string Status__SystemNULL_MSG = "No stats were pulled from API.";
        public readonly List<string> Acceptable_status = new(){ "READY", "SLEEP" };
        public readonly List<string> Acceptable_Warnning_status = new() { "PAUSED","BUSY" };
        public readonly List<string> Acceptable_SysErr_status = new() { SD.Status__SystemError,SD.Status__SystemNULL };

    }
}
