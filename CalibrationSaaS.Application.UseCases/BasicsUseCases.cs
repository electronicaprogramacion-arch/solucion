using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.BusinessExceptions;
using CalibrationSaaS.Domain.BusinessExceptions.Customer;
using CalibrationSaaS.Domain.BusinessExceptions.Procedures;
using CalibrationSaaS.Domain.Repositories;
using Helpers;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.UseCases
{
    

    public partial class BasicsUseCases 
    {
        private readonly IBasicsRepository basicsRepository;
        private readonly IIdentityRepository IdentityRepository;
        private readonly IWorkOrderDetailRepository wodRepository;
        
        public BasicsUseCases(IBasicsRepository basicsRepository, IIdentityRepository _IdentityRepository, IWorkOrderDetailRepository _wodRepository)
        {
            this.basicsRepository = basicsRepository;
            this.IdentityRepository = _IdentityRepository;
            this.wodRepository = _wodRepository;

        }
        #region Manufacturer
        public async Task<Manufacturer> CreateManufacturer(Manufacturer DTO)
        {

            if (await ExistsManufacturer(DTO))
            {
                var result = await this.basicsRepository.UpdateManufacturer(DTO);
                return result;

            }
            else
            {
                var result = await this.basicsRepository.CreateManufacturer(DTO);
                return result;
            }


        }

        public async Task<IEnumerable<Manufacturer>> GetManufacturers()
        {

            var a = await basicsRepository.GetManufacturers();

            return a;

        }



            public async Task<ResultSet<Manufacturer>> GetAllManufacturers(Pagination<Manufacturer> Pagination)
        {
            try { 

            var  a= await basicsRepository.GetAllManufacturers(Pagination);

            return a;
            }
            catch(Exception ex)
            {

                throw ex;
            }

        }

        public async Task<Manufacturer> DeleteManufacturer(Manufacturer id)
        {
            var result = await basicsRepository.DeleteManufacturer(id);
            return result;
        }

        public async Task<Manufacturer> UpdateManufacturer(Manufacturer DTO)
        {
           
            
            var result = await this.basicsRepository.UpdateManufacturer(DTO);
         

            await this.basicsRepository.Save();

            return result;
        }

        public async Task<bool> ExistsManufacturer(Manufacturer DTO)
        {
            var result = await basicsRepository.GetManufacturerXId(DTO.ManufacturerID);
            if (result != null)
                return true;
            else

                return false;
        }

        public async Task<Manufacturer> GetManufacturerXName(Manufacturer DTO)
        {
           
            var result = await this.basicsRepository.GetManufacturerXName(DTO);

            if ((result !=null && result.ManufacturerID == DTO.ManufacturerID && result.Name == DTO.Name) || result == null)
            {
                return new Manufacturer(); 
            }
            else 
            {
                return result;
            }
            

           
        }


        public async Task<ResultSet<EquipmentTypeGroup>> GetEquipmentTypeGroups()
        {
            var result = await this.basicsRepository.GetEquipmentTypeGroups();

            return result;  
        }


        #endregion

        #region Procedure
        public async Task<Procedure> CreateProcedure(Procedure DTO)
        {

            var doesProcedureExists = await this.basicsRepository.GetProcedures();
            
            if (await ExistsProcedure(DTO))
            {
                var result = await this.basicsRepository.UpdateProcedure(DTO);
                return result;

            }
            else
            {
                var result = await this.basicsRepository.CreateProcedure(DTO);
                return result;
            }


        }

        public async Task<IEnumerable<Procedure>> GeProcedures()
        {

            var a = await basicsRepository.GetProcedures();

            return a;

        }



        public async Task<ResultSet<Procedure>> GetAllProcedures(Pagination<Procedure> Pagination)
        {
            try
            {

                var a = await basicsRepository.GetAllProcedures(Pagination);

                return a;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<Procedure> DeleteProcedure(Procedure id)
        {
            var result = await basicsRepository.DeleteProcedure(id);
            return result;
        }

        public async Task<Procedure> UpdateProcedure(Procedure DTO)
        {
           var doesProcedureExists = await this.basicsRepository.GetProcedures();
            var doesProcedureExists1 = doesProcedureExists.Where(x => x.Name == DTO.Name);
            
           if(doesProcedureExists1 != null)
            {
                throw new ExistingProcedureException("This Procedure is already in", null, doesProcedureExists1.FirstOrDefault());
            }
           
            var result = await this.basicsRepository.UpdateProcedure(DTO);
          

            await this.basicsRepository.Save();

            return result;
        }

        public async Task<bool> ExistsProcedure(Procedure DTO)
        {
            var result = await basicsRepository.GetProcedureXId(DTO.ProcedureID);
            if (result != null)
                return true;
            else

                return false;
        }

        public async Task<Procedure> GetProcedureXName(Procedure DTO)
        {
           
            var result = await this.basicsRepository.GetProcedureXName(DTO);

            if ((result != null && result.ProcedureID == DTO.ProcedureID && result.Name == DTO.Name) || result == null)
            {
                return new Procedure();
            }
            else
            {
                return result;
            }

        }
        #endregion
        #region EquipmentType
        public async Task<EquipmentType> CreateEquipmentType(EquipmentType DTO)
        {
            if (await ExistsEquipmentType(DTO))
            {
                var result = await this.basicsRepository.UpdateEquipmentType(DTO);
                return result;

            }
            else
            {
                var result = await this.basicsRepository.CreateEquipmentType(DTO);
                return result;
            }
            

          
            
        }

        public async Task<IEnumerable<EquipmentType>> GetAllEquipmentType()
        {
            return await basicsRepository.GetAllEquipmentType();

        }

        public Task<ResultSet<EquipmentType>> GetEquipmenTypesPag(Pagination<EquipmentType> Pagination)
        {
            return basicsRepository.GetEquipmenTypesPag( Pagination);

        }

        public Task<ResultSet<CalibrationType>> GetCalibrationTypesPag(Pagination<CalibrationType> Pagination)
        {
            return basicsRepository.GetCalibrationTypesPag(Pagination);

        }


        public async Task<EquipmentType> DeleteEquipmentType(EquipmentType id)
        {
            
                var result = await basicsRepository.DeleteEquipmentType(id);
                return result;
            
            
        }

        public async Task<EquipmentType> UpdateEquipmentType(EquipmentType DTO)
        {
            var result = await this.basicsRepository.UpdateEquipmentType(DTO);
            await this.basicsRepository.Save();

            return result;
        }

        public async Task<EquipmentType> GetEquipmentTypeByID(EquipmentType DTO)
        {
            var result = await basicsRepository.GetEquipmentTypeXId(DTO.EquipmentTypeID);
            return result;
        }

        public async Task<bool> ExistsEquipmentType(EquipmentType DTO)
        {
            var result = await basicsRepository.GetEquipmentTypeXId(DTO.EquipmentTypeID);
            if (result != null)
                return true;
            else

                return false;
        }





        #endregion
        #region CMCValues
        public async Task<List<CMCValues>>GetCMCValuesByCalibrationType(CalibrationType DTO)
        {
            return await basicsRepository.GetCMCValuesByCalibrationType(DTO);

        }
        public async Task<CMCValues> GetCMCValuesByID(CMCValues DTO)
        {
            var result = await basicsRepository.GetCMCValuesByID(DTO);
            return result;
        }
        public async Task<CalibrationType> GetCalibrationTypeByID(CalibrationType DTO)
        {
            var result = await basicsRepository.GetCalibrationTypeByID(DTO);
            return result;
        }
        public async Task<CMCValues> DeleteCMCValue(CMCValues DTO)
        {

            var result = await basicsRepository.DeleteCMCValue(DTO);
            return result;


        }
        public async Task<CalibrationType> InsertCMCValue(CalibrationType DTO)
        {
          
                var result = await this.basicsRepository.InsertCMCValue(DTO);
                return result;
       
        }
        public async Task<CMCValues> UpdateCMCValue(CMCValues DTO)
        {
            var result = await this.basicsRepository.UpdateCMCValue(DTO);
            await this.basicsRepository.Save();

            return result;
        }

       

        #endregion
        #region Technician


        public async Task<User> CreateUser(User DTO,bool Fromapi=false)
        {
            var exists = await this.basicsRepository.GetUserById2(DTO,Fromapi);


            var a = await GetUserIdentity(DTO);

            User result;

            //try
            //{
               
                var rl = DTO.RolesList;

                var DTOTMP = DTO;

                string DefaultRol = "job";

                if (Fromapi && exists != null && a==null)
                {
                DefaultRol = "tech";
                DTOTMP.PassWord = "LTICalibration123$";
                //DTOTMP.UserName = DTOTMP.UserName;
                //DTOTMP.Name=
                if (!string.IsNullOrEmpty(DTOTMP.UserName) && string.IsNullOrEmpty(DTOTMP.Email))
                {
                    DTOTMP.Email = DTOTMP.UserName + "@calibration.com";
                }
                if (!string.IsNullOrEmpty(DTOTMP.UserName) && string.IsNullOrEmpty(DTOTMP.Name))
                {
                    DTOTMP.Name = DTOTMP.UserName;
                }

                if (string.IsNullOrEmpty(DTOTMP.UserName) && !string.IsNullOrEmpty(DTOTMP.Email))
                {
                    DTOTMP.UserName = DTO.Email;
                }

                if (string.IsNullOrEmpty(DTOTMP.LastName))
                {
                    DTOTMP.LastName = "NOT COFIGURED";
                }

                DTOTMP.PasswordReset = true;


                }
            if (string.IsNullOrEmpty(DTOTMP.Roles) || (DTOTMP.RolesList == null || DTOTMP.RolesList.Count == 0))
            {
                DTOTMP.Roles = DefaultRol;
                List<Rol> rlfg = new List<Rol>();
                Rol rfg = new Rol();
                rfg.IsDelete = false;
                rfg.Name = DefaultRol;
                rfg.RolID = 1;
                rlfg.Add(rfg);
                DTOTMP.RolesList = rlfg;

            }
            if (rl!= null)
                {
                    string roleslist = "";
                    string prefix = "";
                    foreach(var item in rl)
                    {
                        prefix = item.Name.Trim();
                        
                        if(item.PermissionList != null)
                        {
                            foreach (var i in item.PermissionList)
                            {
                                roleslist = roleslist + prefix + "." + i.Key + ",";
                            }                            
                        }
                        roleslist = prefix + "," + roleslist;
                    }
                    if (!string.IsNullOrEmpty(roleslist))
                    {
                        DTOTMP.Roles = roleslist;
                        DTOTMP.Roles = DTOTMP.Roles.Substring(0, DTOTMP.Roles.LastIndexOf(","));
                    }
                   
                }

                if (DTOTMP.PasswordReset && !DTOTMP.Roles.Contains("reset"))
                {
                   DTOTMP.Roles = DTOTMP.Roles + ",reset";
                }


            if (string.IsNullOrEmpty(DTOTMP.UserName))
            {
                throw new Exception("UserName is Null");
            }






                User r2 = null;

             if(!Fromapi || Fromapi && a ==null && exists != null)
            {
                r2 = await IdentityRepository.CreateRecord(DTOTMP);
                if (Fromapi)
                {
                    exists.PasswordReset = true;
                    result = await this.basicsRepository.UpdateUser(exists);
                }
                
            }
                
              



                if (r2 != null && !string.IsNullOrEmpty(r2.Message))
                {
                    throw new LoginException(r2.Message);
                }


                DTO.RolesList = rl;

                if (exists != null)
                {
                        if (Fromapi)
                        {

                            DTO.UserID = exists.UserID;
                            result = DTO;

                        }
                        else
                        {
                            DTO.UserID = exists.UserID;
                            result = await this.basicsRepository.UpdateUser(DTO);
                        }

                      
                }
            else if(!Fromapi && exists==null)
                {


                    result = await this.basicsRepository.CreateUser(DTO);

                };


                result = await GetUserById(DTO);

                if (Fromapi && a==null && exists != null)
                {
                result.PasswordReset = true;
                }

                return result;



            //}
            //catch (Exception ex)
            //{

            //    var valme = ex.Message;
            //   if(ex.InnerException != null)
            //    {
            //      valme= valme + " Inerrexception: "  + ex.InnerException.Message;
            //    }

            //    throw new Exception(valme);
            //}

 
        }

        public async Task<IEnumerable<User>> GetAllUserIdentity()
        {


            var a = await IdentityRepository.ShowRecords();

            return a;

        }

        public async Task<User> GetUserIdentity(User DTO)
        {

           
            var a = await IdentityRepository.ShowRecords(DTO.UserName);

            return a;
        
        }


            public async Task<User> GetUserById(User DTO)
        {


            User result = null;

            if(DTO.UserID != 0)
            {
                result = await this.basicsRepository.GetUserById(DTO.UserID);
            }
            else
            {
                result = await this.basicsRepository.GetUserByUserName(DTO.UserName);
            }


            if(result != null)
            {
                result.RolesList = new List<Rol>();
                foreach (var item in result.UserRoles)
                {
                    if (item.Rol != null)
                    {
                        item.Rol.UserRoles = null;
                    }

                    if (!string.IsNullOrEmpty(item.Permissions))
                    {
                        var arr = item.Permissions.Split(",");
                        item.Rol.PermissionList = new List<KeyValue>();
                        int cont = 0;
                        foreach (var i in arr)
                        {
                            item.Rol.PermissionList.Add(new KeyValue() { Key = i, Value = cont });
                            cont = cont + 1;
                        }
                        item.Rol.Permission = item.Permissions;
                    }

                    item.User.UserRoles = null;
                    item.Rol.UserRoles = null;
                    result.RolesList.Add(item.Rol);

                }

                result.UserRoles = null;
                if (result?.TechnicianCodes?.Count > 0)
                {
                    foreach (var r in result.TechnicianCodes)
                    {
                        r.User = null;
                    }
                }

            }


            if (result != null)
            {
                DTO = result;
            }

            var a = await GetUserIdentity(DTO);


            if(a== null)
            {
                return null;
            }

            bool reset=false;

            bool enable = true;

            var a12 = a.RolesList.Where(x => x.Name == "disable").FirstOrDefault();

            if(a12 != null)
            {
                enable = false;
                a.RolesList.Remove(a12);
            }

            var a13 = a.RolesList.Where(x => x.Name == "reset").FirstOrDefault();
            if (a13 != null)
            {
                reset = true ;
                a.RolesList.Remove(a13);
            }

            

            if (result!= null)
            {
                result.RolesList2 = a.RolesList;

                result.RolesList = a.RolesList;

                result.Roles2 = a.Roles;

                result.IsEnabled= enable;   

                result.PasswordReset= reset;    
                return result;

            }
            else
            {
                a.RolesList2 = a.RolesList;

                a.Roles2 = a.Roles;

                a.IsEnabled = enable;

                a.PasswordReset = reset;
                
                return a;    
            }
           

        }

        public async Task<User> GetUserById2(User DTO)
        {
            var result = await this.basicsRepository.GetUserById2(DTO);
            return result;
        }



            public async Task<IEnumerable<User>> GetAllUser()
        {
            return await basicsRepository.GetAllUser();

        }


       


        public async Task<Rol> DeleteRole(Rol rol)
        {


            var a = await IdentityRepository.DeleteRol(rol);



            return await basicsRepository.DeleteRole(rol);

        }


        public async Task<Rol>  AddRol(Rol rol)
        {
            
            return await basicsRepository.AddRol(rol);
        
        
        }

        public async Task<User> DeleteUser(User id)
        {
            var a = await basicsRepository.DeleteUser(id);


            if (a != null)
            {
                var result = await IdentityRepository.DeleteRecordByID(id.UserName);

                return result;
                
            }
            else
            {
                throw new Exception("Error Delete Tech");
            }

        }

        public async Task<User> UpdateUser(User DTO)
        {
           
            var result = await this.basicsRepository.UpdateUser(DTO);
           
            await this.basicsRepository.Save();

            return result;
        }

        public async Task<IEnumerable<Certification>> GetCertifications()
        {
            return await basicsRepository.GetCertifications();

        }

        public async Task<IEnumerable<ToleranceType>> GetToleranceTypes()
        {
            return await basicsRepository.GetToleranceTypes();

        }


        public async Task<ResultSet<TechnicianCode>> GetTechnicianCodePag(Pagination<TechnicianCode> pagination)
        {
            return await basicsRepository.GetTechnicianCodePag(pagination);
        }

        public async Task<ResultSet<User>> GetUserPag(Pagination<User> pagination)
        {
            return await basicsRepository.GetUserPag(pagination);
        }

        public async Task<TechnicianCode>  CreateTechnicianCode(TechnicianCode DTO)
        {
            return await basicsRepository.CreateTechnicianCode(DTO);
        }

        public async  Task<TechnicianCode> DeleteTechnicianCode(TechnicianCode DTO)
        {
            return await basicsRepository.DeleteTechnicianCode(DTO);
        }

        #endregion

        #region Contact
        public async Task<Contact> CreateContact(Contact DTO)
        {
            
            var result = await this.basicsRepository.CreateContact(DTO);

            await this.basicsRepository.Save();
            return result;
        }

        public async Task<IEnumerable<Contact>> GetAllContact()
        {
            return await basicsRepository.GetAllContact();

        }

        public async Task<Contact> DeleteContact(Contact id)
        {
            var result = await basicsRepository.DeleteContact(id);
            return result;
        }

        public async Task<Contact> UpdateContact(Contact DTO)
        {

            var result = await this.basicsRepository.UpdateContact(DTO);

            await this.basicsRepository.Save();

            return result;
        }


        #endregion

        #region Status
      
        public async Task<IEnumerable<Status>> GetAllStatus()
        {
            return await basicsRepository.GetAllStatus();

        }

        #endregion

        #region Rol

        public async Task<IEnumerable<Rol>> GetAllRoles()
        {
            var a= await basicsRepository.GetAllRoles();

            foreach(var item in a)
            {
                if (!string.IsNullOrEmpty(item.DefaultPermissions))
                {
                    var arrs = item.DefaultPermissions.Split(",",StringSplitOptions.RemoveEmptyEntries);

                    item.DefaultPermissionList = arrs.ConvertListStringinKeyValue().ToList();

                }
            }

            return a;

        }

        #endregion

        #region Address
        public async Task<Address> CreateAddress(Address DTO)
        {

            var result = await this.basicsRepository.CreateAddress(DTO);

            await this.basicsRepository.Save();
            return result;
        }

        public async Task<IEnumerable<Address>> GetAllAddress()
        {
            return await basicsRepository.GetAllAddress();

        }

        public async Task<Address> DeleteAddress(int id)
        {
            var result = await basicsRepository.DeleteAddress(id);
            return result;
        }

        public async Task<Address> UpdateAddress(Address DTO)
        {

            var result = await this.basicsRepository.UpdateAddress(DTO);

            await this.basicsRepository.Save();

            return result;
        }


        public async Task<Component> DeleteComponent(Component Component)
        {
            var result = await this.basicsRepository.DeleteComponent(Component);
            return result;
        }

        public string FormatRoute(string route)
        {

            var a = route.Split('/', StringSplitOptions.RemoveEmptyEntries);


            if(a.Count() > 0)
            {
                return a[0];
            }
            else
            {
                return "";
            }
           
        }
        public async Task<ResultSet<Component>> CreateComponents(ICollection<Component> Component)
        {
            foreach(var item in Component)
            {
               var a=  FormatRoute(item.Route);
               
                    item.Route = a;
                var b = FormatRoute(item.Name);
                item.Name = b;

            }



            var result = await this.basicsRepository.CreateComponents(Component);


            return result;

        }
        public async Task<Component> GetComponentByRoute(string route)
        {
            var result = await this.basicsRepository.GetComponentByRoute(route);


            return result;


        }

        public async ValueTask<bool> Enable(User userDTO)
        {
            var a = await IdentityRepository.Enable(userDTO.UserName);

            return a;
        }

        public async ValueTask<bool> Reset(User userDTO)
        {
            var a = await IdentityRepository.Reset(userDTO.UserName);

            return a;


        }

       public async Task<ResultSet<CustomSequence>> CustomSequences()
        {
            var result = await this.basicsRepository.CustomSequences();


            return result;
        }


            public async Task<ResultSet<Component>> GetAllComponents()
        {
            var result = await this.basicsRepository.GetAllComponents();


            return result;
        }

        #endregion
    }
}
