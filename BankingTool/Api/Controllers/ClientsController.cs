using Api.Models;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Models;
using EF.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUlid;

namespace Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "admin")]
	public class ClientsController : ControllerBase
	{
		private readonly IClientRepository _repo;
		private readonly IQueryCache<QueryObject> _queryCache;

		public ClientsController(IClientRepository repo, IQueryCache<QueryObject> queryCache)
		{
			_repo = repo;
			_queryCache = queryCache;
		}

		[HttpGet()]
		public async Task<IEnumerable<ClientDTO>> GetClientsAsync([FromQuery] QueryObject query, int page = 1, int pageSize = 20)
		{
			_queryCache.AddQuery(HttpContext.User.Identity.Name, query);

			var start = (page - 1) * pageSize;

			return (await _repo.GetClientsAsync(query.GetFilterOptions(), query.GetSortOptions(), start, pageSize))
				.Select(c => new ClientDTO(c));
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetClientAsync(string id)
		{
			var client = await _repo.GetClientAsync(new Ulid(id));
			if(client is null)
			{
				return NotFound();
			}
			return Ok(new ClientDTO(client));
		}

		[HttpPost]
		public async Task<IActionResult> Post(ClientCreateDTO clientCreateDTO)
		{
			var client = clientCreateDTO.ToClient();
			var address = clientCreateDTO.Address();
			await _repo.CreateClientAsync(client, address, clientCreateDTO.AccountNumbers);
			return Created($@"{client.Id}", new ClientDTO(client));
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateClientAsync(string id, ClientUpdateDTO clientUpdateDTO)
		{
			var client = await _repo.GetClientTrackedAsync(new Ulid(id));
			if (client is null)
			{
				return NotFound();
			}
			client = FillFields(client, clientUpdateDTO);
			await _repo.UpdateClientAsync(client, clientUpdateDTO.AccountNumbers);
			return Ok();
		}

		private Client FillFields(Client client, ClientUpdateDTO clientUpdateDTO)
		{
			if(!string.IsNullOrWhiteSpace(clientUpdateDTO.FirstName)) client.FirstName = clientUpdateDTO.FirstName;
			if (!string.IsNullOrWhiteSpace(clientUpdateDTO.LastName)) client.LastName = clientUpdateDTO.LastName;
			if (!string.IsNullOrWhiteSpace(clientUpdateDTO.Email)) client.Email = clientUpdateDTO.Email;
			if (!string.IsNullOrWhiteSpace(clientUpdateDTO.PersonalId)) client.PersonalId = clientUpdateDTO.PersonalId;
			if (!string.IsNullOrWhiteSpace(clientUpdateDTO.MobileNumber)) client.MobileNumber = clientUpdateDTO.MobileNumber;
			if (clientUpdateDTO.Sex.HasValue) client.Sex = clientUpdateDTO.Sex.Value;

			if (!string.IsNullOrWhiteSpace(clientUpdateDTO.Country)) client.Address.Country = clientUpdateDTO.Country;
			if (!string.IsNullOrWhiteSpace(clientUpdateDTO.City)) client.Address.City = clientUpdateDTO.City;
			if (!string.IsNullOrWhiteSpace(clientUpdateDTO.Street)) client.Address.Country = clientUpdateDTO.Street;
			if (!string.IsNullOrWhiteSpace(clientUpdateDTO.ZipCode)) client.Address.ZipCode = clientUpdateDTO.ZipCode;

			return client;
		}
	}
}
