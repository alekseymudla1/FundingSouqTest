namespace Domain.Interfaces
{
	public interface IQueryCache<T>
	{
		void AddQuery(string userName, T item);

		IEnumerable<T> GetQueries(string userName);
	}
}
