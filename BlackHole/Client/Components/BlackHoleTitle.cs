using BlackHole.Client.Services;
using Microsoft.AspNetCore.Components;

namespace BlackHole.Client.Components
{
    public class BlackHoleTitle : ComponentBase
    {
        [Parameter]
        public string Title { get; set; }

        [Inject]
        public PageTitleService PageTitleService { get; set; }

        protected override void OnInitialized()
        {
            PageTitleService.Title = Title;
        }
    }
}
