using Microsoft.AspNetCore.Components;

namespace Orders.Frontend.Shared
{
    public partial class Pagination
    {
        private List<PageModel> links = [];
        [Parameter] public int CurrentPage { get; set; } = 1; // Initial Page
        [Parameter] public int TotalPages { get; set; } = 1;
        [Parameter] public int Radio { get; set; } = 10; // Cant max bottons pages
        [Parameter] public EventCallback<int> SelectedPage { get; set; }

        protected override void OnParametersSet()
        {
            links = new List<PageModel>();
            // Previous
            links.Add(new PageModel
            {
                Text = "Anterior",
                Page = CurrentPage - 1,
                Enable = CurrentPage != 1
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

            // Next
            var linkNextEnable = CurrentPage != TotalPages;
            var linkNextPage = CurrentPage != TotalPages ? CurrentPage + 1 : CurrentPage;
            links.Add(new PageModel
            {
                Text = "Siguiente",
                //Page = CurrentPage + 1,
                Page = linkNextPage,
                Enable = CurrentPage != TotalPages
            });
        }

        private async Task InternalSelectedPage(PageModel pageModel)
        {
            if (pageModel.Page == CurrentPage || pageModel.Page == 0)
            {
                return;
            }
            await SelectedPage.InvokeAsync(pageModel.Page);
        }

        private class PageModel
        {
            public string Text { get; set; } = null!;
            public int Page { get; set; }
            public bool Enable { get; set; }
        }
    }
}