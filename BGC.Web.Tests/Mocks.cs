using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BGC.WebAPI.Tests
{
	public class Mocks
	{
		public static Mock<HttpRequestBase> GetMockRequest()
		{
			return new Mock<HttpRequestBase>();
		}

		public static Mock<HttpContextBase> GetMockContext()
		{
			return new Mock<HttpContextBase>();
		}
	}
}
