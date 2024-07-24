namespace EF.Extentions
{
	public static class QueryExtentions
	{
		public static IEnumerable<T> AddFilter<T>(this IEnumerable<T> query, Func<T, bool> filterExpression) where T : class
		{
			query.Where(filterExpression.Invoke);

			return query;
		}
	}
}
