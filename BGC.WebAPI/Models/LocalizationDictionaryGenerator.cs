using BGC.WebAPI.Models;

public partial class LocalizationDictionary
{
public LocalizedString LocalizationSuffix { get; private set; }
public AdministrationAreaLocalization AdministrationArea { get; private set; }
public GenericTextLocalization GenericText { get; private set; }
public partial class AdministrationAreaLocalization
{
public AdministrationLocalization Administration { get; private set; }
public partial class AdministrationLocalization
{
public UsersLocalization Users { get; private set; }
public partial class UsersLocalization
{
public LocalizedString Ok { get; private set; }
         }
     }
 }
public partial class GenericTextLocalization
{
 }
}
