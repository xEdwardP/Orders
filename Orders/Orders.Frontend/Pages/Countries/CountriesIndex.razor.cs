using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Shared.Entities;
using System.Net;

namespace Orders.Frontend.Pages.Countries
{
	public partial class CountriesIndex
	{
		[Inject] private IRepository repository { get; set; } = null!;
		[Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
		[Inject] private NavigationManager navigationManager { get; set; } = null!;
		public List<Country>? Countries { get; set; }
		// Cuando la pagina inicie
		protected async override Task OnInitializedAsync()
		{
			await LoadAsync();
		}

		private async Task LoadAsync()
		{
			var responseHttp = await repository.GetAsync<List<Country>>("api/countries");
			if (responseHttp.Error)
			{
				var message = await responseHttp.GetErrorMessageAsync();
				await sweetAlertService.FireAsync("ERROR", message, SweetAlertIcon.Error);
				return;
			}
			Countries = responseHttp.Response;
		}

		private async Task DeleteAsync(Country country)
		{
			var result = await sweetAlertService.FireAsync(new SweetAlertOptions
			{
				Title = "CONFIRMACION",
				Text = $"ESTA SEGURO(A) QUE DESEA ELIMINAR DE FORMA PERMANENTE EL PAIS {country.Name}?",
				Icon = SweetAlertIcon.Question,
				ShowCancelButton = true,
			});

			var confirm = string.IsNullOrEmpty(result.Value);
			if (confirm)
			{
				return;
			}

			var responseHttp = await repository.DeleteAsync<Country>($"api/countries/{country.Id}");
			if (responseHttp.Error)
			{
				if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
				{
					navigationManager.NavigateTo("/countries");
				}
				else
				{
					var msgError = await responseHttp.GetErrorMessageAsync();
					await sweetAlertService.FireAsync("ERROR", msgError, SweetAlertIcon.Error);
				}
				return;
			}
			await LoadAsync();
			// Mensaje emergente -> 5 segundos
			var toast = sweetAlertService.Mixin(new SweetAlertOptions
			{
				Toast = true,
				Position = SweetAlertPosition.BottomEnd,
				ShowConfirmButton = true,
				Timer = 5000
			});
			await toast.FireAsync(icon: SweetAlertIcon.Success, message: "REGISTRO ELIMINADO CON EXITO!!");
		}
	}
}
