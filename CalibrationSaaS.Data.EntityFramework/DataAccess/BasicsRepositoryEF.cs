using AutoMapper;
using CalibrationSaaS.Data.EntityFramework;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Infraestructure.EntityFramework.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Helpers.Controls.ValueObjects;
using Helpers.Controls;
using Component = Helpers.Controls.Component;
using Helpers;
using static System.Net.WebRequestMethods;
using Bogus;
using System.Linq.Expressions;

namespace CalibrationSaaS.Infraestructure.EntityFramework.DataAccess
{
    public class BasicsRepositoryEF<TContext> : IBasicsRepository, IDisposable where TContext : DbContext, ICalibrationSaaSDBContextBase 
    {

        private readonly IDbContextFactory<TContext> DbFactory;


        public BasicsRepositoryEF(IDbContextFactory<TContext> dbFactory)
        {
            DbFactory = dbFactory;
        }




        public async Task<bool> Save()
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            return (await context.SaveChangesAsync()) > 0;
        }

        public async Task<Customer> GetCustomerByName(string name, int tenant)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            return await context.Customer.FirstOrDefaultAsync(c => c.Name.Equals(name) && c.TenantId == tenant);
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

        #endregion
        #region EquipmentType

        public async Task<EquipmentTypeGroup> CreateEquipmentTypeGroup(EquipmentTypeGroup DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();




            var a = await context.EquipmentTypeGroup.AsNoTracking().Where(x => x.EquipmentTypeGroupID == DTO.EquipmentTypeGroupID).FirstOrDefaultAsync();

            if (a == null)
            {
                context.EquipmentTypeGroup.Add(DTO);

                await context.SaveChangesAsync();


            }

            return DTO;
        }


        public async Task<ICollection<EquipmentTypeGroup>> GetEquipmentTypeGroup()
        {
            await using var context = await DbFactory.CreateDbContextAsync();




            return await context.EquipmentTypeGroup.AsNoTracking().ToListAsync();

        }

        public async Task CreateToleranceType(ToleranceType DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var a = await context.ToleranceType.AsNoTracking().Where(x => x.Key == DTO.Key).FirstOrDefaultAsync();
            if(a!= null)
            {
                return;
            }
            context.ToleranceType.Add(DTO);
            await context.SaveChangesAsync();

        }


            public async Task<EquipmentType> CreateEquipmentType(EquipmentType DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            ICollection<Note> notes = new List<Note>();
            if(DTO.Notes != null)
            {
                notes =(List<Note>) DTO.Notes.CloneObject();
            }
           
           
            DTO.CalibrationType = null;
            DTO.PieceOfEquipment = null;
            DTO.EquipmentTemplates = null;
            DTO.EquipmentTypeGroup = null;
            DTO.Notes = null;

            var dro = (EquipmentType)DTO.CloneObject();

            //DTO.EquipmentTypeGroupID = null;
            context.EquipmentType.Add(dro);

            await context.SaveChangesAsync();


            if(notes != null && notes.Count > 0)
            {
                foreach (var item in DTO.Notes)
                {
                    await SaveNotes(item);
                }
            }

            await RemoveNotes<EquipmentType>(DTO.EquipmentTypeID,DTO,1);
            return DTO;
        }

        public async Task<EquipmentType> UpdateEquipmentType(EquipmentType DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            DTO.EquipmentTemplates = null;
            context.EquipmentType.Update(DTO);

            await context.SaveChangesAsync();

            if (DTO.Notes != null && DTO.Notes.Count > 0)
            {
                foreach (var item in DTO.Notes)
                {
                    await SaveNotes(item);
                }



            }

            await RemoveNotes<EquipmentType>(DTO.EquipmentTypeID, DTO, 1);


            return DTO;
        }

        public async Task<EquipmentType> GetEquipmentTypeXId(int? id)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var result = await context.EquipmentType.Where(x => x.EquipmentTypeID == id).AsNoTracking().FirstOrDefaultAsync();

            if(result != null)
            {
                result.Notes = await GetNotes(result.EquipmentTypeID,1);
            }


            return result;
                
         }


        public async Task<IEnumerable<EquipmentType>> GetEquipmentTypes()
        {


            await using var context = await DbFactory.CreateDbContextAsync();



            var eqType = await context.EquipmentType.AsNoTracking()
                .AsTracking().
                Where(x => x.IsEnabled == true).OrderBy(x => x.Name).ToListAsync();
            //var PieceOfEquipmentRepository = new PieceOfEquipmentRepositoryEF(context);

            foreach (var eq in eqType)
            {

                if (eq.EquipmentTypeGroupID.HasValue)
                {
                    var etg = await context.EquipmentTypeGroup.AsNoTracking().Where(x => x.EquipmentTypeGroupID == eq.EquipmentTypeGroupID.Value).FirstOrDefaultAsync();

                    eq.EquipmentTypeGroup = etg;
                }


            }



            return eqType;

        }

        public async Task<IEnumerable<EquipmentType>> GetAllEquipmentType()
        {
           
           
           await using var context = await DbFactory.CreateDbContextAsync();
           
           
           
            var eqType = await context.EquipmentType.AsNoTracking()
                .Include(b => b.CalibrationType).AsNoTracking()
                .Include(c => c.CalibrationType.CalibrationSubTypes).AsTracking().
                Where(x=>x.IsEnabled==true).OrderBy(x => x.Name).ToListAsync();
            //var PieceOfEquipmentRepository = new PieceOfEquipmentRepositoryEF(context);

            foreach (var eq in eqType)
            {

                if (eq.EquipmentTypeGroupID.HasValue)
                {
                    var etg = await context.EquipmentTypeGroup.AsNoTracking().Where(x => x.EquipmentTypeGroupID == eq.EquipmentTypeGroupID.Value).FirstOrDefaultAsync();
                
                     eq.EquipmentTypeGroup = etg;   
                }
            }
            return eqType;

        }



        public async Task<ResultSet<EquipmentType>> GetEquipmenTypesPag(Pagination<EquipmentType> pagination)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var filterQuery = Querys.EquipmentTypeFilter(pagination.Filter);

            var queriable = context.EquipmentType;//context.PieceOfEquipment.Include(x => x.EquipmentTemplate).Include(d => d.Customer).AsQueryable();

            var simplequery = context.EquipmentType;

            //var result = await QueryableExtensions.PaginationAndFilterQuery<PieceOfEquipment>(exprTree, pagination, context.PieceOfEquipment, queriable);
            var result = await queriable.PaginationAndFilterQuery<EquipmentType>(pagination, simplequery, filterQuery);

            return result;
        }

        public async Task<ResultSet<CalibrationType>> GetCalibrationTypesPag(Pagination<CalibrationType> pagination)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var filterQuery = Querys.CalibrationTypeFilter(pagination.Filter);

            var queriable = context.CalibrationType;

            var simplequery = context.CalibrationType;

            var result = await queriable.PaginationAndFilterQuery<CalibrationType>(pagination, simplequery, filterQuery);

            return result;
        }
        public async Task<EquipmentType> DeleteEquipmentType(EquipmentType id)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            EquipmentType equipmentType = await context.EquipmentType.FirstOrDefaultAsync(sc => sc.EquipmentTypeID == id.EquipmentTypeID);

            //context.EquipmentType.Remove(equipmentType);
            context.Entry(equipmentType).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            return equipmentType;
            
           

        }


        //public async Task<EquipmentType> UpdateEquipmentType(EquipmentType DTO)
        //{
        //    context.Entry(DTO).State = EntityState.Modified;
        //    context.SaveChanges();

        //    return DTO;
        //}

        #endregion
        #region Technicians
        public async Task<User> CreateUser(User DTO)
        {

            await using var context = await DbFactory.CreateDbContextAsync();
            List<TechnicianCode> tc = new List<TechnicianCode>();
            //DTO.EquipmentTemplates = null;
            if (DTO.TechnicianCodes != null)
            {
                tc = DTO.TechnicianCodes.ToList();
                DTO.TechnicianCodes = null;
            }

            DTO.PassWord = null;
            context.User.Add(DTO);

            await context.SaveChangesAsync();

            if(DTO.RolesList != null)
            {
                foreach (var item in DTO.RolesList)
                {
                    var ur = new User_Rol();

                    ur.RolID = item.RolID;
                    ur.UserID = DTO.UserID;

                    if (item.PermissionList != null)
                        ur.Permissions = string.Join(",", item.PermissionList.Select(x => x.Key));


                    context.User_Rol.Add(ur);
                    await context.SaveChangesAsync();

                }
            }
           
            if(tc != null)
            {
                foreach (var item in tc)
                {
                    try
                    {
                        item.Certification = null;
                        item.UserID = DTO.UserID;
                        context.TechnicianCode.Add(item);
                        await context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }                
            }

            return DTO;

        }

        public async Task<User> GetUserById(int id)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var result = await context.User.Include(x => x.UserRoles).
                ThenInclude(x => x.Rol).Where(x => x.UserID == id).AsNoTracking().
                Include(b=>b.TechnicianCodes).ToArrayAsync();
            List<TechnicianCode> tc = new List<TechnicianCode>();
            foreach (var item in result)
            {
                foreach (var tech in item.TechnicianCodes)
                {
                    tech.User = null;
                }
               
            }    
            return result.FirstOrDefault();
        }

        public async Task<User> GetUserById2(User DTO,bool fromapi=false)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            //User user = new User();
            var result = await context.User.AsNoTracking().Include(x=>x.TechnicianCodes).AsNoTracking().Where(x => x.UserID == DTO.UserID
            ||!string.IsNullOrEmpty(x.Email) && !string.IsNullOrEmpty(DTO.Email) && x.Email.ToLower() == DTO.Email.ToLower() 
            || !string.IsNullOrEmpty(DTO.UserName) && fromapi==true  && x.UserName == DTO.UserName)
                .AsNoTracking().Include(x=>x.UserRoles).FirstOrDefaultAsync();

           

            if (result != null)
            {
               
                Certification certification = new Certification();
                foreach (var cert in result.TechnicianCodes)
                {

                    cert.Certification = await context.Certification.AsNoTracking().Where(x => x.CertificationID == cert.CertificationID).FirstOrDefaultAsync();
                }
                
                
                return result;
            }

            else
            {
                return null;
            }
                
        }

        public async Task<IEnumerable<User>> GetAllUser()
        {
            if(DbFactory != null)
            {
                await using var context = await DbFactory.CreateDbContextAsync();
                return await context.User.OrderBy(x => x.Name).ToListAsync();
            }

            return null;

        }

        public async Task<User> DeleteUser(User id)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            try
            {
                User user = await context.User.AsNoTracking().Where(sc => sc.UserID == id.UserID).FirstOrDefaultAsync();

                context.Remove(user);   

                await context.SaveChangesAsync();

                return user;
            }catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            
            
            }
           
        }


        public async Task<User> UpdateUser(User DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            //context.Entry(DTO).State = EntityState.Modified;
            List<TechnicianCode> tc = new List<TechnicianCode>();
            //DTO.EquipmentTemplates = null;
            if (DTO.TechnicianCodes != null)
            {
                tc = DTO.TechnicianCodes.ToList();
            }
            else { }
            DTO.TechnicianCodes = null;
            DTO.PassWord= null; 
            context.User.Update(DTO);
            await context.SaveChangesAsync();

           

                var ur0 = await context.User_Rol.Where(x => x.UserID == DTO.UserID).ToListAsync();


                var result = ur0.Where(p => DTO.RolesList.All(p2 => p2.RolID != p.RolID));

                if(result != null)
                {
                    foreach (var yy in result)
                    {
                        context.User_Rol.Remove(yy);
                        await context.SaveChangesAsync();
                    }
                }
                try
                {
                    if (DTO.RolesList != null)
                    {
                        foreach (var item in DTO.RolesList)
                        {
                            var ur = new User_Rol();

                            ur.RolID = item.RolID;
                            ur.UserID = DTO.UserID;

                            if (item.PermissionList != null)
                                ur.Permissions = string.Join(",", item.PermissionList.Select(x => x.Key));

                            var a = await context.User_Rol.Where(x => x.UserID == DTO.UserID && item.RolID == x.RolID).FirstOrDefaultAsync();
                            if (a == null)
                            {
                                context.User_Rol.Add(ur);
                            }
                            else
                            {
                                context.User_Rol.Update(ur);
                            }

                            await context.SaveChangesAsync();

                        }
                    }
                }
                catch 
                {

                }
               

                if(tc != null)
                {
                    foreach (var item in tc)
                    {
                        var a = await context.TechnicianCode.Where(x => x.TechnicianCodeID == item.TechnicianCodeID).AsNoTracking().ToListAsync();
                        item.Certification = null;
                        item.UserID = DTO.UserID;
                        item.User = null;
                        
                        if (a == null || a?.Count == 0)
                        {
                            context.TechnicianCode.Add(item);
                        }
                        else
                        {
                            context.TechnicianCode.Update(item);
                        }


                        await context.SaveChangesAsync();

                    }
                }
               
           


            return DTO;
        }


        public async Task<IEnumerable<Certification>> GetCertifications()
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            return await context.Certification.ToListAsync();
        }

        public async Task<IEnumerable<ToleranceType>> GetToleranceTypes()
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var a = await context.ToleranceType.AsNoTracking().Where(x=>x.Enable.HasValue==true && x.Enable.Value == true).ToListAsync();

            return a;
        }



        public async Task<Certification> GetCertificationXId(int id)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var a = await context.Certification.AsNoTracking().Where(x => x.CertificationID == id).AsNoTracking().FirstOrDefaultAsync();
            return a;

        }


        public async Task<ResultSet<User>> GetUserPag(Pagination<User> pagination)
        {

            await using var context = await DbFactory.CreateDbContextAsync();
            var filterQuery = Querys.UserFilter(pagination.Filter);

            //var queriable = context.User.Include(x => x.TechnicianCodes);//context.PieceOfEquipment.Include(x => x.EquipmentTemplate).Include(d => d.Customer).AsQueryable();

            //var simplequery = context.User;


            //////////////////////////////////////////////
            ///
            var queriable = context.User.Where(x=>x.IsDelete==false );//context.PieceOfEquipment.Include(x => x.EquipmentTemplate).Include(d => d.Customer).AsQueryable();

            var simplequery = context.User.Where(x => x.IsDelete == false );

            //var result = await QueryableExtensions.PaginationAndFilterQuery<PieceOfEquipment>(exprTree, pagination, context.PieceOfEquipment, queriable);
            var result = await queriable.PaginationAndFilterQuery<User>(pagination, simplequery, filterQuery);

            ///////////////////////////////////////////////

            return result; // await context.Certification.ToListAsync();


        }



        public async Task<ResultSet<TechnicianCode>> GetTechnicianCodePag(Pagination<TechnicianCode> pagination)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            User user = new User();
            user.UserID = pagination.Entity.UserID;
            if (pagination.Entity.UserID != null)
            {
                pagination.Entity.User = user;
                var filterQuery = Querys.TechnianCodeByUserFilter(pagination.Filter, pagination.Entity.User);

                var queriable = context.TechnicianCode.Include(x => x.User).Include(x => x.Certification);//context.PieceOfEquipment.Include(x => x.EquipmentTemplate).Include(d => d.Customer).AsQueryable();

                var simplequery = context.TechnicianCode;

                //if (pagination.Filter == null)
                //{
                //    pagination.Filter = "Delete";
                //}

                //var result = await QueryableExtensions.PaginationAndFilterQuery<PieceOfEquipment>(exprTree, pagination, context.PieceOfEquipment, queriable);
                var result = await queriable.PaginationAndFilterQuery<TechnicianCode>(pagination, simplequery, filterQuery);


                return result; // await context.Certification.ToListAsync();

            }
            else
                return null;
        }

        public Task<TechnicianCode> CreateTechnicianCode(TechnicianCode DTO)
        {
            throw new NotImplementedException();
        }

        public async Task<TechnicianCode> DeleteTechnicianCode(TechnicianCode DTO)
        {
            //try
            //{
            //    context.Entry(DTO).State = EntityState.Deleted;
            //    await context.SaveChangesAsync();
            //}
            //catch (Exception ex)
            //{
            //    //DTO.isDelete = true;
            //    context.Entry(DTO).State = EntityState.Deleted;
            //    await context.SaveChangesAsync();
            //    return DTO;
            //}
            await using var context = await DbFactory.CreateDbContextAsync();
            context.Entry(DTO).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            return DTO;
        }

        //public async Task UpdateManufacturer(Manufacturer DTO)
        //{
        //      context.Entry(DTO).State = EntityState.Modified;

        //}

        //public async Task<EquipmentTemplate> Update(EquipmentTemplate EquipmentDTO)
        //{
        //    context.EquipmentTemplate.Add(EquipmentDTO);
        //    await context.SaveChangesAsync();
        //    return EquipmentDTO;
        //}

        #endregion
        #region Contact
        public async Task<Contact> CreateContact(Contact DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            context.Contact.Add(DTO);
            context.SaveChanges();
            return DTO;
        }

        public async Task<IEnumerable<Contact>> GetAllContact()
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            return await context.Contact.OrderBy(x => x.Name).AsNoTracking().ToListAsync();
        }

        public async Task<Contact> DeleteContact(Contact id)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            Contact contact = await context.Contact.FirstOrDefaultAsync(sc => sc.ContactID == id.ContactID);
            context.Entry(contact).State = EntityState.Deleted;
            
            return contact;
        }


        public async Task<Contact> UpdateContact(Contact DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            context.Entry(DTO).State = EntityState.Modified;

            return DTO;
        }
        //public async Task UpdateManufacturer(Manufacturer DTO)
        //{
        //      context.Entry(DTO).State = EntityState.Modified;

        //}

        //public async Task<EquipmentTemplate> Update(EquipmentTemplate EquipmentDTO)
        //{
        //    context.EquipmentTemplate.Add(EquipmentDTO);
        //    await context.SaveChangesAsync();
        //    return EquipmentDTO;
        //}

        #endregion
        #region Manufacturer
        public async Task<Manufacturer> CreateManufacturer(Manufacturer DTO)
        {

            await using var context = await DbFactory.CreateDbContextAsync();










            
            
            
            
            
            var manufacturer = await GetManufacturerXName(DTO);
            if (manufacturer != null && manufacturer.Name != null)
                return null;
                
            context.Manufacturer.Add(DTO);
            await context.SaveChangesAsync();
            return DTO;
        }

        public async Task<IEnumerable<Manufacturer>> GetManufacturers()
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            return await context.Manufacturer.OrderBy(x => x.Name).ToListAsync();

        }


        public async Task<ResultSet<Manufacturer>> GetAllManufacturers(Pagination<Manufacturer> pagination)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var filterQuery = Querys.ManufacturerFilter(pagination.Filter);

            var queriable = context.Manufacturer;//context.PieceOfEquipment.Include(x => x.EquipmentTemplate).Include(d => d.Customer).AsQueryable();


            var simplequery = context.Manufacturer;
            //if (pagination.Filter == null)
            //{
            //    pagination.Filter = "Delete";
            //}
            //var result = await QueryableExtensions.PaginationAndFilterQuery<PieceOfEquipment>(exprTree, pagination, context.PieceOfEquipment, queriable);
            var result = await queriable.PaginationAndFilterQuery<Manufacturer>(pagination, simplequery, filterQuery);

           
            return result;
        }

        public async Task<Manufacturer> DeleteManufacturer(Manufacturer id)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            context.Entry(id).State = EntityState.Deleted;
            //context.Update(id);
            await context.SaveChangesAsync();
            return id;
        }


        public async Task<Manufacturer> UpdateManufacturer(Manufacturer DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            context.Update(DTO);
            // context.Entry(DTO).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return DTO;
        }

        public async Task<Manufacturer> GetManufacturerXId(int id)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var result = await context.Manufacturer.Where(x => x.ManufacturerID == id).AsNoTracking().ToArrayAsync();
            return result.FirstOrDefault();
        }

        public async Task<Manufacturer> GetManufacturerXName(Manufacturer DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var result = await context.Manufacturer.Where(x => x.Name == DTO.Name).AsNoTracking().ToArrayAsync();
            return result.FirstOrDefault();
        }

        #endregion
        #region Procedure
        public async Task<Procedure> CreateProcedure(Procedure DTO)
        {

            await using var context = await DbFactory.CreateDbContextAsync();
            
                DTO.ProcedureID = NumericExtensions.GetUniqueID(DTO.ProcedureID);
            
            
            context.Procedure.Add(DTO);
            await context.SaveChangesAsync();
            return DTO;
        }

        public async Task<IEnumerable<Procedure>> GetProcedures()
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            return await context.Procedure.OrderBy(x => x.Name).ToListAsync();

        }


        public async Task<ResultSet<Procedure>> GetAllProcedures(Pagination<Procedure> pagination)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var filterQuery = Querys.ProcedureFilter(pagination.Filter);

            var queriable = context.Procedure;//context.PieceOfEquipment.Include(x => x.EquipmentTemplate).Include(d => d.Customer).AsQueryable();


            var simplequery = context.Procedure;
            //if (pagination.Filter == null)
            //{
            //    pagination.Filter = "Delete";
            //}
            //var result = await QueryableExtensions.PaginationAndFilterQuery<PieceOfEquipment>(exprTree, pagination, context.PieceOfEquipment, queriable);
            var result = await queriable.PaginationAndFilterQuery<Procedure>(pagination, simplequery, filterQuery);


            return result;
        }

        public async Task<Procedure> DeleteProcedure(Procedure id)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            context.Entry(id).State = EntityState.Deleted;
            //context.Update(id);
            await context.SaveChangesAsync();
            return id;
        }


        public async Task<Procedure> UpdateProcedure(Procedure DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            context.Update(DTO);
            // context.Entry(DTO).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return DTO;
        }

        public async Task<Procedure> GetProcedureXId(int id)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var result = await context.Procedure.Where(x => x.ProcedureID == id).AsNoTracking().ToArrayAsync();
            return result.FirstOrDefault();
        }

        #endregion
        #region equipmenttemplate

        public async Task<EquipmentTemplate> CreateEquipment(EquipmentTemplate EquipmentDTO, 
            bool isThread = false,bool Validate=false,bool throwException=true)
        {


            await using var context = await DbFactory.CreateDbContextAsync();

            //var a1 = await context.EquipmentType.ToListAsync();
            //if (!EquipmentDTO.EquipmentTypeID.GetIsBalance(a1))
            //{
            //    EquipmentDTO.TestGroups = null;
            //}

            bool isDynamic = false;
            if (EquipmentDTO?.EquipmentTypeObject != null)
            {
                isDynamic= EquipmentDTO.EquipmentTypeObject.DynamicConfiguration2;
            }
                
            if(Validate)
            {
                var resd = await context.EquipmentTemplate.AsNoTracking().Where(x => x.EquipmentTemplateID == EquipmentDTO.EquipmentTemplateID).FirstOrDefaultAsync();
            
              if(resd != null)
                {
                    return resd;
                }
            
            }

                var DTO = PrepareDTO(EquipmentDTO);

            //DTO.EquipmentTemplateID = NumericExtensions.GetUniqueID();


            if (DTO.TestPointResult != null && DTO.TestPointResult.Count > 0)
            {

                var groupedCalibrations = DTO.TestPointResult
                    .GroupBy(item => item.CalibrationSubTypeId)
                    .Where(group => group.Any(item => item.Updated != 0))
                    .SelectMany(group => group)
                    .ToList();


                ICollection<GenericCalibrationResult2> genericCalibrationResult2s = new List<GenericCalibrationResult2>();

                foreach (var item in groupedCalibrations)
                {
                    genericCalibrationResult2s.Add(item);
                }

                if (genericCalibrationResult2s.Count == 0)
                {
                    var fas = DTO.TestPointResult
                   .Where(x => x.GenericCalibration2 != null && ((x.GenericCalibration2.WeightSets != null && x.GenericCalibration2.WeightSets.Count > 0)
                   || (x.GenericCalibration2.Standards != null && x.GenericCalibration2.Standards.Count > 0))).FirstOrDefault();

                    if (fas != null)
                    {
                        genericCalibrationResult2s = DTO.TestPointResult;
                    }


                }

                if (isThread || isDynamic)
                {
                    var workOrderDetailRepository = new WODRepositoryEF<TContext>(DbFactory);

                    await workOrderDetailRepository.SaveCalibrationTypes2<GenericCalibration2, GenericCalibrationResult2>("EquipmentTemplate", DTO.EquipmentTemplateID.ToString(),
                   DTO.TestPointResult, workOrderDetailRepository.FormatGenericCalibration2);

                }

            }

            if (DTO.TestGroups != null && DTO.TestGroups.FirstOrDefault().TypeID == null)
                DTO.TestGroups = null;

            

            DTO.TestPointResult = null;


            if (DTO?.AditionalEquipmentTypes?.Count > 0)
            {
                DTO.AditionalEquipmentTypesJSON= Newtonsoft.Json.JsonConvert.SerializeObject(DTO.AditionalEquipmentTypes);
            }

            //DRO.EquipmentTypeObject = null;
            //DRO.EquipmentTypeObjectTemp = null;
            //DRO.EquipmentTypeGroup = null;
            

            var resulteq = await context.EquipmentTemplate.AsNoTracking().Where(x => x.EquipmentTemplateID == DTO.EquipmentTemplateID).FirstOrDefaultAsync();

           
            if (resulteq == null)
            {
                try
                {
                    DTO.NullableCollections = true;
                    context.EquipmentTemplate.Add(DTO);

                    await context.SaveChangesAsync();
                }
                catch
                {
                    //offline clone et
                    var DRO = (EquipmentTemplate)DTO.CloneObject();
                    DRO.NullableCollections = true;
                    context.EquipmentTemplate.Add(DRO);
                    try
                    {
                        await context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        if (throwException)
                        {
                            throw ex;
                        }
                        
                    }
                   

                }
                

                

            }

            resulteq = await context.EquipmentTemplate.AsNoTracking().Where(x => x.EquipmentTemplateID == DTO.EquipmentTemplateID).FirstOrDefaultAsync();

            if (resulteq == null && throwException)
            {
                throw new Exception("Equipment Template No Save correctly");
            }

            return DTO;
           

        }

        public async Task<EquipmentTemplate> DeleteEquipment(EquipmentTemplate EquipmentDTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            context.Entry(EquipmentDTO).State = EntityState.Deleted;
            //context.Update(id);
            await context.SaveChangesAsync();
            return EquipmentDTO;
        }


        public async Task<EquipmentTemplate> GetValidEquipment(string model, int? equipmentypegroup ,int manufacturer)
        {
            await using var context = await DbFactory.CreateDbContextAsync();


            var res =await  context.EquipmentTemplate.AsNoTracking().Where(x => x.Model.ToLower() == model.ToLower() && x.EquipmentTypeGroupID.HasValue && x.EquipmentTypeGroupID == equipmentypegroup && x.ManufacturerID == manufacturer).FirstOrDefaultAsync();


            return res;
        }


        public async Task<ResultSet<EquipmentTemplate>> GetEquipment(Pagination<EquipmentTemplate> pagination)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            bool Isgeneric = false;

            Expression<Func<EquipmentTemplate, bool>> filterQuery = null;

            

            IQueryable<EquipmentTemplate> queriable = null;

            IQueryable<EquipmentTemplate> queriable2 = null;

           if (!string.IsNullOrEmpty(pagination.EntityType) && pagination.EntityType.Contains("[") && pagination.EntityType.Contains("]"))
            {
                var ids = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(pagination.EntityType);

                queriable = context.EquipmentTemplate.AsNoTracking().Include(x => x.Manufacturer1)
                    .Where(d => d.EquipmentTypeObject.ETCalculatedAlgorithm != "Parent").Where(s => ids.Any(f => f == s.EquipmentTypeGroupID))
                    .AsNoTracking().AsQueryable(); //.Include(d => d.EquipmentTypeObject).AsQueryable();

                queriable2 = context.EquipmentTemplate.AsNoTracking().Where(d => d.EquipmentTypeObject.ETCalculatedAlgorithm != "Parent")
                    .Where(s => ids.Any(f => f == s.EquipmentTypeGroupID)).AsQueryable();


            }
            else
            {
                filterQuery = Querys.EquipmentTemplateFilterAdvance(pagination);



                queriable = context.EquipmentTemplate.AsNoTracking().Include(x => x.Manufacturer1).Where(d => d.EquipmentTypeObject.ETCalculatedAlgorithm != "Parent").AsNoTracking().AsQueryable(); //.Include(d => d.EquipmentTypeObject).AsQueryable();

                queriable2 = context.EquipmentTemplate.AsNoTracking().Where(d => d.EquipmentTypeObject.ETCalculatedAlgorithm != "Parent").AsQueryable();

            }
            

            //var test = await context.EquipmentTemplate.ToListAsync();

            

            if (pagination.Entity != null && pagination?.Entity?.IsGeneric ==true)
            {
                //Isgeneric = pagination.Entity.IsGeneric;

                queriable = context.EquipmentTemplate.Include(x => x.Manufacturer1).Include(d => d.EquipmentTypeObject).Where(x=>x.IsGeneric==true).AsQueryable();

                queriable2 = context.EquipmentTemplate.Where(x=>x.IsGeneric==true).AsQueryable();

            }


            
            var result = await queriable.PaginationAndFilterQuery<EquipmentTemplate>(pagination, queriable2, filterQuery);


            foreach (var res in result?.List)
            {
                if (res != null && res.EquipmentTypeGroupID.HasValue)
                {
                    var etg2 = await context.EquipmentTypeGroup.AsNoTracking().Where(x => x.EquipmentTypeGroupID == res.EquipmentTypeGroupID).FirstOrDefaultAsync();



                    var etg = await context.EquipmentType.AsNoTracking()
                        .Where(x => x.EquipmentTypeGroupID.Value == res.EquipmentTypeGroupID.Value).ToListAsync();

                    if (etg.Count > 0)
                    {
                        res.AditionalEquipmentTypes = etg;
                    }

                }
            }


            return result;




        }

        public async Task<bool> Exists(EquipmentTemplate EquipmentDTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var a = await context.EquipmentTemplate.AsNoTracking().Where(Querys.EquipmentTemplateExists(EquipmentDTO)).FirstOrDefaultAsync();

            if (a == null)
            {
                return false;
            }
            else
            {
                return true;

            }

        }


        public async Task<EquipmentTemplate> GetEquipmentByIDHead(EquipmentTemplate EquipmentDTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var res = await context.EquipmentTemplate.AsNoTracking().
               Where(Querys.EquipmentTemplateByID(EquipmentDTO)).FirstOrDefaultAsync();
            return res;

        }


        public async Task<EquipmentTemplate> GetEquipmentByID(EquipmentTemplate EquipmentDTO,string Component= "")
        {
            if (string.IsNullOrEmpty(Component))
            {
                Component = "EquipmentTemplate";
            }

            await using var context = await DbFactory.CreateDbContextAsync();

            var res = await context.EquipmentTemplate.AsNoTracking()
                .Include(x => x.Manufacturer1).AsNoTracking()
                .Include(b => b.Ranges).AsNoTracking()
                .Include(c => c.PieceOfEquipments).AsNoTracking()
                 //yp .Include(b=>  b.EquipmentTypeObject).AsNoTracking()
                 //.Include(b=>  b.EquipmentTypeGroup).AsNoTracking()
                 .Where(x => x.EquipmentTemplateID == EquipmentDTO.EquipmentTemplateID).FirstOrDefaultAsync();



            var res1 = await context.TestPointGroup.AsNoTracking()              
                .Include(x => x.TestPoints)
                .ThenInclude(x => x.UnitOfMeasurement)
                .ThenInclude(x => x.UnitOfMeasureBase).AsNoTracking()
                .Where(c =>
                c.EquipmentTemplateID.Equals(EquipmentDTO.EquipmentTemplateID)
                
                && c.EquipmentTemplateID == EquipmentDTO.EquipmentTemplateID).ToListAsync();

            /////GET Equipmenttypes

            if (res != null && res.EquipmentTypeGroupID.HasValue)
            {
                var etg2 = await context.EquipmentTypeGroup.AsNoTracking().Where(x => x.EquipmentTypeGroupID == res.EquipmentTypeGroupID).FirstOrDefaultAsync();

               

                var etg = await context.EquipmentType.AsNoTracking()
                    .Where(x => x.EquipmentTypeGroupID.Value == res.EquipmentTypeGroupID.Value).ToListAsync();

                if (etg.Count > 0)
                {
                    res.AditionalEquipmentTypes = etg;
                }

            }

            

            if (res != null && res.EquipmentTypeGroupID.HasValue == false && res.EquipmentTypeObject != null && !string.IsNullOrEmpty(res.EquipmentType)
                && res.EquipmentTypeObject?.ETCalculatedAlgorithm != "Parent")
            {
                var arr = res.EquipmentType.Split(',');


                if (arr?.Length > 1)
                {
                    var AditionalEquipmentTypes = new List<EquipmentType>();

                    foreach (var item in arr)
                    {
                        var it = Convert.ToInt32(item);

                        var et2 = await context.EquipmentType.AsNoTracking().Where(x => x.EquipmentTypeID == it && x.ETCalculatedAlgorithm != "Parent").FirstOrDefaultAsync();

                        if (et2 != null)
                        {
                            AditionalEquipmentTypes.Add(et2);
                        }
                    }
                    res.AditionalEquipmentTypes = AditionalEquipmentTypes;
                }
            }

            /////////////////////////////////////////////////////////////////////////////////////
           var PieceOfEquipmentRepository = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

            if (res?.AditionalEquipmentTypes?.Count > 0) {

                

                

                foreach (var itemux in res.AditionalEquipmentTypes)
                {
                    if (itemux != null && (itemux.DynamicConfiguration || itemux.DynamicConfiguration2))
                    {
                        //var ctc = await PieceOfEquipmentRepository.GetDynamicConfiguration(itemux.CalibrationTypeID, Component);

                        //itemux.CalibrationType = ctc;

                        if (res.EquipmentTypeObject != null && res.EquipmentTypeObject.DynamicConfiguration2)
                        {
                            var dynamicprop = await context.GenericCalibrationResult2.Where(x => x.ComponentID == EquipmentDTO.EquipmentTemplateID.ToString() && (x.Component == Component)).AsNoTracking().ToListAsync();
                            if (dynamicprop.Any() && dynamicprop.Count() >0)
                            {
                                res.TestPointResult = new List<GenericCalibrationResult2>();
                                res.TestPointResult.AddRange(dynamicprop);
                            }
                            
                        }
                    }
                    else
                    {
                        itemux.CalibrationType = await context.CalibrationType.AsNoTracking().Where(x => x.CalibrationTypeId == itemux.CalibrationTypeID).FirstOrDefaultAsync();

                    }
                }
                res.AditionalEquipmentTypesJSON = Newtonsoft.Json.JsonConvert.SerializeObject(res.AditionalEquipmentTypes);
            }


            if (res?.AditionalEquipmentTypes?.Count == 1)
            {
                res.EquipmentTypeObject= res.AditionalEquipmentTypes.ElementAtOrDefault(0);
                res.EquipmentTypeID = res.EquipmentTypeObject.EquipmentTypeID;
            }



            ///////
            //////
            ///////
            //////
            ///
            if (res?.EquipmentTypeObject != null && res?.EquipmentTypeObject?.CalibrationTypeID != null 
                && (res?.EquipmentTypeObject?.DynamicConfiguration==true || res?.EquipmentTypeObject?.DynamicConfiguration2==true))
            {
                var CalibrationType = await context.CalibrationType.Where(x => x.CalibrationTypeId == res.EquipmentTypeObject.CalibrationTypeID).FirstOrDefaultAsync();

                await context.Entry<CalibrationType>(CalibrationType).Collection<CalibrationSubType>(p => p.CalibrationSubTypes)
                      .LoadAsync();



                if (CalibrationType != null && CalibrationType.CalibrationSubTypes != null)


                    foreach (var item3 in CalibrationType.CalibrationSubTypes.OrderBy(x => x.Position))
                    {
                        await context.Entry<CalibrationSubType>(item3).Reference<CalibrationSubTypeView>(x => x.CalibrationSubTypeView).LoadAsync();

                        List<DynamicProperty> properties = await context.DynamicProperty.AsNoTracking().Where(x => x.CalibrationSubtype == item3.CalibrationSubTypeId).ToListAsync();

                        foreach (var itendy in properties)
                        {
                            ViewPropertyBase viewproperty = await context.ViewPropertyBase.AsNoTracking().Where(x => x.ViewPropertyID == itendy.ViewPropertyBaseID).FirstOrDefaultAsync();

                            itendy.ViewPropertyBase = viewproperty;
                        }



                        item3.DynamicPropertiesSchema = properties; 
                        item3.DynamicPropertiesObject = properties;
                    }


                foreach (var item3 in CalibrationType.CalibrationSubTypes.OrderBy(x => x.Position))
                {

                    foreach (var item4 in item3.DynamicPropertiesSchema)
                    {
                        item4.ViewPropertyBase.DynamicProperty = null;
                    }

                    if (item3?.CalibrationSubTypeView?.CalibrationSubType != null)
                    {
                        item3.CalibrationSubTypeView.CalibrationSubType = null;
                    }
                }
            }



            /////////////////////////////////////////////////
            ///YPPP quit comment to get DynamicConfiguration
            //res.AditionalEquipmentTypes =  new List<EquipmentType>(); 
            if (res != null && res.EquipmentTypeObject != null && (res.EquipmentTypeObject.DynamicConfiguration))
                {
               
                    var ctc = await PieceOfEquipmentRepository.GetDynamicConfiguration(res.EquipmentTypeObject.CalibrationTypeID);

                     res.EquipmentTypeObject.CalibrationType = ctc;

                 if (res.EquipmentTypeObject.DynamicConfiguration2)
                 {
                       res.TestPointResult = await context.GenericCalibrationResult2.Where(x => x.ComponentID == EquipmentDTO.EquipmentTemplateID.ToString() && (x.Component == "EquipmentTemplate")).AsNoTracking().ToListAsync();

                  } 
                }
                else if(res.EquipmentTypeObject != null )
                {
                   res.EquipmentTypeObject.CalibrationType = await context.CalibrationType.AsNoTracking().Where(x => res.EquipmentTypeObject != null && x.CalibrationTypeId == res.EquipmentTypeObject.CalibrationTypeID).FirstOrDefaultAsync(); 

               }

                res.TestGroups = res1;
            
            return res;
        }
        //&& Querys.EquipmentTemplateByID(EquipmentDTO)

        //public async Task<EquipmentTemplate> GetEquipmentXName(EquipmentTemplate EquipmentDTO)
        //{
        //     await using var context = await DbFactory.CreateDbContextAsync();  
        //    var result = await context.EquipmentTemplate.Where(x =>x.EquipmentTypeGroupID== EquipmentDTO.EquipmentTypeGroupID && x.Model == EquipmentDTO.Model && x.ManufacturerID == EquipmentDTO.ManufacturerID).AsNoTracking().ToArrayAsync();
        //    return result.FirstOrDefault();
            
        //}

        private EquipmentTemplate PrepareDTO(EquipmentTemplate EquipmentDTO)
        {


            List<EquipmentTemplate> lst1 = new List<EquipmentTemplate>();

            lst1.Add(EquipmentDTO);

            var EquipmentDTOtemp = lst1.ToList();

            var eto = EquipmentDTOtemp.ElementAtOrDefault(0);

            if (eto.TestGroups == null || eto.TestGroups.Count == 0)
            {
                eto.TestGroups = null;
            }
            else
            {

                eto.TestGroups.ToList().ForEach(item =>
                {
                    if (item.UnitOfMeasurementOut != null && item?.UnitOfMeasurementOut?.UnitOfMeasureID > 0)
                    {
                        item.OutUnitOfMeasurementID = item.UnitOfMeasurementOut.UnitOfMeasureID;
                    }
                    item.UnitOfMeasurementOut = null;

                    item.TestPoints.ToList().ForEach(item2 =>
                    {
                        item2.UnitOfMeasurementOut = null;
                        item2.UnitOfMeasurement = null;
                        item2.TestPointGroup = null;

                    });

                }

                    );
            }
            eto.Manufacturer1 = null;
            eto.EquipmentTypeObject = null;
            return eto;
        }

       


        public async Task<EquipmentTemplate> UpdateEquipment(EquipmentTemplate EquipmentDTO, bool isThread = false)
        {


            await using var context = await DbFactory.CreateDbContextAsync();

           

                var DTO = PrepareDTO(EquipmentDTO);


                if (DTO.TestPointResult != null && DTO.TestPointResult.Count > 0)
                {

                    var groupedCalibrations = DTO.TestPointResult
                        .GroupBy(item => item.CalibrationSubTypeId)
                        .Where(group => group.Any(item => item.Updated != 0))
                        .SelectMany(group => group)
                        .ToList();


                    ICollection<GenericCalibrationResult2> genericCalibrationResult2s = new List<GenericCalibrationResult2>();

                    foreach (var item in groupedCalibrations)
                    {
                        genericCalibrationResult2s.Add(item);
                    }

                    if (genericCalibrationResult2s.Count == 0)
                    {
                        var fas = DTO.TestPointResult
                       .Where(x => x.GenericCalibration2 != null && ((x.GenericCalibration2.WeightSets != null && x.GenericCalibration2.WeightSets.Count > 0)
                       || (x.GenericCalibration2.Standards != null && x.GenericCalibration2.Standards.Count > 0))).FirstOrDefault();

                        if (fas != null)
                        {
                            genericCalibrationResult2s = DTO.TestPointResult;
                        }


                    }


                    var workOrderDetailRepository = new WODRepositoryEF<TContext>(DbFactory);

                    
                        await workOrderDetailRepository.SaveCalibrationTypes2<GenericCalibration2, GenericCalibrationResult2>("EquipmentTemplate", DTO.EquipmentTemplateID.ToString(),
                       genericCalibrationResult2s, workOrderDetailRepository.FormatGenericCalibration2);


                    

                    //await SaveCalibrationTypes2<GenericCalibration2, GenericCalibrationResult2>(Component, DTO.WorkOrderDetailID.ToString(), DTO.BalanceAndScaleCalibration.GenericCalibration2, FormatGenericCalibration2);

                    //await SaveCalibrationTypes2<GenericCalibration2, GenericCalibrationResult2>(Component, DTO.WorkOrderDetailID.ToString(), genericCalibrationResult2s, FormatGenericCalibration2);


                }

                DTO.TestPointResult = null;








                var ccc = await context.TestPointGroup.AsNoTracking()
                        .Include(x => x.TestPoints).Where(x => x.EquipmentTemplateID == EquipmentDTO.EquipmentTemplateID).ToListAsync();

                if(DTO?.TestGroups == null && ccc.Count > 0)
                {
                    foreach (var i in ccc)
                    {
                        foreach (var ss in i.TestPoints)
                        {

                        }
                        }
                 }


                //if (DTO?.TestGroups != null && DTO?.TestGroups.ElementAtOrDefault(0)?.TestPoints != null)
                if (ccc.Count > 0)
                { 

                    foreach (var i in ccc)
                    {
                        if (i.EquipmentTemplateID > 0)
                        {

                            foreach(var ss in i.TestPoints)
                            {
                                ss.TestPointGroup = null;
                                ss.UnitOfMeasurementOut = null;
                                ss.UnitOfMeasurement = null;
                                try
                                {
                                    var lin =await  context.Linearity.Where(x => x.TestPointID == ss.TestPointID).ToListAsync();

                                    foreach (var ll in lin)
                                    {
                                        ll.TestPointID = null;
                                        context.Linearity.Update(ll);
                                        await context.SaveChangesAsync();
                                    }

                                    var rep = await context.Repeatability.Where(x => x.TestPointID == ss.TestPointID).ToListAsync();

                                    foreach (var ll in rep)
                                    {
                                        ll.TestPointID = null;
                                        context.Repeatability.Update(ll);
                                        await context.SaveChangesAsync();
                                    }


                                    var ecce = await context.Eccentricity.Where(x => x.TestPointID == ss.TestPointID).ToListAsync();

                                    foreach (var ll in ecce)
                                    {
                                        ll.TestPointID = null;
                                        context.Eccentricity.Update(ll);
                                        await context.SaveChangesAsync();
                                    }

                                    context.Remove(ss);
                                    await context.SaveChangesAsync();


                                }
                                catch(Exception ex)
                                {

                                }
                                
                            }

                         
                    }

                    }
                

                if (DTO.TestGroups != null &&  DTO?.TestGroups.ElementAtOrDefault(0)?.TestPoints != null)
                    {
                        var tgt = DTO.TestGroups.ElementAtOrDefault(0);

                        if (tgt.TestPoitGroupID > 0)
                        {
                            int cont = 0;
                            foreach (var item2 in tgt.TestPoints)
                            {
                                if (item2.UnitOfMeasurement != null)
                                    item2.UnitOfMeasurement = null;

                                if (item2.UnitOfMeasurementOut != null)
                                    item2.UnitOfMeasurementOut = null;
                                item2.TestPointGroupTestPoitGroupID = tgt.TestPoitGroupID;
                              //  item2.TestPointID = 0;
                                //item2.Position = cont;
                                context.Add(item2);
                                await context.SaveChangesAsync();
                                cont++;
                            }

                            DTO.TestGroups.ElementAtOrDefault(0).TestPoints = null;

                            if (ccc.Count > 0)
                            {
                               tgt.EquipmentTemplateID = DTO.EquipmentTemplateID;
                                context.TestPointGroup.Update(tgt);
                            }
                            else
                            {
                                context.TestPointGroup.Add(tgt);
                            }
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            if (ccc.Count > 0)
                            {
                            tgt.EquipmentTemplateID = DTO.EquipmentTemplateID;
                            context.TestPointGroup.Update(tgt);
                            }
                            else
                            {
                                context.TestPointGroup.Add(tgt);
                            }
                            await context.SaveChangesAsync();

                        }

                    }


                }
                else
                {
                    if (DTO.TestGroups != null && DTO?.TestGroups.ElementAtOrDefault(0)?.TestPoints != null)
                    {
                        var tgt = DTO.TestGroups.ElementAtOrDefault(0);

                        tgt.EquipmentTemplateID = DTO.EquipmentTemplateID;
                        tgt.TypeID = "Linearity";


                        if (tgt.TestPoitGroupID > 0)
                        {
                            int cont = 0;
                            foreach (var item2 in tgt.TestPoints)
                            {
                                if (item2.UnitOfMeasurement != null)
                                    item2.UnitOfMeasurement = null;

                                if (item2.UnitOfMeasurementOut != null)
                                    item2.UnitOfMeasurementOut = null;
                                item2.TestPointGroupTestPoitGroupID = tgt.TestPoitGroupID;
                                //item2.TestPointID = 0;
                                //item2.Position = cont;
                                context.Add(item2);
                                await context.SaveChangesAsync();
                                cont++;
                            }

                            DTO.TestGroups.ElementAtOrDefault(0).TestPoints = null;

                            if (ccc.Count > 0)
                            {
                            tgt.EquipmentTemplateID = DTO.EquipmentTemplateID;
                            context.TestPointGroup.Update(tgt);
                            }
                            else
                            {
                                context.TestPointGroup.Add(tgt);
                            }
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            if (ccc.Count > 0)
                            {
                            tgt.EquipmentTemplateID = DTO.EquipmentTemplateID;
                            context.TestPointGroup.Update(tgt);
                            }
                            else
                            {
                                context.TestPointGroup.Add(tgt);
                            }
                            await context.SaveChangesAsync();

                        }

                    }
                }


                var rrr = await context.RangeTolerance
                          .Where(x => x.EquipmentTemplateID == DTO.EquipmentTemplateID).ToListAsync();
                
                if(DTO.Ranges != null && rrr.Count > 0)
                {
                    var result = rrr.Where(p => DTO.Ranges.All(p2 => p2.RangeToleranceID != p.RangeToleranceID));

                    foreach (var td in result)
                    {
                        context.Remove(td);
                        await context.SaveChangesAsync();
                    }
                }
                else if(rrr.Count > 0)
                {
                    foreach (var td in rrr)
                    {
                        context.Remove(td);
                        await context.SaveChangesAsync();
                    }
                }
               


                if (DTO.Ranges != null && DTO.Ranges.Count > 0)
                {

                    foreach (var t in DTO.Ranges)
                    {
                        t.EquipmentTemplateID = DTO.EquipmentTemplateID;
                        var rr = await context.RangeTolerance
                            .Where(x => x.EquipmentTemplateID == DTO.EquipmentTemplateID && x.RangeToleranceID ==t.RangeToleranceID).FirstOrDefaultAsync();

                        if(rr != null)
                        {
                            context.Update(t);

                        }
                        else
                        {
                            context.Add(t);
                        }
                        await context.SaveChangesAsync();

                    }
                }
                if (DTO?.AditionalEquipmentTypes?.Count > 0)
                {
                    DTO.AditionalEquipmentTypesJSON = Newtonsoft.Json.JsonConvert.SerializeObject(DTO.AditionalEquipmentTypes);
                }
                DTO.Ranges = null;
                DTO.Manufacturer1 = null;
                DTO.TestGroups = null;
                DTO.AditionalEquipmentTypesJSON  = null;
                DTO.AditionalEquipmentTypes = null;
                DTO.EquipmentTypeObject = null;
                context.EquipmentTemplate.Update(DTO);

                await context.SaveChangesAsync();

                return EquipmentDTO;
           }


        public async Task<IEnumerable<TestPointGroup>> GetTespointGroup(EquipmentTemplate EquipmentDTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TestPoint>> GetTespoint(TestPointGroup DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var res = await context.TestPointGroup.Where(x => x.TestPoitGroupID == DTO.TestPoitGroupID).ToArrayAsync();

            List<TestPoint> lst = new List<TestPoint>();

            foreach (var item in res)
            {

                lst.AddRange(item.TestPoints);
            }

            return lst;
        }

        public async Task<IEnumerable<TestPointGroup>> GetTespointByEquipment(EquipmentTemplate EquipmentDTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var a = await context.EquipmentTemplate.Where(x => x.EquipmentTemplateID == EquipmentDTO.EquipmentTemplateID).FirstOrDefaultAsync();

            //foreach(var item in a)
            //{
            //    var result = await GetTespoint(item);

            //    item.TestPoints = result.ToArray();

            //}

            return a.TestGroups;
        }

        public async Task<TestPointGroup> InsertTestPointGroup(TestPointGroup DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            context.TestPointGroup.Add(DTO);
            await context.SaveChangesAsync();
            return DTO;

        }

        public async Task<TestPoint> InsertTestPoint(TestPoint DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            context.TestPoint.Add(DTO);
            await context.SaveChangesAsync();
            return DTO;

        }
        #endregion
        #region Status


        public async Task<IEnumerable<Status>> GetAllStatus()
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            return await context.Status.OrderBy(x => x.Name).ToListAsync();
        }


        #endregion
        #region Rol


        public async Task<IEnumerable<Rol>> GetAllRoles()
        {
            await using var context = await DbFactory.CreateDbContextAsync();            
            
            
            return await context.Rol.AsNoTracking().Where(x=>x.IsDelete==false).OrderBy(x => x.Name).ToListAsync();
        }


        #endregion

        #region Address
        public async Task<Address> CreateAddress(Address DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            context.Address.Add(DTO);
            context.SaveChanges();
            return DTO;
        }

        public async Task<IEnumerable<Address>> GetAllAddress()
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            return await context.Address.OrderBy(x => x.Name).AsNoTracking().ToListAsync();
        }

        public async Task<Address> DeleteAddress(int id)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            Address _address = await context.Address.FirstOrDefaultAsync(sc => sc.AddressId == id);
           
            context.Entry(_address).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            return _address;
        }


        public async Task<Address> UpdateAddress(Address DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            context.Entry(DTO).State = EntityState.Modified;

            return DTO;
        }

        public Task<IEnumerable<Rol>> GetAllRol()
        {
            throw new NotImplementedException();
        }

        public async Task<Rol> DeleteRole(Rol rol)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            
            
            var a = await context.Rol.AsNoTracking().Where(x => x.RolID == rol.RolID).FirstOrDefaultAsync();

            context.Rol.Remove(a);  

            await context.SaveChangesAsync();   

            return rol;
        }

        public async Task<Rol> AddRol(Rol rol)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var a = await context.Rol.AsNoTracking().Where(x => x.RolID == rol.RolID || x.Name.ToLower()==rol.Name.ToLower()).FirstOrDefaultAsync();



            if (a == null)
            {
                context.Rol.Add(rol);
            }
            else
            {
                context.Rol.Update(rol);
            }
            

            await context.SaveChangesAsync();

            return rol;
        }


        public async Task AddRols(ICollection<Rol> rols)
        {
            foreach(var item in rols)
            {
                await AddRol(item);
            }
        }

        public async  Task<Component> DeleteComponent(Component Component)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            
            var ccc = new Component();

                    ccc.CopyPropertiesFrom(Component);

            context.Component.Remove(ccc);

            await context.SaveChangesAsync();
            return Component;


        }

        public async Task<ResultSet<Component>> CreateComponents(ICollection<Component> Component)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var cc = await context.Component.AsNoTracking().ToListAsync();


            foreach (var item in Component)
            {
                if (!string.IsNullOrEmpty(item.Route))
                {
                   var c= cc.Where(x => x.Route.ToLower() == item.Route.ToLower()).FirstOrDefault();

                    var ccc = new Component();

                    ccc.CopyPropertiesFrom(item);
                    if (c == null)
                    {
                        context.Component.Add(ccc);
                    }
                    else
                    {
                        //context.Component.Update(item);
                    }
                }
            }
            await context.SaveChangesAsync();

            ResultSet<Component> sss = new ResultSet<Component>();
            var a= await GetAllComponents();

            return a;
        }


        public async Task<ResultSet<CustomSequence>> CustomSequences()
        {

            await using var context = await DbFactory.CreateDbContextAsync();
            var c = await context.CustomSequence.AsNoTracking().ToListAsync();
            ResultSet<CustomSequence> sss = new ResultSet<CustomSequence>();


            sss.List = c;

            return sss;

        }



        public async Task<ResultSet<Component>> GetAllComponents()
        {

            await using var context = await DbFactory.CreateDbContextAsync();
            var c = await context.Component.AsNoTracking().ToListAsync();


            ResultSet<Component> sss = new ResultSet<Component>();
            List<Component> result = new List<Component>(); 
            foreach (var item in c)
            {
                var cccc = new Component();
                cccc.CopyPropertiesFrom(item);
                result.Add(cccc);
            }

            sss.List = result;

            return sss;

        }

        public async  Task<Component> GetComponentByRoute(string route)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var c = await context.Component.AsNoTracking().Where(x => x.Route == route).FirstOrDefaultAsync();

            if(c!= null)
            {
                 var cccc = new Component();
                cccc.CopyPropertiesFrom(c);
                return cccc;
            }
            else
            {
                return null;    
            }
           

            
        }




        //public async Task UpdateManufacturer(Manufacturer DTO)
        //{
        //      context.Entry(DTO).State = EntityState.Modified;

        //}

        //public async Task<EquipmentTemplate> Update(EquipmentTemplate EquipmentDTO)
        //{
        //    context.EquipmentTemplate.Add(EquipmentDTO);
        //    await context.SaveChangesAsync();
        //    return EquipmentDTO;
        //}

        #endregion


        public async Task<bool> RemoveNotes<TSource>(int Id,TSource DTO,int Source) where TSource : INote
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            if (Source == 1)
            {

                var notes = await context.Note.AsNoTracking().Where(x => x.EquipmnetTypeId == Id).ToListAsync();

                if(notes.Count>0 && (DTO.Notes == null || DTO?.Notes?.Count==0))
                {
                    context.RemoveRange(notes);
                    await context.SaveChangesAsync();   
                }
                if(notes.Count > 0 && DTO?.Notes?.Count > 0)
                {

                    foreach (var item in notes)
                    {
                        var notetmp = DTO.Notes.Where(x=>x.NoteId==item.NoteId).FirstOrDefault();

                        if (notetmp == null)
                        {
                            context.Remove(item);
                            await context.SaveChangesAsync();
                        }


                    }



                }


            }

            if (Source == 2)
            {

                var notes = await context.Note.AsNoTracking().Where(x => x.TestCodeID == Id).ToListAsync();

                if (notes.Count > 0 && (DTO.Notes == null || DTO?.Notes?.Count == 0))
                {
                    context.RemoveRange(notes);
                    await context.SaveChangesAsync();
                }
                if (notes.Count > 0 && DTO?.Notes?.Count > 0)
                {

                    foreach (var item in notes)
                    {
                        var notetmp = DTO.Notes.Where(x => x.NoteId == item.NoteId).FirstOrDefault();

                        if (notetmp == null)
                        {
                            context.Remove(item);
                            await context.SaveChangesAsync();
                        }


                    }



                }


            }



            return true;

        }


            public async Task<Note> SaveNotes(Note DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var  note= await context.Note.AsNoTracking().Where(x=>x.NoteId==DTO.NoteId).FirstOrDefaultAsync();

            if(note!= null) { 
            
            context.Note.Update(DTO);

            }
            else
            {
                context.Note.Add(DTO);
            }

            await context.SaveChangesAsync();   

            return null;

        }


        public async Task<List<Note>> GetNotes(int Id, int Source = 1)

        {

            await using var context = await DbFactory.CreateDbContextAsync();
            IQueryable<Note> query = null;

            if (Source == 2)
            {
                query = context.Set<Note>().AsNoTracking().Where(x => x.TestCodeID == Id);
            }

            if (Source == 1)
            {
                query = context.Set<Note>().AsNoTracking().Where(x => x.EquipmnetTypeId == Id);
            }

            try
            {
                var forces = await query.ToListAsync();




                return forces;
            }catch(Exception ex) 
            {
                throw ex;
            
            }

           



            

        }

       

        public async Task<User> GetUserByUserName(string id)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var result = await context.User.Include(x => x.UserRoles).
                ThenInclude(x => x.Rol).Where(x => x.UserName == id).AsNoTracking().
                Include(b => b.TechnicianCodes).ToArrayAsync();
            List<TechnicianCode> tc = new List<TechnicianCode>();
            foreach (var item in result)
            {
                foreach (var tech in item.TechnicianCodes)
                {
                    tech.User = null;
                }

            }
            return result.FirstOrDefault();
        }

        public  async Task<Procedure> GetProcedureXName(Procedure DTO)
        {
                await using var context = await DbFactory.CreateDbContextAsync();
                var result = await context.Procedure.Where(x => x.Name == DTO.Name).AsNoTracking().ToArrayAsync();
                return result.FirstOrDefault();
            

        }

         public async Task<ResultSet<Manufacturer>> GetAllManufacturers()
        {

            Pagination<Manufacturer> pagination = new Pagination<Manufacturer>();



            //return await context.Manufacturer.OrderBy(x => x.Name).ToListAsync();
            await using var context = await DbFactory.CreateDbContextAsync();
            var filterQuery = Querys.ManufacturerFilter(pagination.Filter);

            var queriable = context.Manufacturer;//context.PieceOfEquipment.Include(x => x.EquipmentTemplate).Include(d => d.Customer).AsQueryable();


            var simplequery = context.Manufacturer;
            //if (pagination.Filter == null)
            //{
            //    pagination.Filter = "Delete";
            //}
            //var result = await QueryableExtensions.PaginationAndFilterQuery<PieceOfEquipment>(exprTree, pagination, context.PieceOfEquipment, queriable);
            var result = await queriable.PaginationAndFilterQuery<Manufacturer>(pagination, simplequery, filterQuery);


            return result;
        }
        public async Task<CalibrationType> CreateCalibrationType(CalibrationType DTO)
        {
            using (var context = await DbFactory.CreateDbContextAsync())
            {
                var existingType = await context.CalibrationType
                    .FirstOrDefaultAsync(c => c.CalibrationTypeId == DTO.CalibrationTypeId);

                if (existingType != null)
                {
                    // Update existing entity
                    context.Entry(existingType).CurrentValues.SetValues(DTO);
                }
                else
                {
                    // Add new entity
                    await context.CalibrationType.AddAsync(DTO);
                }

                await context.SaveChangesAsync();
                return DTO;
            }
        }

        public async Task<Status> CreateStatus(Status DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            context.Status.Add(DTO);
            await context.SaveChangesAsync();
            return DTO;
        } 
        
     public async Task<ResultSet<EquipmentTypeGroup>> GetEquipmentTypeGroups()
        {
        	await using var context = await DbFactory.CreateDbContextAsync();
        	
            var nrty = await context.EquipmentTypeGroup.AsNoTracking().ToListAsync();

            ResultSet<EquipmentTypeGroup> res = new ResultSet<EquipmentTypeGroup>();

            res.List= nrty; 

            return res; 
       }
        #region CMCValues
        public async Task<List<CMCValues>> GetCMCValuesByCalibrationType(CalibrationType DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var res = await context.CMCValues.AsNoTracking().Where(x=>x.CalibrationTypeId == DTO.CalibrationTypeId).ToListAsync();

            return res;
        }

        public async Task<CMCValues> GetCMCValuesByID(CMCValues DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var res = await context.CMCValues.AsNoTracking().Where(x => x.CalibrationTypeId == DTO.CalibrationTypeId).FirstOrDefaultAsync();

            return res;
        }
        public async Task<CalibrationType> GetCalibrationTypeByID(CalibrationType DTO)
        {
            if (DTO == null)
            {
                throw new ArgumentNullException(nameof(DTO), "DTO cannot be null");
            }

            await using var context = await DbFactory.CreateDbContextAsync();

            var res = await context.CalibrationType.AsNoTracking()
                .Include(x => x.CalibrationSubTypes).AsNoTracking()
                .Where(x => x.CalibrationTypeId == DTO.CalibrationTypeId).FirstOrDefaultAsync();

            if (res == null)
            {
                return null;
            }

            var cmc = await GetCMCValuesByCalibrationType(res);

            if (cmc != null  && cmc.Count() > 0)
            {
                res.CMCValues = cmc;
            }

            return res;
        }
        public async Task<CalibrationType> InsertCMCValue(CalibrationType DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var cmc = await context.CMCValues.Where(x => x.CalibrationTypeId == DTO.CalibrationTypeId).ToListAsync();
            if (cmc!=null && cmc.Count()>0)
            {
                context.CMCValues.RemoveRange(cmc);
                await context.SaveChangesAsync();
            }

            if (DTO != null && DTO.CMCValues != null && DTO.CMCValues.Count() > 0)
            {
                foreach (var item in DTO.CMCValues)
                {
                    item.CMCValueId = 0;//NumericExtensions.GetUniqueID(item.CMCValueId);
                    item.CalibrationTypeId = DTO.CalibrationTypeId;
                    context.CMCValues.Add(item);
                }

               

            }
            await context.SaveChangesAsync();

            
            return DTO;
        }

        public async Task<CMCValues> DeleteCMCValue(CMCValues DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            CMCValues cmc = await context.CMCValues.FirstOrDefaultAsync(sc => sc.CMCValueId == DTO.CMCValueId);

            //context.EquipmentType.Remove(equipmentType);
            context.Entry(cmc).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            return cmc;

        }

        public async Task<CMCValues> UpdateCMCValue(CMCValues DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
           
            context.CMCValues.Update(DTO);
            await context.SaveChangesAsync();
            return DTO;
        }
        #endregion
    }
}
