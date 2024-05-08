using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Frontend.Shared;
using Orders.Shared.Entities;
using System.Net;

namespace Orders.Frontend.Pages.Categories
{
    [Authorize(Roles = "Admin")]
    public partial class CategoryEdit
	{
		private Category? category;
		private FormWithName<Category>? categoryForm;
		[Inject] private IRepository Repository { get; set; } = null!;
		[Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
		[Inject] private NavigationManager NavigationManager { get; set; } = null!;

		[EditorRequired, Parameter] public int Id { get; set; }

		protected override async Task OnParametersSetAsync()
		{
			var responseHttp = await Repository.GetAsync<Category>($"/api/categories/{Id}");
			if (responseHttp.Error)
			{
				if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
				{
					NavigationManager.NavigateTo("/categories");
				}
				else
				{
					var message = await responseHttp.GetErrorMessageAsync();
					await SweetAlertService.FireAsync("ERROR", message, SweetAlertIcon.Error);
				}
			}
			else
			{
				category = responseHttp.Response;
			}
		}

		private async Task EditAsync()
		{
			var responseHttp = await Repository.PutAsync("/api/categories", category);
			if (responseHttp.Error)
			{
				var message = await responseHttp.GetErrorMessageAsync();
				await SweetAlertService.FireAsync("ERROR", message, SweetAlertIcon.Error);
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
			await toast.FireAsync(icon: SweetAlertIcon.Success, message: "CAMBIOS APLICADOS CON EXITO!");
		}

		private void Return()
		{
			categoryForm!.FormPostedSuccessfully = true;
			NavigationManager.NavigateTo("/categories");
		}
	}
}