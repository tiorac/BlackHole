namespace BlackHole.Client.Services
{
    public class PageTitleService
    {
        private string _title;
        public string Title 
        { 
            get => _title;
            set
            {
                _title = value;
                OnTitleChanged?.Invoke(value);
            }
        }

        public Action<string> OnTitleChanged { get; set; }
    }
}
