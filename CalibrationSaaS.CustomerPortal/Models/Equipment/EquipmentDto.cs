namespace CalibrationSaaS.CustomerPortal.Models.Equipment;

public class EquipmentDto
{
    public int Id { get; set; }
    public string SerialNumber { get; set; } = "";
    public string Description { get; set; } = "";
    public string Manufacturer { get; set; } = "";
    public string Model { get; set; } = "";
    public string? Location { get; set; }
    public DateTime? LastCalibrationDate { get; set; }
    public DateTime? NextCalibrationDate { get; set; }
    public int CalibrationInterval { get; set; }
    public string? CalibrationStatus { get; set; }
}

public class EquipmentSearchRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SerialNumber { get; set; }
    public string? Description { get; set; }
    public int? ManufacturerId { get; set; }
    public int? EquipmentTypeId { get; set; }
    public string? CalibrationStatus { get; set; }
    public string? Location { get; set; }
    public DateTime? DueDateFrom { get; set; }
    public DateTime? DueDateTo { get; set; }
    public string SortBy { get; set; } = "SerialNumber";
    public string SortDirection { get; set; } = "asc";
}

public class EquipmentSearchResult
{
    public List<EquipmentDto> Equipment { get; set; } = new();
    public int TotalCount { get; set; }
}

public class ManufacturerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class EquipmentTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}
