using BlackHole.Client.Services;
using Microsoft.AspNetCore.Components;

namespace BlackHole.Client.Components
{
    public class BlackHoleTools : ComponentBase
    {
        [Inject]
        public MainPageService PageTitleService { get; set; }

        private RenderFragment _childContent;

        [Parameter]
        public RenderFragment ChildContent 
        { 
            get => _childContent;
            set
            {
                _childContent = value;
                PageTitleService.ToolsButtons = value;
            }
        }
    }
}
