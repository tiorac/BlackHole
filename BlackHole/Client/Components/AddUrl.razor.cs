using Microsoft.AspNetCore.Components;
using BlackHole.Client.Services;
using Microsoft.AspNetCore.Components.Forms;

namespace BlackHole.Client.Components
{
    public partial class AddUrl
    {
        [Inject]
        public ClipboardService Clipboard { get; set; }

        [Parameter]
        public string Url { get; set; }

        [Parameter]
        public EventCallback<string> UrlChanged { get; set; }

        [Parameter]
        public Action<string> UrlAdded { get; set; }

        [Parameter]
        public Func<string, bool> CanAddUrl { get; set; }

        private InputText _inputText { get; set; }

        private bool _showField { get; set; }

        private bool _validateHideField { get; set; }

        private async void ShowField()
        {
            try
            {
                Url = string.Empty;
                string text = await Clipboard.ReadTextAsync();

                if (!string.IsNullOrWhiteSpace(text) 
                    && IsUrl(text)
                    && (CanAddUrl?.Invoke(text) ?? true))
                {
                    Url = text;
                    FinalizeAddUrl();
                }
                else
                {
                    _showField = true;
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Clipboard error: {ex}");
            }
        }

        private bool IsUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        private async void FinalizeAddUrl()
        {
            _validateHideField = false;

            if (IsUrl(Url)
                && (CanAddUrl?.Invoke(Url) ?? true))
            {
                await UrlChanged.InvokeAsync(Url);
                UrlAdded?.Invoke(Url);
                Url = string.Empty;
                _showField = false;
            }
            else
            {
                await _inputText.Element.Value.FocusAsync();
            }
        }

        private void CancelAddUrl()
        {
            _validateHideField = true;

            Task.Delay(200).ContinueWith(_ =>
            {
                if (_validateHideField)
                {
                    _showField = false;
                    
                }

                _validateHideField = false;
                StateHasChanged();
            });
        }
    }
}
