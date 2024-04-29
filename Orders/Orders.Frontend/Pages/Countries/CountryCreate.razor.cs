﻿using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Shared.Entities;
using System.Reflection.Metadata.Ecma335;

namespace Orders.Frontend.Pages.Countries
{
	public partial class CountryCreate
	{
		private Country country = new();
		private CountryForm? countryForm;
		[Inject] private IRepository Repository { get; set; } = null!;
		[Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
		[Inject] private NavigationManager NavigationManager { get; set; } = null!;

		private async Task CreateAsync()
		{
			var responseHttp = await Repository.PostAsync("/api/countries", country);
			if (responseHttp.Error)
			{
				var message = await responseHttp.GetErrorMessageAsync();
				await SweetAlertService.FireAsync("ERROR", message);
				return;
			}

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