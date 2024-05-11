using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Orders.Frontend.Shared
{
    public partial class InputImg
    {
        private string? imageBase64;

        [Parameter] public string Label { get; set; } = "Imagen"; // Image Name
        [Parameter] public string? ImageURL { get; set; }
        [Parameter] public EventCallback<string> ImageSelected { get; set; } // Event with parameter

        private async Task OnChange(InputFileChangeEventArgs e)
        {
            var imagenes = e.GetMultipleFiles();

            foreach (var imagen in imagenes)
            {
                var arrBytes = new byte[imagen.Size]; // Array
                await imagen.OpenReadStream().ReadAsync(arrBytes);
                imageBase64 = Convert.ToBase64String(arrBytes);
                ImageURL = null;
                await ImageSelected.InvokeAsync(imageBase64);
                StateHasChanged();
            }
        }
    }
}