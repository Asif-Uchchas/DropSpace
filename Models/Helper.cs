namespace DropSpace.Models
{
	public static class Helper
	{
		private static Random random = new Random();
		public static string Randomstring(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, length)
			  .Select(s => s[random.Next(s.Length)]).ToArray());
		}
	}
}
