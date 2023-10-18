namespace EdvAutomation
{
    internal class AppSettings
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public TimeSpan DelayAfterLogin { get; set; }
        public TimeSpan DelayAfterSubmit { get; set; }
        public TimeSpan SearchPopupTimeout { get; set;}
    }
}