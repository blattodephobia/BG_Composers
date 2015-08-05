using System.Collections.Generic;

namespace BGC.WebAPI
{
	public partial class LocalizationDictionary
    {
        public partial class AdministrationAreaLocalization
        {
            public partial class AdministrationLocalization
            {
                public partial class UsersLocalization
                {
                    public UsersLocalization(IDictionary<string, string> localizedStrings)
                    {
                        this.LocalizedStrings = localizedStrings;
                    }
                    
                    private IDictionary<string, string> LocalizedStrings { get; set; }
                }
                public UsersLocalization Users { get; private set; }
                
                public AdministrationLocalization(IDictionary<string, string> localizedStrings)
                {
                    this.LocalizedStrings = localizedStrings;
                    
                    this.Users = new UsersLocalization(this.LocalizedStrings);
                }
                
                private IDictionary<string, string> LocalizedStrings { get; set; }
            }
            public AdministrationLocalization Administration { get; private set; }
            
            public AdministrationAreaLocalization(IDictionary<string, string> localizedStrings)
            {
                this.LocalizedStrings = localizedStrings;
                
                this.Administration = new AdministrationLocalization(this.LocalizedStrings);
            }
            
            private IDictionary<string, string> LocalizedStrings { get; set; }
        }
        public partial class GenericTextLocalization
        {
            public LocalizedString Ok { get; private set; }
            public LocalizedString Cancel { get; private set; }
            public GenericTextLocalization(IDictionary<string, string> localizedStrings)
            {
                this.LocalizedStrings = localizedStrings;
                this.Ok = new LocalizedString(GenericTextLocalization.ok, this.LocalizedStrings);
                this.Cancel = new LocalizedString(GenericTextLocalization.cancel, this.LocalizedStrings);
            }
            
            private IDictionary<string, string> LocalizedStrings { get; set; }
        }
        public AdministrationAreaLocalization AdministrationArea { get; private set; }
        public GenericTextLocalization GenericText { get; private set; }
        
        public LocalizationDictionary(IDictionary<string, string> localizedStrings)
        {
            this.LocalizedStrings = localizedStrings;
            
            this.AdministrationArea = new AdministrationAreaLocalization(this.LocalizedStrings);
            this.GenericText = new GenericTextLocalization(this.LocalizedStrings);
        }
        
        private IDictionary<string, string> LocalizedStrings { get; set; }
    }
}
