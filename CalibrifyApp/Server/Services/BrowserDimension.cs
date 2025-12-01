namespace CalibrifyApp.Server.Services
{
    public class BrowserDimension
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Online { get; set; }
        public int Scroll { get; set; } = 0;
        public bool Install { get; set; }
    }
}
