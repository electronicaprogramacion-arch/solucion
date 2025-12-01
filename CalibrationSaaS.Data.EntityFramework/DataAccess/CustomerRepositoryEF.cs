using CalibrationSaaS.Data.EntityFramework;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Infraestructure.EntityFramework.Helpers;
using CalibrationSaaS.Models.ViewModels;
using Helpers;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.EntityFramework.DataAccess
{
    public class CustomerRepositoryEF<TContext> : ICustomerRepository, IDisposable where TContext : DbContext, ICalibrationSaaSDBContextBase
    {
        private readonly IDbContextFactory<TContext> DbFactory;


        public CustomerRepositoryEF(IDbContextFactory<TContext> dbFactory)
        {
            DbFactory = dbFactory;
        }

        public async Task<Customer> DeleteCustomer(Customer customerDTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            //customerDTO.IsDelete = true;
            context.Entry(customerDTO).State = EntityState.Deleted;
                
                await context.SaveChangesAsync();
                return customerDTO;
            
           
        }

        public async Task<Address> DeleteAddress(Address DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            //DTO.Delete = true;
            context.Entry(DTO).State = EntityState.Deleted;
            //context.Update(DTO);
            await context.SaveChangesAsync();
            return DTO;

        }

        public async Task<Contact> DeleteContact(Contact DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            //DTO.Delete = true;
            context.Entry(DTO).State = EntityState.Deleted;
            //context.Update(DTO);
            await context.SaveChangesAsync();
            return DTO;

        }


        public async Task<PhoneNumber> DeletePhone(PhoneNumber   DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            try
            {
                context.Entry(DTO).State = EntityState.Deleted;
                await context.SaveChangesAsync();
                return DTO;
            }
            catch (Exception ex)
            {
                return null;
            
            }



        }

        public async Task<Social> DeleteSocial(Social DTO)
        {
await using var context = await DbFactory.CreateDbContextAsync();
            context.Entry(DTO).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            return DTO;


        }


        public async Task<string> ValidateCustomer(Customer customerDTO)
        {
await using var context = await DbFactory.CreateDbContextAsync();
            string result = "";

            Customer customer = await context.Customer
                .FirstOrDefaultAsync(sc => 
                sc.Name.ToLower()== customerDTO.Name.ToLower()
                
                );
            
            if(customer != null)
            {
                return "Name already exists";
            }


          
            foreach (var item in customerDTO.Aggregates)
            {
                if(item.Contacts != null)
                {
                    foreach (var item2 in item.Contacts)
                    {

                        Contact Contact = await context.Contact
                       .FirstOrDefaultAsync(sc =>
                        item2.Email != null && item2.IsDelete == false && (sc.Email.ToLower() == item2.Email.ToLower())
                        && (item2.UserName != null && sc.UserName == item2.UserName));

                        if (Contact != null)
                        {
                            return "Contact already exists";
                        }
                    }
                }
               
            }

           
            return result;
        }



        public async Task<Customer> GetCustomerByID(Customer id)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            
                var address = new List<Address>();
                var contacts = new List<Contact>();

                var phone = new List<Address>();
                var result = await context.Customer
                    .Include(x => x.Aggregates)
                    .ThenInclude(d => d.Addresses)
                    .Include(x => x.Aggregates)
                    .ThenInclude(d => d.Contacts)
                    .Include(x => x.Aggregates)
                    .ThenInclude(d => d.PhoneNumbers)
                    .Include(x => x.Aggregates)
                    .ThenInclude(d => d.Socials)

                    .Where(x => x.CustomerID == id.CustomerID).AsNoTracking().FirstOrDefaultAsync();



                
                

                return result;
            
            
            
        }





        //public async Task<Customer> GetCustomerByName(string id)
        //{
        //    var result = await context.Customer
        //        //.Include(x => x.Aggregates)
        //        //.ThenInclude(d => d.Addresses)
        //        //.Include(x => x.Aggregates)
        //        //.ThenInclude(d => d.Contacts)
        //        //.Include(x => x.Aggregates)
        //        //.ThenInclude(d => d.PhoneNumbers)
        //        .Where(x => x.Name.ToLower() == id.ToLower()).AsNoTracking().FirstOrDefaultAsync();
        //    return result;
        //}

        public async Task<ResultSet<Customer>> GetCustomers(Pagination<Customer> pagination)
        {
   await using var context = await DbFactory.CreateDbContextAsync();
            var filterQuery = Querys.CustomerFilter(pagination.Filter);
            var queriable = context.Customer;//context.PieceOfEquipment.Include(x => x.EquipmentTemplate).Include(d => d.Customer).AsQueryable();
            var simplequery = context.Customer;

            //if (pagination.Filter == null)
            //{
            //    pagination.Filter = "Delete";
            //}
            //var result = await QueryableExtensions.PaginationAndFilterQuery<PieceOfEquipment>(exprTree, pagination, context.PieceOfEquipment, queriable);
            var result = await queriable.PaginationAndFilterQuery<Customer>( pagination, simplequery, filterQuery);
           

            return result;

        }

        public async Task<Customer> InsertCustomer(Customer newCustomer,bool IsApi=false)
        {
                await using var context = await DbFactory.CreateDbContextAsync();
            
                var res = await GetCustomerByID(newCustomer);
                if (res != null)
                {
                    var x = await UpdateCustomer(newCustomer, IsApi);
                await context.SaveChangesAsync();
                }
                else
                {

                    Customer c = new Customer();

                    c =(Customer) newCustomer.CloneObject();

                    c.Aggregates = null;// new List<CustomerAggregate>();

                    c.CustomerID = NumericExtensions.GetUniqueID(newCustomer.CustomerID);
                    context.Customer.Add(c);
                    await context.SaveChangesAsync();

                    newCustomer.CustomerID = c.CustomerID;
                
                    newCustomer.CustomerID = c.CustomerID;

                foreach (var ca2 in newCustomer.Aggregates)
                {
                    CustomerAggregate ca = new CustomerAggregate();
                    
                    ca.CustomerID = c.CustomerID;
                    ca.AggregateID = NumericExtensions.GetUniqueID(ca2.AggregateID);
                    context.CustomerAggregates.Add(ca);
                    await context.SaveChangesAsync();
                    ca2.CustomerID = c.CustomerID;
                    ca2.AggregateID = ca.AggregateID;


                }

                var x = await UpdateCustomer(newCustomer,IsApi);
                
            }

            var res2=await GetCustomerByID(newCustomer);

            return res2;
           
        }

        public async Task<Customer> UpdateCustomer(Customer newCustomer, bool IsApi = false)
        {

            await using var context = await DbFactory.CreateDbContextAsync();
            await RemoveAddress(newCustomer, context, IsApi);
          
            await RemoveContacts(newCustomer, context, IsApi);

            await RemovePhoneNumber(newCustomer, context, IsApi);

            await RemoveSocial(newCustomer, context, IsApi);

            if (newCustomer?.Aggregates?.ElementAtOrDefault(0) != null)
            {
                newCustomer.Aggregates.ElementAtOrDefault(0).Addresses = null;
                newCustomer.Aggregates.ElementAtOrDefault(0).Contacts = null;
                newCustomer.Aggregates.ElementAtOrDefault(0).PhoneNumbers = null;
                newCustomer.Aggregates.ElementAtOrDefault(0).Socials = null;
                newCustomer.Aggregates = null;
            }


            context.Customer.Update(newCustomer);
            //context.Entry(a).State= EntityState.Modified;
            await context.SaveChangesAsync();
            return newCustomer;
        }

        public async Task RemoveAddress(Customer newCustomer,TContext context, bool IsApi = false)
        {


            //await using var context = await DbFactory.CreateDbContextAsync();

            var a = await context.CustomerAggregates.AsNoTracking().Include(x => x.Addresses).Where(x => x.CustomerID == newCustomer.CustomerID).ToListAsync();

            var b = newCustomer.Aggregates; // await context.CustomerAggregates.Include(x => x.Addresses).Where(x => x.CustomerID == newCustomer.CustomerID).ToListAsync();


            if (a.Count > 0 && !IsApi)
            {       

                var agid = a.ElementAtOrDefault(0).AggregateID;

                foreach (var item in a.Where(x => x.Addresses != null))
                {

                    foreach (var item2 in item?.Addresses)
                    {

                        CustomerAggregate match = null;
                        if (b != null && b?.Count>0  )
                        {
                            b.Where(x => x.Addresses != null && x.Addresses.Count > 0 && x.Addresses.Where(x => x.AddressId == item2.AddressId).FirstOrDefault() != null).FirstOrDefault();
                        }
                       

                        if (match == null)
                        {
                            item2.IsDelete = true;

                            context.Address.Update(item2);
                            //context.Address.Remove(item2);

                            await context.SaveChangesAsync();
                        }
                      
                    }

                                      
                }


            }

            if(b.Count > 0)
            {
                int aggid = 0;

                 var a1 = await context.CustomerAggregates.AsNoTracking().Where(x => x.CustomerID == newCustomer.CustomerID).FirstOrDefaultAsync();

                if(a1!= null)
                {
                    aggid = a1.AggregateID;
                }
                else
                {
                    NumericExtensions.GetUniqueID(b.FirstOrDefault().AggregateID);
                }
                 



                foreach (var adds in b.Where(x => x.Addresses != null))
                {

                    adds.AggregateID = aggid;
                    adds.CustomerID = newCustomer.CustomerID;

                    var ca = a.Where(x => x.AggregateID == aggid).FirstOrDefault();

                    if (ca == null)
                    {
                        var cloneadds =(CustomerAggregate) adds.CloneObject();

                        cloneadds.Addresses = null;
                        cloneadds.Contacts = null;
                        cloneadds.PhoneNumbers = null;
                        cloneadds.Socials = null;                    

                        context.CustomerAggregates.Add(cloneadds);
                        context.SaveChanges();
                    }

                    foreach (var item2 in adds?.Addresses)
                    {

                        var ity2 =a.Where(x=> x.Addresses.Where(x => x.AddressId !=item2.AddressId && !string.IsNullOrEmpty(x.StreetAddress1) && !string.IsNullOrEmpty(item2.StreetAddress1)
                        && (x.StreetAddress1.ToLower() == item2.StreetAddress1.ToLower())
                        ).FirstOrDefault()!=null).FirstOrDefault();

                        if (ity2 != null && ity2.CustomerID != adds.CustomerID)
                        {
                            throw new Exception("Street Address already exists!!! " + item2.AddressId  + " ----  " + item2.StreetAddress1);
                        }

                        var ity = a.Where(x => x.Addresses.Where(x => x.AddressId == item2.AddressId).FirstOrDefault() != null).FirstOrDefault();


                        //var ityyy = a.Where(x => x.Addresses.Where(x => x.AddressId == item2.AddressId).FirstOrDefault() != null).ToList();
                        var iteyr = await context.Address.AsNoTracking().Where(x => x.AddressId == item2.AddressId).FirstOrDefaultAsync();


                        var iteyr1 = await context.Address.AsNoTracking().Where(x => !string.IsNullOrEmpty(x.StreetAddress1) && x.StreetAddress1.ToLower() 
                        == item2.StreetAddress1.ToLower() && x.CustomerAggregateAggregateID== aggid).FirstOrDefaultAsync();

                        if (ity == null && iteyr==null && iteyr1==null)
                        {
                            item2.AddressId = NumericExtensions.GetUniqueID(item2.AddressId);
                            item2.AggregateID = aggid;
                            item2.CustomerAggregateAggregateID = aggid;
                            context.Address.Add(item2);
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            try
                            {
                                var local2 = context.Set<Address>()
                                .Local
                                .FirstOrDefault(entry => entry.AddressId.Equals(item2.AddressId));

                                if (local2 != null || iteyr1 != null)
                                {
                                    // detach
                                    if(local2 != null)
                                    {
                                        context.Entry(local2).State = EntityState.Detached;
                                    }
                                   

                                    item2.AddressId = NumericExtensions.GetUniqueID(item2.AddressId);
                                    item2.AggregateID = aggid;
                                    item2.CustomerAggregateAggregateID = aggid;
                                    context.Address.Update(item2);
                                    await context.SaveChangesAsync();
                                }
                                else
                                {
                                    item2.AddressId = NumericExtensions.GetUniqueID(item2.AddressId);
                                    item2.AggregateID = aggid;
                                    item2.CustomerAggregateAggregateID = aggid;
                                    context.Address.Add(item2);
                                    await context.SaveChangesAsync();
                                }
                              
                            }
                            catch (Exception ex)
                            {

                            }
                        
                        }
                    }

                }

                
            }


        }

        public async Task RemoveContacts(Customer newCustomer,TContext context, bool IsApi = false)
        {


            //await using var context = await DbFactory.CreateDbContextAsync();

            var a = await context.CustomerAggregates.AsNoTracking().Include(x => x.Contacts).Where(x => x.CustomerID == newCustomer.CustomerID).ToListAsync();

            var b = newCustomer.Aggregates; // await context.CustomerAggregates.Include(x => x.Addresses).Where(x => x.CustomerID == newCustomer.CustomerID).ToListAsync();

            if (a.Count > 0 && !IsApi)
            {


                var agid = a.ElementAtOrDefault(0).AggregateID;

                foreach (var item in a.Where(x => x.Contacts != null))
                {

                    foreach (var item2 in item?.Contacts)
                    {

                        var match = b.Where(x => x.Contacts.Where(x => x.ContactID == item2.ContactID).FirstOrDefault() != null).FirstOrDefault();

                        if (match == null)
                        {
                            item2.IsDelete = true;

                            context.Contact.Update(item2);
                            //context.Contact.Remove(item2);

                            await context.SaveChangesAsync();
                        }
                        //else
                        //{
                        //    context.Address.Update(item2);

                        //    await context.SaveChangesAsync();
                        //}
                    }


                }


            }

            if (b.Count > 0)
            {


                int aggid = 0;

                var a1 = await context.CustomerAggregates.AsNoTracking().Where(x => x.CustomerID == newCustomer.CustomerID).FirstOrDefaultAsync();

                if (a1 != null)
                {
                    aggid = a1.AggregateID;
                }
                else
                {
                    NumericExtensions.GetUniqueID(b.FirstOrDefault().AggregateID);
                }


                foreach (var adds in b.Where(x => x.Contacts != null))
                {
                    foreach (var item2 in adds?.Contacts)
                    {
                        try
                        {
                            var ity2 = a.Where(x => x.Contacts.Where(x => x.ContactID != item2.ContactID && !string.IsNullOrEmpty(x.PhoneNumber)
                    && !string.IsNullOrEmpty(item2.PhoneNumber)
                    && (x.PhoneNumber.ToLower() == item2.PhoneNumber.ToLower())
                    ).FirstOrDefault() != null).FirstOrDefault();

                            if (ity2 != null)
                            {
                                //throw new Exception("Contact Phonenumber already exists!!!");
                            }

                            CustomerAggregate ity = null;
                            Contact iteyr1 = null;
                            if (ity2 != null)
                            {
                                ity = a.Where(x => x.Contacts.Where(x => x.ContactID == item2.ContactID).FirstOrDefault() != null).FirstOrDefault();

                                iteyr1 = await context.Contact.AsNoTracking().Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToLower()
                              == item2.Name.ToLower() && x.CustomerAggregateAggregateID == aggid).FirstOrDefaultAsync();
                            }

                            var conta = await context.Contact.AsNoTracking().Where(x => x.ContactID == item2.ContactID).FirstOrDefaultAsync();

                            if (ity == null && iteyr1 == null && ity2 == null && conta == null)
                            {
                                item2.ContactID = NumericExtensions.GetUniqueID(item2.ContactID);
                                item2.AggregateID = aggid;
                                item2.CustomerAggregateAggregateID = aggid;
                                context.Contact.Add(item2);
                                await context.SaveChangesAsync();
                            }
                            else
                            {


                                var local2 = context.Set<Contact>()
                                   .Local
                                   .FirstOrDefault(entry => entry.ContactID.Equals(item2.ContactID));

                                if (local2 != null)
                                {
                                    context.Entry(local2).State = EntityState.Detached;
                                }
                                try
                                {
                                    if (iteyr1 != null)
                                    {
                                        item2.ContactID = NumericExtensions.GetUniqueID(iteyr1.ContactID);
                                    }
                                    else
                                    {
                                        item2.ContactID = NumericExtensions.GetUniqueID(item2.ContactID);
                                    }

                                    item2.AggregateID = aggid;
                                    item2.CustomerAggregateAggregateID = aggid;
                                    context.Contact.Update(item2);
                                    await context.SaveChangesAsync();
                                }
                                catch (Exception ex)

                                {
//                                    Console.WriteLine("-------------contacts error--------------------------");
//                                    Console.WriteLine(ex.Message);

                                    if (ex.InnerException != null)
                                    {
//                                        Console.WriteLine(ex.InnerException.Message);
//                                        Console.WriteLine(ex.StackTrace);
                                    }


                                }

                            }
                        }
                        catch (Exception ex)
                        {
//                            Console.WriteLine("Error en contact ID" + item2.ContactID + " CAggregate " + item2.CustomerAggregateAggregateID);
                        }
                    
                    }
                }


            }


        }

        public async Task RemovePhoneNumber(Customer newCustomer,TContext context, bool IsApi = false)
        {


            

            var a = await context.CustomerAggregates.AsNoTracking().Include(x => x.PhoneNumbers).Where(x => x.CustomerID == newCustomer.CustomerID).ToListAsync();

            var b = newCustomer.Aggregates; // await context.CustomerAggregates.Include(x => x.Addresses).Where(x => x.CustomerID == newCustomer.CustomerID).ToListAsync();

            if (a.Count > 0)
            {


                var agid = a.ElementAtOrDefault(0).AggregateID;

                foreach (var item in a.Where(x => x.PhoneNumbers != null))
                {

                    foreach (var item2 in item?.PhoneNumbers)
                    {

                        CustomerAggregate match = null;

                        if(b != null && b.Count > 0)
                        {
                            match = b.Where(x => x.PhoneNumbers != null && x.PhoneNumbers.Where(x => x.PhoneNumberID == item2.PhoneNumberID).FirstOrDefault() != null).FirstOrDefault();
                        }
                        

                        if (match == null)
                        {
                            //item2.IsDelete = true;

                            //context.Address.Update(item2);
                            context.PhoneNumber.Remove(item2);

                            await context.SaveChangesAsync();
                        }
                        //else
                        //{
                        //    context.Address.Update(item2);

                        //    await context.SaveChangesAsync();
                        //}
                    }


                }


            }

            if (b.Count > 0)
            {


                var aggid = b.FirstOrDefault().AggregateID;


                foreach (var adds in b.Where(x=>x.PhoneNumbers != null))
                {

                    foreach (var item2 in adds?.PhoneNumbers)
                    {

                        var ity2 = a.Where(x => x.PhoneNumbers.Where(x => x.PhoneNumberID != item2.PhoneNumberID && !string.IsNullOrEmpty(x.Number)
                        && !string.IsNullOrEmpty(item2.Number)
                        && (x.Number.ToLower() == item2.Number.ToLower())
                        ).FirstOrDefault() != null).FirstOrDefault();

                        if (ity2 != null)
                        {
                            throw new Exception("Phonenumber already exists!!!");
                        }

                        var ity = a.Where(x => x.PhoneNumbers.Where(x => x.PhoneNumberID == item2.PhoneNumberID).FirstOrDefault() != null).FirstOrDefault();



                        if (ity == null)
                        {
                            item2.PhoneNumberID = NumericExtensions.GetUniqueID(item2.PhoneNumberID);
                            item2.CustomerAggregateAggregateID = aggid;                            
                            context.PhoneNumber.Add(item2);
                            await context.SaveChangesAsync();
                        }
                        else
                        {

                            item2.PhoneNumberID = NumericExtensions.GetUniqueID(item2.PhoneNumberID);                            
                            item2.CustomerAggregateAggregateID = aggid;
                            context.PhoneNumber.Update(item2);
                            await context.SaveChangesAsync();
                        }
                    }

                }


            }


        }

        //public async Task RemoveContacts(Customer newCustomer)
        //{
        //    await using var context = await DbFactory.CreateDbContextAsync();
        //    var a = await context.CustomerAggregates.Include(x => x.Contacts).Where(x => x.CustomerID == newCustomer.CustomerID).ToListAsync();

        //    if (a.Count > 0)
        //    {
        //        var adds = new List<Contact>();
        //        var adds1 = new List<Contact>();
        //        adds1 = a.ElementAtOrDefault(0).Contacts.ToList();
        //        if (newCustomer?.Aggregates?.ElementAtOrDefault(0)?.Contacts?.Count > 0)
        //        {
        //            adds = newCustomer.Aggregates.ElementAtOrDefault(0).Contacts.ToList();

        //        }


        //            foreach (var item in a)
        //        {

        //            //foreach (var item2 in item.Contacts)
        //            foreach (var item2 in item.Contacts)
        //            {

        //                var match = adds.Where(x => x.ContactID == item2.ContactID).FirstOrDefault();

        //                if (match == null)
        //                {
        //                    item2.IsDelete = true;

        //                    //context.Address.Update(item2);
        //                    context.Contact.Remove(item2);

        //                    await context.SaveChangesAsync();
        //                }

        //            }

        //            foreach (var item2 in adds)
        //            {
        //                var ity = adds1.Where(x => x.ContactID == item2.ContactID
        //                || !string.IsNullOrEmpty(x.Name) && !string.IsNullOrEmpty(item2.Name)
        //                && (x.Name.ToLower() == item2.Name.ToLower())
        //                ).FirstOrDefault();

        //                if (ity==null)
        //                {
        //                    item2.ContactID = NumericExtensions.GetUniqueID(item2.ContactID);
        //                    item2.AggregateID = item.AggregateID;
        //                    item2.CustomerAggregateAggregateID = item.AggregateID;
        //                    context.Contact.Add(item2);

        //                    await context.SaveChangesAsync();
        //                }
        //                else

        //                {
        //                    context.Contact.Update(item2);

        //                    await context.SaveChangesAsync();

        //                }
        //            }
        //        }
        //    }
        //}

        //public async Task RemovePhoneNumber(Customer newCustomer)
        //{
        //    await using var context = await DbFactory.CreateDbContextAsync();
        //    var a = await context.CustomerAggregates.Include(x => x.PhoneNumbers).Where(x => x.CustomerID == newCustomer.CustomerID).ToListAsync();

        //    if (a.Count > 0)
        //    {
        //        var adds = new List<PhoneNumber>();
        //        if (newCustomer?.Aggregates?.ElementAtOrDefault(0)?.PhoneNumbers?.Count > 0)
        //        {
        //            adds = newCustomer.Aggregates.ElementAtOrDefault(0).PhoneNumbers.ToList();
        //        }
        //        else
        //        {



        //        }
        //        foreach (var item in a)
        //        {

        //            foreach (var item2 in item.PhoneNumbers)
        //            {

        //                var match = adds.Where(x => x.PhoneNumberID == item2.PhoneNumberID).FirstOrDefault();

        //                if (match == null)
        //                {


        //                    //context.Address.Update(item2);
        //                    context.PhoneNumber.Remove(item2);

        //                    await context.SaveChangesAsync();
        //                }
        //                //else
        //                //{
        //                //    context.PhoneNumber.Update(item2);

        //                //    await context.SaveChangesAsync();
        //                //}
        //            }
        //            foreach (var item2 in adds)
        //            {

        //                if (item2.PhoneNumberID == 0)
        //                {
        //                    //item2.AggregateID = item.AggregateID;
        //                    item2.CustomerAggregateAggregateID = item.AggregateID;
        //                    context.PhoneNumber.Add(item2);

        //                    await context.SaveChangesAsync();
        //                }
        //                else
        //                {
        //                    context.PhoneNumber.Update(item2);

        //                    await context.SaveChangesAsync();
        //                }
        //            }
        //        }
        //    }
        //}

        public async Task RemoveSocial(Customer newCustomer,TContext context, bool IsApi = false)
        {
            
            var a = await context.CustomerAggregates.AsNoTracking().Include(x => x.Socials).Where(x => x.CustomerID == newCustomer.CustomerID).ToListAsync();

            if (a.Count > 0)
            {
                var adds = new List<Social>();
                if (newCustomer?.Aggregates?.ElementAtOrDefault(0)?.Socials?.Count > 0)
                {
                    adds = newCustomer.Aggregates.ElementAtOrDefault(0).Socials.ToList();
                }
                else
                {



                }
                foreach (var item in a)
                {

                    foreach (var item2 in item.Socials)
                    {




                        var match = adds.Where(x => x.SocialID == item2.SocialID).FirstOrDefault();

                        if (match == null)
                        {
                            //context.Address.Update(item2);
                            context.Social.Remove(item2);

                            await context.SaveChangesAsync();
                        }
                        //else
                        //{
                        //    context.Social.Update(item2);

                        //    await context.SaveChangesAsync();
                        //}
                    }
                    foreach (var item2 in adds)
                    {

                        if (item2.SocialID == 0)
                        {
                            //item2.AggregateID = item.AggregateID;
                            item2.CustomerAggregateAggregateID = item.AggregateID;
                            context.Social.Add(item2);

                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            context.Social.Update(item2);
                            await context.SaveChangesAsync();
                        }
                    }
                }
            }
        }



        public async Task<bool> Save()
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            return (await context.SaveChangesAsync()) > 0;
        }

        public async Task<Customer> GetCustomerByName(string name,string customname=null)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            
            Customer customer = null;

            if (!string.IsNullOrEmpty(customname))
            {

                customer = await context.Customer.AsNoTracking().Where(c => !string.IsNullOrEmpty(customname) && c.CustomID.Equals(customname)).FirstOrDefaultAsync();
                
                if(customer != null) 
                {
                    return customer;
                }


            }

            //if(customer==null && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(customname))
            //{
            //    var aaa = await context.Customer.AsNoTracking().Where(c => c.Name.Equals(name)).FirstOrDefaultAsync();

            //    return aaa;
            //}
           
            
            if (string.IsNullOrEmpty(customname) && !string.IsNullOrEmpty(name))
            {
                var aaa = await context.Customer.AsNoTracking().Where(c => c.Name.Equals(name)).FirstOrDefaultAsync();

                return aaa;
            }
            //else if (!string.IsNullOrEmpty(customname) && !string.IsNullOrEmpty(name))
            //{
            //    var aaa2 = await context.Customer.AsNoTracking().Where(c =>  !string.IsNullOrEmpty(customname) && c.CustomID.Equals(customname)).FirstOrDefaultAsync();

            //    var aaa = await context.Customer.AsNoTracking().Where(c=> c.Name.Equals(name) && !string.IsNullOrEmpty(customname) && c.CustomID.Equals(customname)).FirstOrDefaultAsync();

            //    if(aaa2.Name.ToLower() != aaa.Name.ToLower())
            //    {
            //        throw new Exception("The EBMS ID code and the Customer name do not match consult your administrator");
            //    }

            //    return aaa;
            //}
            

            return null;
           
        }

        #region Dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void InsertUser(User userDTO)
        {
            throw new NotImplementedException();
        }


        public async Task<ICollection<Address>> GetAddressesAsync()
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            //customerDTO.IsDelete = true;


            var a = await context.Address.ToListAsync();

            return a;
        }

        public async Task<ResultSet<Address>> GetAddress(Pagination<Address> pagination)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var filterQuery = Querys.AddressFilter(pagination.Filter);
            var queriable = context.Address;
            var simplequery = context.Address;

            var result = await queriable.PaginationAndFilterQuery<Address>(pagination, simplequery, filterQuery);

            return result;
        }

        public async Task<ResultSet<AddressCustomerViewModel>> GetAddressCustomer(Pagination<AddressCustomerViewModel> pagination)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            // Set command timeout to prevent timeout errors
            context.Database.SetCommandTimeout(120); // 2 minutes timeout

            // Optimized query with AsNoTracking for better performance
            var query = from address in context.Address.AsNoTracking()
                       join aggregate in context.CustomerAggregates.AsNoTracking() on address.AggregateID equals aggregate.AggregateID
                       join customer in context.Customer.AsNoTracking() on aggregate.CustomerID equals customer.CustomerID
                       where !address.IsDelete && !customer.IsDelete
                       select new AddressCustomerViewModel
                       {
                           AddressId = address.AddressId,
                           StreetAddress1 = address.StreetAddress1 ?? "",
                           StreetAddress2 = address.StreetAddress2 ?? "",
                           StreetAddress3 = address.StreetAddress3 ?? "",
                           CityID = address.CityID ?? "",
                           City = address.City ?? "",
                           StateID = address.StateID ?? "",
                           State = address.State ?? "",
                           ZipCode = address.ZipCode ?? "",
                           CountryID = address.CountryID ?? "",
                           Country = address.Country ?? "",
                           Description = address.Description ?? "",
                           AggregateID = address.AggregateID,
                           IsDefault = address.IsDefault,
                           IsEnable = address.IsEnable,
                           County = address.County ?? "",
                           IsDelete = address.IsDelete,
                           Name = address.Name ?? "",
                           CustomerID = customer.CustomerID,
                           CustomerName = customer.Name ?? "",
                           CustomID = customer.CustomID ?? "",
                           CustomerDescription = customer.Description ?? ""
                       };

            var filterQuery = Querys.AddressCustomerFilterOptimized(pagination.Filter);
            var result = await query.PaginationAndFilterQuery<AddressCustomerViewModel>(pagination, query, filterQuery);

            return result;
        }

        // Alternative high-performance method with pre-filtering
        public async Task<ResultSet<AddressCustomerViewModel>> GetAddressCustomerOptimized(Pagination<AddressCustomerViewModel> pagination)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            // Set command timeout
            context.Database.SetCommandTimeout(120);

            // Pre-filter addresses if search term is provided
            IQueryable<Address> addressQuery = context.Address.AsNoTracking().Where(a => !a.IsDelete);

            if (!string.IsNullOrEmpty(pagination.Filter))
            {
                var filter = pagination.Filter.ToLower();
                addressQuery = addressQuery.Where(a =>
                    (a.StreetAddress1 != null && a.StreetAddress1.ToLower().Contains(filter)) ||
                    (a.City != null && a.City.ToLower().Contains(filter)) ||
                    (a.State != null && a.State.ToLower().Contains(filter)) ||
                    (a.ZipCode != null && a.ZipCode.ToLower().Contains(filter)) ||
                    a.AddressId.ToString().Contains(filter)
                );
            }

            // Join with pre-filtered addresses
            var query = from address in addressQuery
                       join aggregate in context.CustomerAggregates.AsNoTracking() on address.AggregateID equals aggregate.AggregateID
                       join customer in context.Customer.AsNoTracking() on aggregate.CustomerID equals customer.CustomerID
                       where !customer.IsDelete
                       select new AddressCustomerViewModel
                       {
                           AddressId = address.AddressId,
                           StreetAddress1 = address.StreetAddress1 ?? "",
                           StreetAddress2 = address.StreetAddress2 ?? "",
                           StreetAddress3 = address.StreetAddress3 ?? "",
                           City = address.City ?? "",
                           State = address.State ?? "",
                           ZipCode = address.ZipCode ?? "",
                           Country = address.Country ?? "",
                           Description = address.Description ?? "",
                           AggregateID = address.AggregateID,
                           IsDefault = address.IsDefault,
                           IsEnable = address.IsEnable,
                           County = address.County ?? "",
                           IsDelete = address.IsDelete,
                           Name = address.Name ?? "",
                           CustomerID = customer.CustomerID,
                           CustomerName = customer.Name ?? "",
                           CustomID = customer.CustomID ?? "",
                           CustomerDescription = customer.Description ?? ""
                       };

            // Apply additional customer filtering if needed
            if (!string.IsNullOrEmpty(pagination.Filter))
            {
                var filter = pagination.Filter.ToLower();
                query = query.Where(vm =>
                    (vm.CustomerName != null && vm.CustomerName.ToLower().Contains(filter)) ||
                    (vm.CustomID != null && vm.CustomID.ToLower().Contains(filter)) ||
                    vm.CustomerID.ToString().Contains(filter) ||
                    (vm.StreetAddress1 != null && vm.StreetAddress1.ToLower().Contains(filter)) ||
                    (vm.City != null && vm.City.ToLower().Contains(filter)) ||
                    (vm.State != null && vm.State.ToLower().Contains(filter)) ||
                    (vm.ZipCode != null && vm.ZipCode.ToLower().Contains(filter))
                );
            }

            // Use null filter for PaginationAndFilterQuery since we already applied filtering
            var result = await query.PaginationAndFilterQuery<AddressCustomerViewModel>(pagination, query, null);

            return result;
        }

        public async Task<Address> GetAddressesByIDAsync(Address Address)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            //customerDTO.IsDelete = true;


            var a = await context.Address.AsNoTracking().Where(x => x.AddressId == Address.AddressId).FirstOrDefaultAsync();

            return a;

        }

        public async Task<Address> InserAddress(Address DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var exis = await context.Address.AsNoTracking().Where(x => x.AddressId == DTO.AddressId).FirstOrDefaultAsync();
            if (exis == null)
            {
                context.Address.Add(DTO);
            }
            else
            {
                context.Address.Update(DTO);
            }

            await context.SaveChangesAsync();

            return DTO;

        }

        public async Task Clear()
        {
            await using var context = await DbFactory.CreateDbContextAsync();


            await context.Social.Clear(context);
            await context.Address.Clear(context);
            await context.PhoneNumber.Clear(context);
            await context.Contact.Clear(context);
            await context.CustomerAggregates.Clear(context);
            await context.Customer.Clear(context);

        }

        public async Task<Customer> ReplaceCustomer(CustomerReplaced DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            if (DTO != null  && DTO.CustomerNew != null)
            {
                if ( DTO.ListPieceOfEquipment != null)
                {
                    foreach (var item in DTO.ListPieceOfEquipment)
                    {
                        item.CustomerId = DTO.CustomerNew.CustomerID;
                        item.AddressId = DTO.CustomerOld.AddressId;
                        item.EquipmentTemplate = null;
                        var existingEntity = await context.PieceOfEquipment
                            .AsNoTracking()
                            .FirstOrDefaultAsync(e => e.PieceOfEquipmentID == item.PieceOfEquipmentID);

                        if (existingEntity != null)
                        {
                            context.Entry(existingEntity).State = EntityState.Detached;
                        }

                        context.PieceOfEquipment.Update(item);
                    }
                    await context.SaveChangesAsync();
                }
                DTO.CustomerOld.IsDelete = true;
                context.Customer.Update(DTO.CustomerOld);
                await context.SaveChangesAsync();
                var result = await GetCustomerByID(DTO.CustomerNew);
                return result;
            }
            return DTO.CustomerOld;

        }

        public async Task<IEnumerable<Contact>> GetContactsByCustomID(Customer cust)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            int? customID = cust.CustomerID;
            // First find the customer by CustomID
            var customer = await context.Customer
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CustomerID == customID);

            //if (customer == null)
            //{
            //    return new ResultSet<Contact> { List = new List<Contact>(), TotalRows = 0 };
            //}

            // Get the customer aggregates
            var customerAggregates = await context.CustomerAggregates
                .AsNoTracking()
                .Where(ca => ca.CustomerID == customer.CustomerID)
                .Select(ca => ca.AggregateID)
                .ToListAsync();

            //if (!customerAggregates.Any())
            //{
            //    return new ResultSet<Contact> { List = new List<Contact>(), TotalRows = 0 };
            //}

            var contacts = await context.Contact
                .AsNoTracking()
                .Where(c => customerAggregates.Contains(c.AggregateID))
                .ToListAsync();
            
            var result = contacts.ToList();
            return (IEnumerable<Contact>) result;
        }

        public async Task<Contact> GetContactByEmail(string email)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            // Search for contact by email across all customer aggregates
            var contact = await context.Contact
                .AsNoTracking()
                .Where(c => !c.IsDelete && c.IsEnabled &&
                           !string.IsNullOrEmpty(c.Email) &&
                           c.Email.ToLower() == email.ToLower())
                .FirstOrDefaultAsync();

            return contact;
        }


        #endregion
    }
}
