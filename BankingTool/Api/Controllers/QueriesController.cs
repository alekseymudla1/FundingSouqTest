using Api.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class QueriesController : ControllerBase
	{

		private readonly IQueryCache<QueryObject> _queryCache;

		public QueriesController(IQueryCache<QueryObject> queryCache)
		{
			_queryCache = queryCache;
		}

		[HttpGet]
		public IEnumerable<QueryObject> Get()
		{
			return _queryCache.GetQueries(HttpContext.User.Identity.Name);
		}
	}
}
