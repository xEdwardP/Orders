using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Shared.DTOs;

namespace Orders.Frontend.Pages.Auth
{
    public partial class ChangePassword
    {
        private ChangePasswordDTO changePasswordDTO { get; set; } = new();

        private bool loading;

        [Inject] NavigationManager NavigationManager { get; set; } = null!;
        [Inject] SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] IRepository Repository { get; set; } = null!;

        private async Task ChangePasswordAsync()
        {
            loading = true;
            var responseHttp = await Repository.PostAsync("/api/accounts/changePassword", changePasswordDTO);
            loading = false;
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("ERROR!", message, SweetAlertIcon.Error);
                //loading = false;
                return;
            }
            //loading = false;
            NavigationManager.NavigateTo("/EditUser");

            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });

            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Contraseña cambiada con éxito!");
        }
    }
}