namespace BlazorApp1.Components
{
    public class CustomNavigationContext
    {
        public CustomNavigationContext(string path, bool isNavigationIntercepted)
        {
            Path = path;
            IsNavigationIntercepted = isNavigationIntercepted;
        }

        public string Path { get; }
        public bool IsNavigationIntercepted { get; }
        private bool _preventDefault;

        public void PreventDefault()
        {
            _preventDefault = true;
        }

        public bool DefaultPrevented { get; set; }
    }
}