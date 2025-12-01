using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.BusinessExceptions;
using CalibrationSaaS.Domain.Repositories;
using Helpers;
using Helpers.Controls.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.UseCases
{
    

    public partial class BasicsUseCases
    {
        

        #region EquipmetTemplate

        public async Task<EquipmentTemplate> CreateEquipment(EquipmentTemplate EquipmentDTO,bool isThread=false)
        {

                      
                var a = await basicsRepository.Exists(EquipmentDTO);
                EquipmentTemplate newET = new EquipmentTemplate();
                var equipmentTypeId = 0;
                var etoTestCode = new EquipmentType();
            EquipmentDTO.EquipmentTypeID = null;
                        
            if (EquipmentDTO != null && EquipmentDTO.EquipmentTypeID == 501)
            {

               // EquipmentDTO.EquipmentTypeObject.ETCalculatedAlgorithm = "Parent";

                var selectType1 = EquipmentDTO.TestPointResult.Where(x => x.KeyObject != null && x.CalibrationSubTypeId == 505 ).FirstOrDefault();
                dynamic selectType2 = JsonConvert.DeserializeObject(selectType1.ExtendedObject);
                int selectType = Convert.ToInt16(selectType2.ValueType[2]);




                //var selectSeries1 = EquipmentDTO.TestPointResult.Where(x => x.KeyObject != null && x.CalibrationSubTypeId == 506 ).FirstOrDefault();

                //if(selectSeries1 != null) 
                //{ 

                //}

                //dynamic selectSeries2 = JsonConvert.DeserializeObject(selectSeries1.ExtendedObject);
                //int selectSeries = Convert.ToInt16(selectSeries2.ValueSeries[2]);

                var selectSeries1 = EquipmentDTO.TestPointResult.Where(x => x.CalibrationSubTypeId == 506 && x.KeyObject != null).FirstOrDefault();
               
                dynamic selectSeries2 = JsonConvert.DeserializeObject(selectSeries1.ExtendedObject);
                int selectSeries = Convert.ToInt16(selectSeries2.ValueSeries[2]);


                //Validation for get Grid in wod
                if (selectType == 5 && selectSeries == 3)
                    equipmentTypeId = 502;
                else if (selectType == 7 && selectSeries == 5)
                    equipmentTypeId = 503;
                else if (selectType == 4 && selectSeries == 1)
                    equipmentTypeId = 504;
                else if (selectType == 6 && selectSeries == 5)
                    equipmentTypeId = 505;
                else if (selectType == 6 && selectSeries == 8)
                    equipmentTypeId = 506;
                else if (selectType == 3 && selectSeries == 4)
                    equipmentTypeId = 507;
                else if (selectType == 3 && selectSeries == 3)
                    equipmentTypeId = 508;
                else if (selectType == 3 && selectSeries == 9)
                    equipmentTypeId = 509;
                else if (selectType == 3 && selectSeries == 1)
                    equipmentTypeId = 511;
                else if (selectType == 3 && selectSeries == 2)
                    equipmentTypeId = 512;
                else
                    equipmentTypeId = 0;


                if (equipmentTypeId != 0)
                {


                    EquipmentType etype = new EquipmentType();
                    etype.EquipmentTypeID = equipmentTypeId;
                    //update Parent
                    newET.EquipmentTypeID = equipmentTypeId;
                    newET.Capacity = EquipmentDTO.Capacity;
                    newET.EquipmentTemplateID = NumericExtensions.GetUniqueID(newET.EquipmentTemplateID);
                    newET.UnitofmeasurementID = EquipmentDTO.UnitofmeasurementID;
                    newET.ManufacturerID = EquipmentDTO.ManufacturerID;
                    newET.Model = EquipmentDTO.Model.ToString();
                    newET.Manufacturer = EquipmentDTO.Manufacturer;
                    newET.DeviceClass = EquipmentDTO.DeviceClass;
                    newET.StatusID = EquipmentDTO.StatusID;
                    //rev newET.EquipmentTypeObject = await GetEquipmentTypeByID(etype);

                    newET.EquipmentTemplateParent = EquipmentDTO.EquipmentTemplateID;
                   // etoTestCode = newET.EquipmentTypeObject;
                    IDictionary<string, object> dic2 = new Dictionary<string, object>();
                    dic2["idParent"] = EquipmentDTO.EquipmentTemplateID.ToString();

                    string json = JsonConvert.SerializeObject(dic2);
                    //newET.EquipmentTypeObject.ETCalculatedAlgorithm = EquipmentDTO.EquipmentTemplateID.ToString();

                   
                    Pagination<TestCode> pagination = new Pagination<TestCode>();
                    pagination.Show = 10000;
                    pagination.Page = 1;


                    var result = await wodRepository.GetTestCodes(pagination);
                    var testCodeExist = result.List.Where(x => x.EquipmentTypeID == newET.EquipmentTypeID);

                    if ((testCodeExist == null || testCodeExist.Count() == 0) && equipmentTypeId != 0)
                    {


                        TestCode testCode = new TestCode();
                        testCode.Code = "Test Code-" + etoTestCode.Name;
                        
                        testCode.RangeMIn = 0;
                        testCode.RangeMax = 0;
                        //testCode.UnitOfMeasureID = 2102;
                        testCode.Description = etoTestCode.Name;
                        testCode.CalibrationTypeID = etoTestCode.CalibrationTypeID;
                        testCode.CalibrationType = etoTestCode.CalibrationType;
                        testCode.EquipmentTypeID = etoTestCode.EquipmentTypeID;
                        
                        await wodRepository.CreateTestCode(testCode);
                    }


                    if (!a)
                    {
                        await CreateEquipment(newET,true);

                    }
                    else
                    {
                        var r = await basicsRepository.UpdateEquipment(newET,true);

                    }

                    if (a && EquipmentDTO.EquipmentTemplateID > 0)
                    {
                        var r = await basicsRepository.UpdateEquipment(EquipmentDTO);
                        if (EquipmentDTO.AddFromPoe != true)
                        {
                            return await GetEquipmentByID(EquipmentDTO);
                        }
                        else
                        {
                            return await GetEquipmentByID(newET);
                        }
                        //throw new ExistingException("Equipment Template alredy exists");//ExistingRecordException<EquipmentTemplate>("Equipment Template alredy exists", null, EquipmentDTO);
                    }
                    else if (!a)
                    {
                        var r = await basicsRepository.CreateEquipment(EquipmentDTO);


                        if (EquipmentDTO.AddFromPoe != true)
                        {
                            return await GetEquipmentByID(EquipmentDTO);
                        }
                        else
                        {
                            return await GetEquipmentByID(newET);
                        }
                    }

                }

                else
                {
                    EquipmentDTO.EquipmentTemplateID = 0;

                }

            }

           
            else
            {
                if (a && EquipmentDTO.EquipmentTemplateID > 0)
                {
                    var r = await basicsRepository.UpdateEquipment(EquipmentDTO);
                    
                        return await GetEquipmentByID(EquipmentDTO);
                    
                    //throw new ExistingException("Equipment Template alredy exists");//ExistingRecordException<EquipmentTemplate>("Equipment Template alredy exists", null, EquipmentDTO);
                }
                else if (!a)
                {
                   EquipmentDTO.EquipmentTemplateID = NumericExtensions.GetUniqueID();

                    var r = await basicsRepository.CreateEquipment(EquipmentDTO);


                    return await GetEquipmentByID(EquipmentDTO);
                   
                }

            }

        
            return EquipmentDTO;

        }


        public async Task<EquipmentTemplate> Update(EquipmentTemplate EquipmentDTO)
        {

            var r = await basicsRepository.UpdateEquipment(EquipmentDTO);
            return r;


          


        }



        public async Task<ResultSet<EquipmentTemplate>> GetEquipment(Pagination<EquipmentTemplate> pagination)
        {

            var result= await basicsRepository.GetEquipment(pagination);

            return result;


        }



        public async Task<bool> Exists(EquipmentTemplate EquipmentDTO)
        {

            var result = await basicsRepository.Exists(EquipmentDTO);

            //return result;

            return result;


        }


        public async Task<EquipmentTemplate> DeleteEquipment(EquipmentTemplate EquipmentDTO)
        {
            

            return await basicsRepository.DeleteEquipment(EquipmentDTO);


        }

        public async Task<EquipmentTemplate> GetEquipmentByID(EquipmentTemplate EquipmentDTO,string Component="")
        {
            
                var result = await basicsRepository.GetEquipmentByID(EquipmentDTO, Component);

                return result;   

        }

        public Task<bool> Save()
        {
            throw new NotImplementedException();
        }

        public async Task<EquipmentTemplate> GetEquipmentXName(EquipmentTemplate DTO)
        {

            
            var result = await this.basicsRepository.GetValidEquipment(DTO.Model,DTO.EquipmentTypeGroupID,DTO.ManufacturerID);

            if(result== null)
            {
                return new EquipmentTemplate();
            }
            return result;

            //if ((result != null && result.EquipmentTypeGroupID==DTO.EquipmentTypeGroupID && result.EquipmentTemplateID == DTO.EquipmentTemplateID && result.ManufacturerID == DTO.ManufacturerID && result.Model == DTO.Model) || result == null)
            //{
            //    return new EquipmentTemplate();
            //}
            //else
            //{
            //    return result;
            //}
        }

        #endregion

    }
}
