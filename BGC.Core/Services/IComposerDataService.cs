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
		IList<Composer> GetAllComposers();

        [OperationContract]
        Composer FindComposer(Guid id);

        [OperationContract]
        Composer FindComposerByArticle(Guid articleId);

        [OperationContract]
        IList<ComposerName> GetNames(CultureInfo culture);

        [OperationContract]
        void Add(Composer composer);
	}
}
