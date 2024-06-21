namespace Se130RPGGame
{
	public static class ExtensionMethods
	{
		public static string GetFullMessage(this Exception ex)
		{
			return ex.InnerException == null

				? ex.Message
				: $"{ex.Message} --> {ex.InnerException.GetFullMessage()}";
		}
	}
}
