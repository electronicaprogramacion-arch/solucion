# Contact_Search Usage Guide

## Overview
The Contact_Search component allows searching and selecting contacts associated with a specific customer using the CustomID parameter.

## Features
- **Pagination**: Supports server-side pagination for large contact lists
- **Filtering**: Allows filtering contacts by Name, LastName, Email, PhoneNumber, etc.
- **Modal Support**: Can be used as a modal dialog when `IsModal=true`
- **Customer-specific**: Shows only contacts associated with the specified CustomID

## Usage Examples

### 1. Basic Usage (Standalone Page)
```html
<!-- Navigate to the page directly -->
<a href="/ContactSearch">Search Contacts</a>
```

### 2. Modal Usage with CustomID
```html
@* In your parent component *@
<button @onclick="OpenContactSearch">Select Contact</button>

@code {
    private async Task OpenContactSearch()
    {
        var parameters = new ModalParameters();
        parameters.Add("CustomID", "CUSTOMER123");
        parameters.Add("IsModal", true);
        parameters.Add("SelectOnly", true);
        
        var modal = Modal.Show<Contact_Search>("Select Contact", parameters);
        var result = await modal.Result;
        
        if (!result.Cancelled && result.Data != null)
        {
            var selectedContact = (Contact)result.Data;
            // Handle selected contact
        }
    }
}
```

### 3. Programmatic Navigation with Parameters
```csharp
// In your component code
NavigationManager.NavigateTo($"/ContactSearch?CustomID={customerCustomID}");
```

## Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| CustomID | string | The Customer's CustomID to filter contacts |
| IsModal | bool | Whether the component is used as a modal dialog |
| SelectOnly | bool | Whether the component is used for selection only |

## API Integration

The Contact_Search component integrates with:
- **CustomerRepositoryEF.GetContactsByCustomID()**: Retrieves contacts by customer CustomID
- **BasicsServiceGRPC**: For contact CRUD operations
- **Querys.ContactFilter()**: For filtering contacts

## Database Query
The component queries contacts using the following logic:
1. Find Customer by CustomID
2. Get Customer's Aggregates
3. Filter Contacts belonging to those aggregates
4. Apply pagination and filtering

## Notes
- Contacts are filtered to exclude deleted records (`IsDelete = false`)
- The component supports all standard ResponsiveTable features
- Modal functionality requires proper setup of Blazored.Modal
