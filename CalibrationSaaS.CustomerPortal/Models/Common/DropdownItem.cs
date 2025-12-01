namespace CalibrationSaaS.CustomerPortal.Models.Common;

public class DropdownItem<T>
{
    public T Value { get; set; }
    public string Text { get; set; }

    public DropdownItem(T value, string text)
    {
        Value = value;
        Text = text;
    }
}
