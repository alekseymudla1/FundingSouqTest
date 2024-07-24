using Api.Models;
using Domain.Interfaces;

namespace Api.Cache
{
	public class QueryCache : IQueryCache<QueryObject>
	{
		private readonly Dictionary<string, List<QueryObject>> _queries = new Dictionary<string, List<QueryObject>>();

		public QueryCache()
		{

		}

		public void AddQuery(string userName, QueryObject query)
		{
			if (!_queries.ContainsKey(userName))
			{
				_queries.Add(userName, new List<QueryObject>() { query });
			}

			if (!_queries[userName].Exists(qr => qr.Equals(query)))
			{
				_queries[userName].Add(query);
			}

			if (_queries[userName].Count > 3)
			{
				_queries[userName].RemoveRange(0, _queries[userName].Count - 3);
			}
		}

		public IEnumerable<QueryObject> GetQueries(string userName)
		{
			if (_queries.ContainsKey(userName))
			{
				return _queries[userName];
			}

			return new List<QueryObject>();
		}
	}
}
