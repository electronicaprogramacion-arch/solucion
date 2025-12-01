# Address_Search Component Usage

This document explains how to use the Address_Search component as a modal dialog with proper pagination and styling.

## Modal Configuration

To display the Address_Search component as a modal with proper pagination, use the following configuration:

### Example Usage

```csharp
public async Task ShowModalAddress()
{
    var parameters = new ModalParameters();
    parameters.Add("SelectOnly", true);
    parameters.Add("IsModal", true);
    
    ModalOptions op = new ModalOptions();
    op.ContentScrollable = true;
    op.Class = "blazored-modal " + ModalSize.MediumWindow;
    
    var messageForm = Modal.Show<Address_Search>("Select Address", parameters, op);
    var result = await messageForm.Result;
    
    if (!result.Cancelled && result.Data != null)
    {
        var selectedAddress = (Address)result.Data;
        // Handle selected address
    }
}
```

### Key Configuration Points

1. **Modal Size**: Use `ModalSize.MediumWindow` (which equals "halfWindow")
2. **Content Scrollable**: Set `op.ContentScrollable = true` to enable proper scrolling
3. **CSS Class**: Use `"blazored-modal " + ModalSize.MediumWindow` for proper styling
4. **Parameters**: 
   - `SelectOnly = true` for selection mode
   - `IsModal = true` to enable modal behavior

### SearchButtonComponent Usage

You can also use the SearchButtonComponent to show the Address_Search modal:

```html
<SearchButtonComponent FormType="typeof(Address_Search)"
                       Size="@ModalSize.MediumWindow"
                       Text="Select Address"
                       Enabled="true"
                       Parameters="@addressParameters"
                       CloseWindow="OnAddressSelected">
</SearchButtonComponent>
```

```csharp
@code {
    private ModalParameters addressParameters;
    
    protected override void OnInitialized()
    {
        addressParameters = new ModalParameters();
        addressParameters.Add("SelectOnly", true);
        addressParameters.Add("IsModal", true);
    }
    
    private async Task OnAddressSelected(ChangeEventArgs args)
    {
        var modalResult = (ModalResult)args.Value;
        if (!modalResult.Cancelled && modalResult.Data != null)
        {
            var selectedAddress = (Address)modalResult.Data;
            // Handle selected address
        }
    }
}
```

## Important Notes

- The modal will show pagination at the bottom when configured correctly
- The search functionality is built-in and will filter addresses by street address, city, state, zip code, and country
- The component supports both modal and non-modal usage
- Make sure to set both `SelectOnly` and `IsModal` parameters for proper modal behavior
