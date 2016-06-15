using System;
using System.Collections.Generic;
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
		IQueryable<Composer> GetAllComposers();

        [OperationContract]
        void Add(Composer composer);
	}
}
