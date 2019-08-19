// CheckInData.cs                                                                 

using System.IO;
using System.ServiceModel;

namespace RemoteRepositoryService
{
	[MessageContract]
	public class CheckInData
	{
		[MessageHeader(MustUnderstand = true)]
		public string Username { get; set; }
		[MessageHeader(MustUnderstand = true)]
		public string Project { get; set; }
		[MessageHeader(MustUnderstand = true)]
		public string Filename { get; set; }
		[MessageBodyMember(Order = 1)]
		public Stream File { get; set; }
	}
}