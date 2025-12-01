namespace IntegrationApi.ViewModels
{


    public class UserViewModel
    {
       
        public string? name { get; set; }

    }

    public class CustomerViewModel
    {
        public string? customerID { get; set; }
        public string? name { get; set; }
        public List<AddressViewModel>? addresses { get; set; }
        public List<ContactViewModel>? contacts { get; set; }
    }
    public class AddressViewModel
    {
        public string? addressId { get; set; }
        public string? streetAddress1 { get; set; }
        public string? streetAddress2 { get; set; }
        public string? streetAddress3 { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? zipCode { get; set; }
        public string? country { get; set; }
        public string? description { get; set; }
        public string? county { get; set; }
    }

    public class ContactViewModel
    {
        public string? lastName { get; set; }
        public string? name { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public string? office { get; set; }
    }
    


}
