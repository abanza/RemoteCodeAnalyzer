// CodeAnalyzerUser.cs                                                            

namespace SharedObjects
{
	public class CodeAnalyzerUser
	{
		public int Id { get; set; }
		public string Role { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }

		public bool IsAdmin()
		{
			return Role == "Administrator";
		}
	}
}
