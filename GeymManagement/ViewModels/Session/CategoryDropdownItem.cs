namespace GymManagement.Presentation.ViewModels.Session;

public record CategoryDropdownItem
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
}
