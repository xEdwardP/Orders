using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages.Categories
{
	public partial class CategoryCreate
	{
		private Category category = new();
		private CategoryForm? categoryForm;
		[Inject] private IRepository Repository { get; set; } = null!;
		[Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
		[Inject] private NavigationManager NavigationManager { get; set; } = null!;

		private async Task CreateAsync()
		{
			var responseHttp = await Repository.PostAsync("/api/categories", category);
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
			categoryForm!.FormPostedSuccessfully = true;
			NavigationManager.NavigateTo("/categories");
		}
	}
}