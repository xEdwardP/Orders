﻿using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Shared.Entities;
using System.Net;

namespace Orders.Frontend.Pages.Categories
{
	public partial class CategoriesIndex
	{
		[Inject] private IRepository Repository { get; set; } = null!;
		[Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
		[Inject] private NavigationManager NavigationManager { get; set; } = null!;
		public List<Category>? Categories { get; set; }

        private int currentPage = 1;
        private int totalPages;

        protected async override Task OnInitializedAsync()
		{
			await LoadAsync();
		}

        private async Task SelectedPageAsync(int page)
        {
            currentPage = page;
            await LoadAsync(page);
        }

        private async Task LoadAsync(int page = 1)
        {
            var ok = await LoadListAsync(page);
            if (ok)
            {
                await LoadPagesAsync();
            }
        }

        private async Task<bool> LoadListAsync(int page)
        {
            var responseHttp = await Repository.GetAsync<List<Category>>($"api/categories?page={page}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return false;
            }
            Categories = responseHttp.Response;
            return true;
        }

        private async Task LoadPagesAsync()
        {
            var responseHttp = await Repository.GetAsync<int>("api/categories/totalPages");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            totalPages = responseHttp.Response;
        }


        private async Task DeleteAsync(Category category)
		{
			var result = await SweetAlertService.FireAsync(new SweetAlertOptions
			{
				Title = "CONFIRMACION",
				Text = $"Esta seguro(a) que desea eliminar de forma permanente la categoría {category.Name}?",
				Icon = SweetAlertIcon.Question,
				ShowCancelButton = true,
			});

			var confirm = string.IsNullOrEmpty(result.Value);
			if (confirm)
			{
				return;
			}

			var responseHttp = await Repository.DeleteAsync<Category>($"api/categories/{category.Id}");
			if (responseHttp.Error)
			{
				if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
				{
					NavigationManager.NavigateTo("/categories");
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
