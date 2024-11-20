namespace DropSpace.Helpers
{
    public static class ReplaceValues
    {
        public static string? ReplaceSingleQuote(string? Quote)
        {
            if (Quote == null)
            {
                Quote = "";
            }
            string? data = Quote.Replace("'", "\"");
            return data;
        }
    }
}
