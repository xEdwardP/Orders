using Microsoft.AspNetCore.Components;

namespace Orders.Frontend.Shared
{
    public partial class Pagination
    {
        private List<PageModel> links = [];
        private List<OptionModel> options = [];
        private int selectedOptionValue = 10;

        [Parameter] public int CurrentPage { get; set; } = 1; // Initial Page
        [Parameter] public int TotalPages { get; set; } = 1;
        [Parameter] public int Radio { get; set; } = 10; // Cant max bottons pages
        [Parameter] public EventCallback<int> SelectedPage { get; set; }
        [Parameter] public EventCallback<int> RecordsNumber { get; set; }

        protected override void OnParametersSet()
        {
            BuildPages();
            BuildOptions();
        }

        private void BuildPages()
        {
            links = [];
            var previousLinkEnable = CurrentPage != 1;
            var previousLinkPage = CurrentPage - 1;

            links.Add(new PageModel
            {
                Text = "Anterior",
                Page = previousLinkPage,
                Enable = previousLinkEnable
            });

            for (int i = 1; i <= TotalPages; i++)
            {
                var conditional1 = TotalPages <= Radio;
                var conditional2 = TotalPages > Radio && i <= Radio && CurrentPage <= Radio;
                var conditional3 = CurrentPage > Radio && i > CurrentPage - Radio && i <= CurrentPage;

                if (conditional1 || conditional2 || conditional3)
                {
                    links.Add(new PageModel
                    {
                        Text = $"{i}",
                        Page = i,
                        Enable = i == CurrentPage
                    });
                }
            }

            var linkNextEnable = CurrentPage != TotalPages;
            var linkNextPage = CurrentPage != TotalPages ? CurrentPage + 1 : CurrentPage;
            links.Add(new PageModel
            {
                Text = "Siguiente",
                Page = linkNextPage,
                Enable = linkNextEnable
            });
        }

        private void BuildOptions()
        {
            options =
                [
                new OptionModel { Value = 10, Name = "10" },
                new OptionModel { Value = 25, Name = "25" },
                new OptionModel { Value = 50, Name = "50" },
                new OptionModel { Value = int.MaxValue, Name = "Todos" },
                ];
        }

        private async Task InternalRecordsNumberSelected(ChangeEventArgs e)
        {
            if (e.Value != null)
            {
                selectedOptionValue = Convert.ToInt32(e.Value.ToString());
            }
            await RecordsNumber.InvokeAsync(selectedOptionValue);
        }

        private async Task InternalSelectedPage(PageModel pageModel)
        {
            if (pageModel.Page == CurrentPage || pageModel.Page == 0)
            {
                return;
            }
            await SelectedPage.InvokeAsync(pageModel.Page);
        }

        private class OptionModel
        {
            public string Name { get; set; } = null!;
            public int Value { get; set; }
        }

        private class PageModel
        {
            public string Text { get; set; } = null!;
            public int Page { get; set; }
            public bool Enable { get; set; }
        }
    }
}