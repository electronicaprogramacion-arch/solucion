# Audit Log Module - Phase 1 Prototype

## Overview

This is a Phase 1 prototype implementation of the Audit Log module for CalibrationSaaS. The module provides a user interface for searching and viewing audit log entries that track changes to specific entities in the system.

## Features Implemented

### 1. Search Interface
- **Entity Type Dropdown**: Filter by specific entity types (WorkOrder, WorkOrderDetail, PieceOfEquipment, Customer, User)
- **Date Range Picker**: Filter by date range with "From Date" and "To Date" fields
- **Entity ID Input**: Search for specific record IDs
- **Search and Clear Buttons**: Execute queries and reset filters

### 2. Results Grid
- **Responsive Table**: Displays search results using the existing ResponsiveTable component
- **Columns**: Timestamp, User, Entity Type, Entity ID, Action Type
- **Action Buttons**: "View Previous" and "View Current" buttons for each entry
- **Pagination**: Client-side pagination for large result sets
- **Sorting**: Sortable columns with default sort by timestamp

### 3. JSON Viewer Modal
- **Formatted Display**: JSON data displayed in a readable, formatted view
- **Syntax Highlighting**: Color-coded JSON elements (keys, strings, numbers, booleans, null)
- **Copy to Clipboard**: Copy JSON data to clipboard
- **Download**: Download JSON data as a file
- **Responsive Design**: Modal adapts to different screen sizes

### 4. Mock Data
- **Realistic Data Generation**: Generates sample audit log entries with realistic data
- **Entity-Specific JSON**: Different JSON structures for each entity type
- **Previous/Current States**: Simulates before and after states for updates
- **Filtering Support**: Mock data respects search filters

## File Structure

```
Pages/Settings/
├── Log_Search.razor              # Main UI component
├── Log_SearchBase.cs            # Business logic and data handling
├── JsonViewerModal.razor        # JSON viewer modal component
└── README.md                    # This documentation

wwwroot/css/
└── audit-log.css               # Styling for the audit log module
```

## Navigation

The module is accessible through:
- **Settings** → **Audit** → **Audit Log**
- Direct URL: `/Log`

## Components Used

### Radzen Components
- `RadzenDropDown`: Entity type selection
- `RadzenTextBox`: Entity ID input
- `RadzenDatePicker`: Date range selection
- `RadzenButton`: Search, clear, and action buttons
- `RadzenDialog`: JSON viewer modal

### CalibrationSaaS Components
- `ResponsiveTable`: Results grid
- `HeaderComponent`: Page header
- Standard Bootstrap styling and layout

## Data Models

### AuditLogEntry
```csharp
public class AuditLogEntry
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string UserName { get; set; }
    public string EntityType { get; set; }
    public string EntityId { get; set; }
    public string ActionType { get; set; }
    public string PreviousState { get; set; }
    public string CurrentState { get; set; }
}
```

### EntityTypeOption
```csharp
public class EntityTypeOption
{
    public string Value { get; set; }
    public string Text { get; set; }
}
```

## Styling

The module includes custom CSS (`audit-log.css`) that provides:
- Consistent styling with CalibrationSaaS theme
- Responsive design for mobile devices
- JSON syntax highlighting
- Dark mode support (if enabled)
- Professional appearance for business use

## Future Enhancements (Phase 2)

The following features are planned for Phase 2:

1. **Backend Integration**
   - Entity Framework modifications to capture audit data
   - Database schema for audit log storage
   - gRPC services for data retrieval

2. **Real-time Updates**
   - SignalR integration for live audit log updates
   - Automatic refresh when new entries are added

3. **Advanced Features**
   - Export to Excel/PDF
   - Advanced filtering options
   - Audit log retention policies
   - Performance optimization for large datasets

4. **Security**
   - Role-based access control
   - Audit log integrity verification
   - Sensitive data masking

## Usage Instructions

1. **Navigate to the Audit Log**
   - Go to Settings → Audit → Audit Log

2. **Search for Audit Entries**
   - Select an entity type (optional)
   - Enter an entity ID (optional)
   - Set date range (defaults to last 30 days)
   - Click "Search"

3. **View Results**
   - Results are displayed in a sortable, paginated table
   - Click "Previous" to view the state before changes
   - Click "Current" to view the state after changes

4. **JSON Viewer**
   - JSON data is displayed with syntax highlighting
   - Use "Copy" to copy data to clipboard
   - Use "Download" to save as a JSON file

## Technical Notes

- **Mock Data**: Currently uses generated mock data for demonstration
- **Client-side Filtering**: All filtering is performed client-side
- **Responsive Design**: Works on desktop, tablet, and mobile devices
- **Performance**: Optimized for up to 100 entries in the prototype
- **Browser Compatibility**: Supports modern browsers with ES6+ features

## Dependencies

- Radzen.Blazor (UI components)
- System.Text.Json (JSON serialization)
- Microsoft.AspNetCore.Components (Blazor framework)
- Blazed.Controls (CalibrationSaaS custom components)

## Testing

The prototype includes comprehensive mock data generation that covers:
- All supported entity types
- Various action types (Create, Update, Delete)
- Realistic JSON data structures
- Date range filtering
- Entity ID filtering
- Entity type filtering

To test the module:
1. Navigate to the Audit Log page
2. Try different search combinations
3. View JSON data in the modal
4. Test responsive design on different screen sizes
5. Verify all buttons and interactions work correctly
