namespace IntegrationApi.ViewModels
{
    public class WorkOrderViewModel
    {
        public string? workOrderId { get; set; }
        public string? customerId { get; set; }
        public string? workOrderDate { get; set; }
        public string? addressId { get; set; }
        public string? contactId { get; set; }
        public List<TechnicianViewModel>? technicians { get; set; }
    }

    public class TechnicianViewModel
    {
        public string? name { get; set; }
    }
}


