using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Shared.Entites;

namespace Orders.Frontend.Pages.Countries
{
	public partial class CountriesIndex
	{
		[Inject] private IRepository repository { get; set; } = null!;
		public List<Country>? Countries { get; set; }
		// Cuando la pagina inicie
		protected async override Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			var responseHttp = await repository.GetAsync<List<Country>>("api/countries");
			Countries = responseHttp.Response;
		}
	}
}
