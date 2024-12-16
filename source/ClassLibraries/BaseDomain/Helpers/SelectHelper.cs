namespace BaseDomain.Helpers
{
    public class SelectHelper
    {
        public int id { get; set; }
        public bool isSelected { get; set; }
    }

    public class SelectHelperWithTitle : SelectHelper
    {
        public string title { get; set; }
    }
}
