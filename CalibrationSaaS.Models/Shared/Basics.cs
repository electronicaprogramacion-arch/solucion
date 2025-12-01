using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using Helpers;
using Helpers.Controls;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalibrationSaaS.Domain.Aggregates.Shared.Basic
{
    public class AppState
    {

        public event Action OnChange;
        public string PathS { get; set; }

        //public List<UnitOfMeasure> GetUnitOfMeasureByCalibrationType(EquipmentTemplate EquipmentTemplate)
        //{
        //    if(UnitofMeasureList== null)
        //    {
        //        UnitofMeasureList= new List<UnitOfMeasure>();   
        //    }

        //    if(EquipmentTemplate?.EquipmentTypeObject != null && EquipmentTemplate?.EquipmentTypeObject?.CalibrationTypeID > 0)
        //    {
        //        var b = UnitofMeasureList.Where(x => x.Type.CalibrationTypeID == EquipmentTemplate.EquipmentTypeObject.CalibrationTypeID).ToList();

        //        return b;
        //    }

        //    return UnitofMeasureList;

        //}
        public List<UnitOfMeasure> GetUoMByTypes(int[] calibrationTypes)
        {
            if (UnitofMeasureList == null)
            {
                UnitofMeasureList = new List<UnitOfMeasure>();
            }

            if (calibrationTypes.Count() > 0)
            {
                var b = UnitofMeasureList.Join(calibrationTypes, e => e.TypeID, id => id, (e, id) => e).ToList();//Where(x => x.Type.CalibrationTypeID == calibrationType).ToList();

                return b;
            }

            return UnitofMeasureList;

        }


        //public List<UnitOfMeasure> GetUoMByCalibrationType(int calibrationType)
        //{
        //    if (UnitofMeasureList == null)
        //    {
        //        UnitofMeasureList = new List<UnitOfMeasure>();
        //    }

        //    if (calibrationType > 0)
        //    {
        //        var b = UnitofMeasureList.Where(x => x.Type.CalibrationTypeID == calibrationType).ToList();

        //        return b;
        //    }

        //    return UnitofMeasureList;

        //}

        public List<UnitOfMeasure> GetUoMByCalibrationTypeObj(CalibrationType CalibrationType)
        {
            if (UnitofMeasureList == null)
            {
                UnitofMeasureList = new List<UnitOfMeasure>();
            }

            //if (calibrationType > 0)
            //{
            //    var b = UnitofMeasureList.Where(x => x.Type.CalibrationTypeID == calibrationType).ToList();

            //    return b;
            //}

            //List<int?> lst = new List<int?>();
            //if (CalibrationType?.CalibrationSubTypes?.Count > 0)
            //{
            //    var cs = CalibrationType.CalibrationSubTypes;

            //    foreach (var item in cs)
            //    {
            //        lst.Add(item.UnitOfMeasureTypeID);
            //    }



            //    if (lst.Count() > 0)
            //    {
            //        var b = UnitofMeasureList.Join(lst, e => e.TypeID, id => id, (e, id) => e).ToList();//Where(x => x.Type.CalibrationTypeID == calibrationType).ToList();

            //        return b;
            //    }

            //}


            List<int?> lst = new List<int?>();
            List<CalibrationSubType> cs = null;

            if (CalibrationType?.CalibrationSubTypes?.Count > 0)
            {
                cs = CalibrationType.CalibrationSubTypes.ToList();
            }
            else if (this?.EquipmentTypes?.Count > 0)
            {
                cs = new List<CalibrationSubType>();
                var css1 = this?.EquipmentTypes.Where(x => x.CalibrationTypeID == CalibrationType.CalibrationTypeId).ToList();

                if (css1 != null && css1?.Count > 0)
                {
                    foreach (var item in css1)
                    {
                        if (item?.CalibrationType?.CalibrationSubTypes != null && item?.CalibrationType?.CalibrationSubTypes?.Count > 0)
                        {
                            foreach (var item22 in item?.CalibrationType?.CalibrationSubTypes)
                            {
                                if (item22.UnitOfMeasureTypeID.HasValue)
                                {
                                    cs.Add(item22);
                                }

                            }


                        }

                    }
                }
            }

            if (cs?.Count > 0)
            {
                foreach (var item in cs)
                {
                    lst.Add(item.UnitOfMeasureTypeID);
                }



                if (lst.Count() > 0)
                {
                    var b = UnitofMeasureList.Join(lst, e => e.TypeID, id => id, (e, id) => e).ToList();//Where(x => x.Type.CalibrationTypeID == calibrationType).ToList();

                    return b.DistinctBy(x => x.UnitOfMeasureID).ToList();
                }

            }




            return UnitofMeasureList;

        }

        public Dictionary<string, string> GetUoMByTypeDIC(int Type)
        {
            List<int> lst = new List<int>();

            lst.Add(Type);

            var b = GetUoMByTypes(lst.ToArray());

            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (var item in b)
            {
                dic.Add(item.UnitOfMeasureID.ToString(), item.Abbreviation + " - " + item.Name);
            }

            return dic;

        }


        public Dictionary<string, string> GetUoMByTypeDIC(CalibrationType Type)
        {
            List<int> lst = new List<int>();


            foreach (var item in Type.CalibrationSubTypes)
            {
                if (item.UnitOfMeasureTypeID.HasValue)
                {
                    lst.Add(item.UnitOfMeasureTypeID.Value);
                }
                
            }

           

            var b = GetUoMByTypes(lst.ToArray());

            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (var item in b)
            {
                dic.Add(item.UnitOfMeasureID.ToString(), item.Abbreviation + " - " + item.Name);
            }

            return dic;

        }

        //public Dictionary<string, string> GetUoMByCalibrationTypeDIC(int calibrationType)
        //{


        //    var b = GetUoMByCalibrationType(calibrationType);

        //    Dictionary<string, string> dic = new Dictionary<string, string>();

        //    foreach (var item in b)
        //    {
        //        dic.Add(item.UnitOfMeasureID.ToString(), item.Abbreviation + " - " + item.Name);
        //    }

        //    return dic;

        //}

        public List<UnitOfMeasure> GetUoMByEquipmentType(EquipmentType _EquipmentType)
        {
            if (UnitofMeasureList == null)
            {
                UnitofMeasureList = new List<UnitOfMeasure>();
            }

            List<int?> lst = new List<int?>();
            List<CalibrationSubType> cs = null;

            if (_EquipmentType?.CalibrationType?.CalibrationSubTypes?.Count > 0)
            {
                cs = _EquipmentType.CalibrationType.CalibrationSubTypes.ToList();
            }
            else if (this?.EquipmentTypes?.Count > 0)
            {
                cs = new List<CalibrationSubType>();
                var css1 = this?.EquipmentTypes.Where(x => x.CalibrationTypeID == _EquipmentType.CalibrationTypeID).ToList();

                if (css1 != null && css1?.Count > 0)
                {
                    foreach (var item in css1)
                    {
                        if(item?.CalibrationType?.CalibrationSubTypes != null && item?.CalibrationType?.CalibrationSubTypes?.Count > 0 )
                        {
                            foreach(var item22 in item?.CalibrationType?.CalibrationSubTypes)
                            {
                                if (item22.UnitOfMeasureTypeID.HasValue)
                                {
                                    cs.Add(item22);
                                }
                              
                            }

                            
                        }
                        
                    }
                }
            } 

            if(cs?.Count > 0)
            {
                foreach (var item in cs)
                {
                    lst.Add(item.UnitOfMeasureTypeID);
                }



                if (lst.Count() > 0)
                {
                    var b = UnitofMeasureList.Join(lst, e => e.TypeID, id => id, (e, id) => e).ToList();//Where(x => x.Type.CalibrationTypeID == calibrationType).ToList();

                    return b.DistinctBy(x=>x.UnitOfMeasureID).ToList();
                }

            }





//            Console.WriteLine("----------------NO Calibration type yet configured");
            return UnitofMeasureList;

        }

        public Dictionary<string, Dictionary<string, string>> GetAllUoMHierarchicalDic()
        {
            if (UnitofMeasureList == null)
            {
                UnitofMeasureList = new List<UnitOfMeasure>();
            }

            if (UnitofMeasureTypeList == null)
            {
                UnitofMeasureTypeList = new List<UnitOfMeasureType>();
            }

            var allUnits = GetAllUnitOfMeasure(); // Usa el método existente que ya aplica DistinctBy2

            Dictionary<string, Dictionary<string, string>> hierarchicalDic = new Dictionary<string, Dictionary<string, string>>();

            // Agrupar unidades de medida por TypeID
            var groupedByType = allUnits.GroupBy(x => x.TypeID);

            foreach (var typeGroup in groupedByType)
            {
                var typeId = typeGroup.Key.ToString();

                // Crear el diccionario interno para las unidades de este tipo
                Dictionary<string, string> unitsInType = new Dictionary<string, string>();

                foreach (var unit in typeGroup)
                {
                    unitsInType.Add(unit.UnitOfMeasureID.ToString(), unit.Name);
                }

                // Agregar al diccionario principal usando TypeID como clave
                hierarchicalDic.Add(typeId, unitsInType);
            }

            return hierarchicalDic;
        }
        //public Dictionary<string,string> GetUnitOfMeasureByCalibrationTypeDic(EquipmentTemplate EquipmentTemplate)
        //{
        //    var b = GetUnitOfMeasureByCalibrationType(EquipmentTemplate);

        //    Dictionary<string, string> dic = new Dictionary<string, string>();

        //    foreach (var item in b)
        //    {
        //        dic.Add(item.UnitOfMeasureID.ToString(), item.Abbreviation + " - " + item.Name);
        //    }

        //    return dic;

        //}

        public List<UnitOfMeasure> GetAllUnitOfMeasure()
        {
            if (UnitofMeasureList == null)
            {
                UnitofMeasureList = new List<UnitOfMeasure>();
            }

            var b = UnitofMeasureList; //.DistinctBy2(x => x.Abbreviation).ToList();

            return b;

        }

        public Dictionary<string, string> GetAllUoMDic()
        {
            

            var b = GetAllUnitOfMeasure();

            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (var item in b)
            {
                dic.Add(item.UnitOfMeasureID.ToString(), item.Abbreviation + " - " + item.Name);
            }

            return dic;

        }

        public List<UnitOfMeasureType> GetAllUoMType()
        {


            List<UnitOfMeasureType> lst = new List<UnitOfMeasureType>();

            lst.AddRange(UnitofMeasureTypeList);

            UnitOfMeasureType uomt = new UnitOfMeasureType() { Value = -1, Name = "All UoM" }; 

            lst.Insert(0, uomt);

            return lst;


        }




        public AppState()
        {
            NotifyStateChanged();
            EquipmentTypes = new List<EquipmentType>();
            CalibrationTypes = new List<CalibrationType>();
            Manufacturers = new List<Manufacturer>();
            Procedures = new List<Procedure>();
            Equipments = new List<EquipmentTemplate>();

            TemperatureStandard = new List<PieceOfEquipment>();

            UnitofMeasureList = new List<UnitOfMeasure>();
            WeightTypeList = new List<WeightType>();
            WeightDistributionList = new List<WeightDistribution>();


            StatusList = new Dictionary<string, string>();

            StatusList.Add(0.ToString(), "N/A");
            StatusList.Add(1.ToString(), "Approved");
            StatusList.Add(2.ToString(), "Created");
            StatusList.Add(3.ToString(), "Inactive");
            StatusList.Add(4.ToString(), "Rejected");

            ToleranceList2 = new List<ToleranceType>();

            ToleranceListNoDynamic = new List<ToleranceType>();
            ToleranceListNoDynamic.Add(new ToleranceType() { Key = "2", Value = "Percentage + Resolution", CalibrationTypeId = 0 });
            ToleranceListNoDynamic.Add(new ToleranceType() { Key = "3", Value = "HB44", CalibrationTypeId = 0 });
            ToleranceListNoDynamic.Add(new ToleranceType() { Key = "4", Value = "Percentage", CalibrationTypeId = 0 });
         


            ToleranceListDynamic = new List<ToleranceType>();
            ToleranceListDynamic.Add(new ToleranceType() { Key = "100", Value = "Accuracy%", CalibrationTypeId = 0 });
            ToleranceListDynamic.Add(new ToleranceType() { Key = "101", Value = "Accuracy% + Value", CalibrationTypeId = 0 });
            ToleranceListDynamic.Add(new ToleranceType() { Key = "102", Value = "Accuracy% * Value", CalibrationTypeId = 0 });
            ToleranceListDynamic.Add(new ToleranceType() { Key = "103", Value = "Accuracy% + Resolution", CalibrationTypeId = 0 });
            ToleranceListDynamic.Add(new ToleranceType() { Key = "200", Value = "Resolution + Value", CalibrationTypeId = 0 });
            ToleranceListDynamic.Add(new ToleranceType() { Key = "201", Value = "Resolution * Value", CalibrationTypeId = 0});
            ToleranceListDynamic.Add(new ToleranceType() { Key = "1", Value = "Resolution", CalibrationTypeId = 0 });
           




            ClassList = new Dictionary<int, string>();
            ClassList.Add(1, "|");
            ClassList.Add(2, "||");
            ClassList.Add(3, "|||");
            ClassList.Add(4, "||||");
            ClassList.Add(5, "||| L");


            ClassPOE = new Dictionary<int, string>();
            ClassPOE.Add(1, "I");
            ClassPOE.Add(2, "II");
            ClassPOE.Add(3, "III");
            ClassPOE.Add(4, "IV");
            ClassPOE.Add(5, "V");
            ClassPOE.Add(6, "VI");
            ClassPOE.Add(7, "VII");
            ClassPOE.Add(8, "F");
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
            DistributionUncertList = new Dictionary<int, string>();
            DistributionUncertList.Add(1, "Rectangular");
            DistributionUncertList.Add(2, "Normal");
            DistributionUncertList.Add(3, "Expanded");
            DistributionUncertList.Add(4, "Resolution");


            DescriptionUncertList = new Dictionary<int, string>();
            DescriptionUncertList.Add(1, "I");
            DescriptionUncertList.Add(2, "II");
            DescriptionUncertList.Add(3, "III");
            DescriptionUncertList.Add(4, "IV");
            DescriptionUncertList.Add(5, "F");

            //ToleranceList.Add(3.ToString(), "Percentage");
            //ToleranceList.Add(4.ToString(), "Resolution Compound");
            //ToleranceList.Add(5.ToString(), "Tolerance Compound");

            //UnitofMeasureList.Add(new UnitOfMeasure { Name = "Pounds", Abbreviation = "Pd", UnitOfMeasureID = 2 });

            //UnitofMeasureList.Add(new UnitOfMeasure { Name = "Kilogramo", Abbreviation = "Kg", UnitOfMeasureID = 4 });

            TestPointGroupTypeList = new Dictionary<string, string>();

            TestPointGroupTypeList.Add("Linearity", "Linearity");
            TestPointGroupTypeList.Add("Repeatability", "Repeatability");
            TestPointGroupTypeList.Add("Eccentricity", "Eccentricity");

            //TestPointGroupTypeList.Add(2.ToString(), "Other");

            CompanyList = new Dictionary<string, string>();
            CompanyList.Add(0.ToString(), "Company 1");
            CompanyList.Add(1.ToString(), "Company 2");
            //CompanyList.Add(2.ToString(), "Company 3");
            CompanyList.Add(3.ToString(), "Company 4");

            WeightTypeList.Add(new WeightType { Name = "A", WeightTypeID = 1 });
            WeightTypeList.Add(new WeightType { Name = "B", WeightTypeID = 2 });

            WeightDistributionList.Add(new WeightDistribution { Name = "Rectangular", WeightDistributionID = 1 });
            WeightDistributionList.Add(new WeightDistribution { Name = "Normal", WeightDistributionID = 2 });
            WeightDistributionList.Add(new WeightDistribution { Name = "Expanded", WeightDistributionID = 3 });
            WeightDistributionList.Add(new WeightDistribution { Name = "Resolution", WeightDistributionID = 4 });



            Comments = new List<KeyValue2>();

            Comments.Add(new KeyValue2 { Key=1,Value= "Always" });
            Comments.Add(new KeyValue2 { Key = 2, Value = "When Failed" });
            Comments.Add(new KeyValue2 { Key = 3, Value = "When Passed" });
            Comments.Add(new KeyValue2 { Key = 4, Value = "Accredited" });
            Comments.Add(new KeyValue2 { Key = 5, Value = "Non-Accredited" });

            TYpeMicro= new List<KeyValue2>();
            TYpeMicro.Add(new KeyValue2 { Key = 1, Value = "HV (Vicker)" });
            TYpeMicro.Add(new KeyValue2 { Key = 2, Value = "HK (Knoop)" });

            CalibrationInterval = new Dictionary<int, string>();

            CalibrationInterval.Add(1, "Custom");
            CalibrationInterval.Add(2, "1 months");
            CalibrationInterval.Add(3, "2 months");
            CalibrationInterval.Add(4, "3 months");
            CalibrationInterval.Add(5, "6 months");
            CalibrationInterval.Add(6, "12 months");

            StatusListET = new Dictionary<string, string>();
            StatusListET.Add(0.ToString(), "N/A");
            StatusListET.Add(1.ToString(), "Approved");
            StatusListET.Add(2.ToString(), "Created");
            StatusListET.Add(3.ToString(), "Inactive");
            StatusListET.Add(4.ToString(), "Rejected");

        }


        public List<UnitOfMeasureType> UnitofMeasureTypeList { get; set; }

        public void AddEquipmentType(EquipmentType eq)
        {
            EquipmentTypes.Add(eq);
        }

        public void AddCalibrationType(CalibrationType eq)
        {
            CalibrationTypes.Add(eq);
        }
        //TODO CAMBIO GENERAL
        public void AddManufacturer(Manufacturer eq)
        {

             var item2 = Manufacturers.Where(x => x.ManufacturerID == eq.ManufacturerID).FirstOrDefault();

            if (item2 == null)
            {

                Manufacturers.Add(eq);
            }


        }

        public void AddProcedure(Procedure eq)
        {

            var item2 = Procedures.Where(x => x.ProcedureID == eq.ProcedureID).FirstOrDefault();

            if (item2 == null)
            {

                Procedures.Add(eq);
            }


        }

        public void AddEquipment(EquipmentTemplate eq)
        {
            Equipments.Add(eq);
        }

        public void AddTemperatureStandard(PieceOfEquipment eq)
        {
            TemperatureStandard.Add(eq);
        }

        public List<EquipmentTemplate> EquipmentTemplate { get; set; }
        public List<PieceOfEquipment> TemperatureStandard { get; set; }


        public List<EquipmentTypeGroup> EquipmentTypeGroups
        {
            get
            {
                List<EquipmentTypeGroup> etgl=new List<EquipmentTypeGroup>();
                List<EquipmentTypeGroup> etgl2 = new List<EquipmentTypeGroup>();

                var list= EquipmentTypes?.Where(x => x.EquipmentTypeGroupID.HasValue == true).OrderBy(x=>x.EquipmentTypeGroupID).ToList(); 

                if (list?.Count > 0)
                {
                    foreach (var item in list)
                    {
                        etgl.Add(item.EquipmentTypeGroup);
                    }

                    etgl2=etgl.DistinctBy(x=>x.Name).OrderBy(y=>y.Name).ToList();
                }

                return etgl2;
            }
        }

        public Dictionary<string, string> StatusListET { get; set; }

        public List<EquipmentType> EquipmentTypes { get; set; }

        public List<CalibrationType> CalibrationTypes { get; set; }

        public List<Manufacturer> Manufacturers { get; set; }

        public List<Procedure> Procedures { get; set; }

        public List<EquipmentTemplate> Equipments { get; set; }

        public Dictionary<string, string> StatusList { get; set; }

        //public Dictionary<string, string> ToleranceList { get; set; }

        public List<ToleranceType> ToleranceList2 { get; set; }
        public List<ToleranceType> ToleranceListDynamic { get; set; }
        public List<ToleranceType> ToleranceListNoDynamic { get; set; }

        public Dictionary<int, string> ClassList { get; set; }

        public Dictionary<int, string> ClassPOE { get; set; }
        public Dictionary<int, string> DistributionUncertList { get; set; }
        public Dictionary<int, string> DescriptionUncertList { get; set; }

        public List<UnitOfMeasure> UnitofMeasureList { get; set; }
        
        public Dictionary<string, string> TestPointGroupTypeList { get; set; }

        public Dictionary<string, string> CompanyList { get; set; }
        public Dictionary<string, string> AddressList { get; set; }
        public Dictionary<string, string> UsersList { get; set; }
        private void NotifyStateChanged() => OnChange?.Invoke();

        public List<WeightType> WeightTypeList { get; set; }


        public List<WeightDistribution> WeightDistributionList { get; set; }


       public List<Status> WODStatus { get; set; }


        public IEnumerable<int> WODStatusCount { get; set; }


        public Dictionary<int, string> CalibrationInterval { get; set; }

        public int? TotalWorkOrder { get; set; }
        public int? UpcomingWorkOrderDetails { get; set; }
        public int? ActiveWorkOrderDetails   { get; set; }

        public IEnumerable<KeyValueDate> WODByDay { get; set; }

        public List<KeyValue2> Comments { get; set; }

        public List<KeyValue2> TYpeMicro { get; set; }
        public List<CalibrationSubType> CalibrationSubTypes { get; set; }

    }
}