using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Shared.Entities;
using System.Net;

namespace Orders.Frontend.Pages.Countries
{
	public partial class CountriesIndex
	{
		[Inject] private IRepository Repository { get; set; } = null!;
		[Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
		[Inject] private NavigationManager NavigationManager { get; set; } = null!;
		public List<Country>? Countries { get; set; }
		// Cuando la pagina inicie
		protected async override Task OnInitializedAsync()
		{
			await LoadAsync();
		}

		private async Task LoadAsync()
		{
			var responseHttp = await Repository.GetAsync<List<Country>>("api/countries");
			if (responseHttp.Error)
			{
				var message = await responseHttp.GetErrorMessageAsync();
				await SweetAlertService.FireAsync("ERROR", message, SweetAlertIcon.Error);
				return;
			}
			Countries = responseHttp.Response;
		}

		private async Task DeleteAsync(Country country)
		{
			var result = await SweetAlertService.FireAsync(new SweetAlertOptions
			{
				Title = "CONFIRMACION",
				Text = $"Esta seguro(a) que desea eliminar de forma permanente el país {country.Name}?",
				Icon = SweetAlertIcon.Question,
				ShowCancelButton = true,
			});

			var confirm = string.IsNullOrEmpty(result.Value);
			if (confirm)
			{
				return;
			}

			var responseHttp = await Repository.DeleteAsync<Country>($"api/countries/{country.Id}");
			if (responseHttp.Error)
			{
				if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
				{
					NavigationManager.NavigateTo("/countries");
				}
				else
				{
					var msgError = await responseHttp.GetErrorMessageAsync();
					await SweetAlertService.FireAsync("ERROR", msgError, SweetAlertIcon.Error);
				}
				return;
			}
			await LoadAsync();
			// Mensaje emergente -> 3 segundos
			var toast = SweetAlertService.Mixin(new SweetAlertOptions
			{
				Toast = true,
				Position = SweetAlertPosition.BottomEnd,
				ShowConfirmButton = true,
				Timer = 3000
			});
			await toast.FireAsync(icon: SweetAlertIcon.Success, message: "REGISTRO ELIMINADO CON EXITO!!");
		}
	}
}