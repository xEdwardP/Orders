﻿using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages.Countries
{
	public partial class CountryForm
	{
		private EditContext editContext = null!;

		[EditorRequired,Parameter] public Country Country { get; set; } = null!;

		[EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; } // Codigo Guardar

		[EditorRequired, Parameter] public EventCallback ReturnAction { get; set; } // Codigo Cancelar

		[Inject] public SweetAlertService SweetAlertService { get; set; } = null!;

        public bool FormPostedSuccessfully { get; set; } // Verificacion de posteo del form

		protected override void OnInitialized()
		{
			editContext = new(Country);
		}

		private async Task OnBeforeInternalNavigationAsync(LocationChangingContext context)
		{
			var formWasEdited = editContext.IsModified();
			if (!formWasEdited || FormPostedSuccessfully)
			{
				return;
			}

			var result = await SweetAlertService.FireAsync(new SweetAlertOptions
			{
				Title = "CONFIRMACION",
				Text = "DESEA ABANDONAR LA PAGINA Y PERDER TODOS LOS CAMBIOS REALIZADOS?",
				Icon = SweetAlertIcon.Question,
				ShowCancelButton = true,
			});

			var confirm = !string.IsNullOrEmpty(result.Value);
			if(confirm)
			{
				return;
			}
			context.PreventNavigation();
		}

    }
}
