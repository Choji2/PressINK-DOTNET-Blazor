namespace PressINK_Server_App.Model.Auth._Models
{
    public class LDAP_Obj
    {

        public string cn { get; set; }
        public string physicalDeliveryOfficeName { get; set; }
        public string displayName { get; set; }
        public string department { get; set; }
        public string company { get; set; }
        public string mail { get; set; }
        public IEnumerable<string> role { get; set; }

    }
}
