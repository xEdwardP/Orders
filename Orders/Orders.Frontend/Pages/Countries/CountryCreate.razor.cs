using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Frontend.Shared;
using Orders.Shared.Entities;
using System.Reflection.Metadata.Ecma335;

namespace Orders.Frontend.Pages.Countries
{
    [Authorize(Roles = "Admin")]
    public partial class CountryCreate
	{
		private Country country = new();
		private FormWithName<Country>? countryForm;
		[Inject] private IRepository Repository { get; set; } = null!;
		[Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
		[Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;

        private async Task CreateAsync()
		{
			var responseHttp = await Repository.PostAsync("/api/countries", country);
			if (responseHttp.Error)
			{
				var message = await responseHttp.GetErrorMessageAsync();
				await SweetAlertService.FireAsync("ERROR", message);
				return;
			}

            await BlazoredModal.CloseAsync(ModalResult.Ok());
            Return();

			// Mensaje emergente -> 3 segundos
			var toast = SweetAlertService.Mixin(new SweetAlertOptions
			{
				Toast = true,
				Position = SweetAlertPosition.BottomEnd,
				ShowConfirmButton = true,
				Timer = 5000
			});
			await toast.FireAsync(icon: SweetAlertIcon.Success, message: "REGISTRO CREADO CON EXITO!");
		}

		private void Return()
		{
			countryForm!.FormPostedSuccessfully = true;
			NavigationManager.NavigateTo("/countries");
		}
    }
}
