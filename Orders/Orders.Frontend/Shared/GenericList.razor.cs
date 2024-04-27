using Microsoft.AspNetCore.Components;

namespace Orders.Frontend.Shared
{
	public partial class GenericList<Titem>
	{
		// Cuando se estan cargando los registros
		[Parameter] public RenderFragment? Loading {  get; set; }
		// Cuando no existan registros
		[Parameter] public RenderFragment? NoRecords { get; set; }
		[EditorRequired, Parameter] public RenderFragment Body { get; set; } = null!;
		[EditorRequired, Parameter] public List<Titem> MyList { get; set; } = null!;
	}
}
