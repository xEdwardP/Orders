using Microsoft.AspNetCore.Components;

namespace Orders.Frontend.Shared
{
    public partial class FiltersButtonsGeneric
    {
        [Parameter] public string PlaceholderText { get; set; } = null!;
        [Parameter] public EventCallback ApplyFilterAsync { get; set; }
        [Parameter] public EventCallback CleanFilterAsync { get; set; }
        [Parameter, SupplyParameterFromQuery] public string FilterValue { get; set; } = string.Empty;
    }
}