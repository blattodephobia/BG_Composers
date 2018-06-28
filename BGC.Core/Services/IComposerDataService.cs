using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Services
{
	[ServiceContract]
	public interface IComposerDataService
	{
		[OperationContract]
		IEnumerable<Composer> GetAllComposers();

        [OperationContract]
        Composer FindComposer(Guid id);
        
        [OperationContract]
        IList<ComposerName> GetNames(CultureInfo culture);

        [OperationContract]
        void AddOrUpdate(Composer composer);
	}
}
