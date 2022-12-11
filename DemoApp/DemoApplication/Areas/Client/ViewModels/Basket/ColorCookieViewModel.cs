namespace DemoApplication.Areas.Client.ViewModels.Basket
{
    public class ColorCookieViewModel
    {
        public ColorCookieViewModel(int id, string? name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string? Name { get; set; }

    }
}
