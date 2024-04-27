using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Shared.Entites;
using System.Net;

namespace Orders.Frontend.Pages.Countries
{
	public partial class CountryEdit
	{
		private Country? country;
		private CountryForm? countryForm;
		[Inject] private IRepository repository { get; set; } = null!;
		[Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
		[Inject] private NavigationManager navigationManager { get; set; } = null!;

		[EditorRequired, Parameter] public int Id { get; set; }

		protected async override Task OnParametersSetAsync()
		{
			var responseHttp = await repository.GetAsync<Country>($"/api/countries/{Id}");
			if (responseHttp.Error)
			{
				if(responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
				{
					navigationManager.NavigateTo("/countries");
				}
				else
				{
					var message = await responseHttp.GetErrorMessageAsync();
					await sweetAlertService.FireAsync("ERROR", message, SweetAlertIcon.Error);
				}
			}
			else
			{
				country = responseHttp.Response;
			}
		}

		private async Task EditAsync()
		{
			var responseHttp = await repository.PutAsync("/api/countries", country);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
				await sweetAlertService.FireAsync("ERROR", message, SweetAlertIcon.Error);
				return;
            }

			Return();

			// Mensaje emergente -> 3 segundos
			var toast = sweetAlertService.Mixin(new SweetAlertOptions
			{
				Toast = true,
				Position = SweetAlertPosition.BottomEnd,
				ShowConfirmButton = true,
				Timer = 5000
			});
			await toast.FireAsync(icon: SweetAlertIcon.Success, message: "CAMBIOS APLICADOS CON EXITO!");
		}

		private void Return()
		{
			countryForm!.FormPostedSuccessfully = true;
			navigationManager.NavigateTo("/countries");
		}
	}
}
