using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.BusinessExceptions;
using CalibrationSaaS.Domain.BusinessExceptions.Customer;
using CalibrationSaaS.Domain.Repositories;
using Helpers;
using Helpers.Controls.ValueObjects;
using Newtonsoft.Json;
using Reports.Domain.ReportViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using static CalibrationSaaS.Domain.Aggregates.Querys.Querys;
using Totales = Reports.Domain.ReportViewModels.Totales;
using UncertaintyViewModel = Reports.Domain.ReportViewModels.UncertaintyViewModel;

namespace CalibrationSaaS.Application.UseCases
{
    public class PieceOfEquipmentUseCases
    {
        //private readonly ValidatorHelper modelValidator;
        
        public AppState AppState2 { get; set; }
        private readonly IPieceOfEquipmentRepository pieceOfEquipmentRepository;
        private readonly IWorkOrderDetailRepository wodRepository;
        private readonly IUOMRepository uomRepository;
        //private readonly ICustomerRepository customerRepository;
        private readonly IBasicsRepository Basics;
        public PieceOfEquipmentUseCases(IPieceOfEquipmentRepository pieceOfEquipmentRepository , IWorkOrderDetailRepository wodRepository, IUOMRepository uomRepository, IBasicsRepository _Basics)
        //, ICustomerRepository customerRepository,IBasicsRepository _basicsRepository)
        {
            this.pieceOfEquipmentRepository = pieceOfEquipmentRepository;
            this.wodRepository = wodRepository;
            this.uomRepository = uomRepository;
            Basics = _Basics;
            //this.customerRepository = customerRepository;
            //this.basicsRepository = _basicsRepository;
        }

    
        public async Task<PieceOfEquipment> CreatePieceOfEquipment(PieceOfEquipment pieceOfEquipmentDTO, string Component="")
        {

            pieceOfEquipmentDTO = await this.pieceOfEquipmentRepository.InsertPieceOfEquipment(pieceOfEquipmentDTO, Component);

            return pieceOfEquipmentDTO;


        }

        public async Task<ResultSet<PieceOfEquipment>> GetPieceOfEquipment(Pagination<PieceOfEquipment> pagination)
        {

            var a= await pieceOfEquipmentRepository.GetPieceOfEquipment(pagination);

            return a;
        }

        public async Task<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentChildren(Pagination<PieceOfEquipment> pagination)
        {

            var a = await pieceOfEquipmentRepository.GetPieceOfEquipmentChildren(pagination);

            return a;
        }


        public async Task<IEnumerable<PieceOfEquipment>> GetAllWeightSets(PieceOfEquipment DTO)
        {
            return await pieceOfEquipmentRepository.GetAllWeightSets(DTO);
        }

        public async Task<PieceOfEquipment> DeletePieceOfEquipment(PieceOfEquipment newPieceOfEquipment)
        {

            return await pieceOfEquipmentRepository.DeletePieceOfEquipment(newPieceOfEquipment);
        }

        public async Task<PieceOfEquipment> UpdatePieceOfEquipment(PieceOfEquipment newPieceOfEquipment,string Component="")
        {

            var result = await pieceOfEquipmentRepository.UpdatePieceOfEquipment(newPieceOfEquipment, Component);

          //  this.UpdatePieceOfEquipment.Save();
            return result;
        }

        public async Task<PieceOfEquipment> GetPieceOfEquipmentByID(PieceOfEquipment newPieceOfEquipment,string user="",string Component="")
        {
            PieceOfEquipment poe = await pieceOfEquipmentRepository.GetPieceOfEquipmentByID(newPieceOfEquipment.PieceOfEquipmentID,user, Component);
          
           
            return poe;
 
        }

        public async Task<ResultSet<PieceOfEquipment>> GetPieceOfEquipmenIndicator(Pagination<PieceOfEquipment> id)
        {
            return await pieceOfEquipmentRepository.GetPieceOfEquipmentIndicator(id);
        }

        public async Task<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentBalanceByIndicator(Pagination<PieceOfEquipment> id)
        {
            return await pieceOfEquipmentRepository.GetPieceOfEquipmentBalanceByIndicator(id);
        }

         public async Task<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentBalanceByPer(Pagination<PieceOfEquipment> id)
        {
            return await pieceOfEquipmentRepository.GetPieceOfEquipmentBalanceByPer(id);
        }

        


        public async Task<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentByCustomer(Pagination<PieceOfEquipment> DTO)
        {
            return await pieceOfEquipmentRepository.GetPieceOfEquipmentByCustomer(DTO);

        }

        //YPPP
        public async Task<IEnumerable<WorkOrderDetail>> GetPieceOfEquipmentHistory(string id)
        {
            return await pieceOfEquipmentRepository.GetPieceOfEquipmentHistory(id);
        }

        public async Task<IEnumerable<PieceOfEquipment>> GetPieceOfEquipmentByET(int id)
        {
            var result = await pieceOfEquipmentRepository.GetPieceOfEquipmentByET(id);
            return result;

        }

        public async Task<IEnumerable<PieceOfEquipment>> GetTemperatureStandard()
        {

            var a = await pieceOfEquipmentRepository.GetTemperatureStandard();

            return a;

        }

        
        
        public async Task<IEnumerable<PieceOfEquipment>> GetPieceOfEquipmentByCustomer(int id)
        {
            var result = await pieceOfEquipmentRepository.GetPieceOfEquipmentByCustomer(id);
            return result;

        }

        public async Task<PieceOfEquipment> UpdateIndicator(PieceOfEquipment newPieceOfEquipment)
        {

            await pieceOfEquipmentRepository.UpdatePieceOfEquipment(newPieceOfEquipment);

            //  this.UpdatePieceOfEquipment.Save();
            return newPieceOfEquipment;
        }


        public async Task<ResultSet<PieceOfEquipment>> GetAllWeightSetsPag(Pagination<PieceOfEquipment> pagination)
        {

           return await  pieceOfEquipmentRepository.GetAllWeightSetsPag(pagination);

           
        }


        public async Task<ResultSet<PieceOfEquipment>> GetAllPeripheralsPag(Pagination<PieceOfEquipment> pagination)
        {

            return await pieceOfEquipmentRepository.GetAllPeripheralsPag(pagination);


        }


        public async Task<ResultSet<WeightSet>> SaveWeights(ICollection<WeightSet> W)
        {
            return await pieceOfEquipmentRepository.SaveWeights(W);
        }

         public async Task<Uncertainty> CreateUncertainty(TableChanges<Uncertainty> Uncertainty)
        {
            return await pieceOfEquipmentRepository.CreateUncertainty(Uncertainty);
        }


        public async Task<ResultSet<Uncertainty>> GetUncertainty(Pagination<Uncertainty> Uncertainty)
        {
            return await pieceOfEquipmentRepository.GetUncertainty(Uncertainty);
        }

        //public async Task<CalibrationResultContributor> DeletetCalibrationResultContributor(PieceOfEquipment idCalibrationResult)
        //{

        //    return await pieceOfEquipmentRepository.DeletePieceOfEquipment(idCalibrationResult);
        //}

        
        public async Task<ICollection<Force>> GetCalculatesForISOandASTM(ISOandASTM ISOandASTM)
        {
            try
            {
                WorkOrderDetail w = new WorkOrderDetail();

                var forces = ISOandASTM.Forces;

                if (forces == null || forces.Count ==0)
                {
                    return forces;
                }
               
                forces.ForEach(item => item.BasicCalibrationResult.ClassRun1 = 0.ToString());
                forces.ForEach(item => item.BasicCalibrationResult.Class = 0.ToString());
                w.WorkOrderDetailID = forces.FirstOrDefault().WorkOrderDetailId;
                var wod = ISOandASTM.WorkOrderDetail;//await wodRepository.GetByID(w);
                int? iso = 0;
                if (wod.CertificationID != null)
                    iso = wod.CertificationID;
                List<Force> forceResultAux = new List<Force>();

                bool isAdj;
                if (forces.Sum(x => x.BasicCalibrationResult.RUN2) != 0)
                    isAdj = true;
                else
                    isAdj = false;

                double relativeResolution = (wod.Resolution / wod.PieceOfEquipment.Capacity) * 100;
                double classRelativeresolution = 0;

                if (relativeResolution >= 0 && relativeResolution <= 0.25)
                    classRelativeresolution = 0.5;
                else if (relativeResolution > 0.25 && relativeResolution <= 0.5)
                    classRelativeresolution = 1;
                else if (relativeResolution > 0.5 && relativeResolution <= 1)
                    classRelativeresolution = 2;
                else if (relativeResolution > 1 && relativeResolution <= 1.5)
                    classRelativeresolution = 3;
                else
                    classRelativeresolution = 0;

                double classZero = 0;
                double errorZero = 0;

                if (iso != null)
                {
                    List<Force> forceResult = new List<Force>();
                    double zeroError = 0;
                    List<double> forcesApplied = new List<double>();
                    forcesApplied = forces
                        .Where(item2 => item2.BasicCalibrationResult.Nominal != 0)
                        .Select(item2 => item2.BasicCalibrationResult.Nominal)
                        .ToList();

                    double maxForce = forcesApplied.Max();
                    double maxForce_ = Math.Abs((maxForce * 0.1) / 100);
                    double minForce = forcesApplied.Min();
                    double minForce_ = Math.Abs((minForce * 1) / 100);
                 
                    foreach (var item in forces)
                    {


                        var standards = await pieceOfEquipmentRepository.GetCalibrationByWod(item.WorkOrderDetailId, item.CalibrationSubTypeId);
                        item.CalibrationSubType_Weights = standards.ToList();
                        //Max and Min Fs 
                       

                        int i = 0;
                        //foreach (var item2 in forces)
                        //{
                            

                        //    if (item2.BasicCalibrationResult.Nominal != 0)
                        //        forcesApplied.Add(item2.BasicCalibrationResult.Nominal);

                        //}

                        

                        //Search Standard
                        CalibrationSubType_Weight weight = new CalibrationSubType_Weight();
                        string standardId = "";
                        weight = standards.Where(x => x.SecuenceID == item.SequenceID).FirstOrDefault();
                        if (weight != null && standards.Count() > 0 && item.SequenceID == weight.SecuenceID)
                        {


                            var x = await pieceOfEquipmentRepository.GetWeigthSetById(weight.WeightSetID);
                            standardId = x.PieceOfEquipmentID;
                             item.BasicCalibrationResult.StandardId = standardId;
                        }

                       if (item.BasicCalibrationResult.Nominal != item.BasicCalibrationResult.NominalTemp)
                        {
                            item.BasicCalibrationResult.FS = Math.Round((item.BasicCalibrationResult.Nominal * 100) / wod.PieceOfEquipment.Capacity, 3);
                        }

                       if (item.BasicCalibrationResult.FS != item.BasicCalibrationResult.FsTemp )
                        {
                            //Calculate Nominal 
                            item.BasicCalibrationResult.Nominal = Math.Round((wod.PieceOfEquipment.Capacity * item.BasicCalibrationResult.FS) / 100);

                        }
                        item.BasicCalibrationResult.FsTemp = item.BasicCalibrationResult.FS;
                        item.BasicCalibrationResult.NominalTemp = item.BasicCalibrationResult.Nominal;
                        item.Uncertainty = 0;
                        item.BasicCalibrationResult.Uncertanty = 0;
                        double classRelativeIndicationError = 0;
                        double classRepeatabilityIndicationError = 0;
                        var force = item;
                        double error1 = 0;
                        double error2 = 0;
                        double error3 = 0;
                        double error4 = 0;

                        
                          var res = wod.Resolution;

                        
                        ////////////////////////
                        ///Test Point cero Error ISO
                        ///////////////////////
                        if (force.BasicCalibrationResult.Nominal == 0)
                       {
                            double run1 = force.BasicCalibrationResult.RUN1;
                            double run2 = force.BasicCalibrationResult.RUN2;
                            double run3 = force.BasicCalibrationResult.RUN3;
                            double run4 = force.BasicCalibrationResult.RUN4;
                            var fn = maxForce;

                            error1 = (run1 / fn) * 100;
                            force.BasicCalibrationResult.ErrorRun1 = Math.Round(error1, 3);

                            error2 = (run2 / fn) * 100;
                            force.BasicCalibrationResult.ErrorRun2 = Math.Round(error2, 3);

                            error3 = (run3 / fn) * 100;
                            force.BasicCalibrationResult.ErrorRun3 = Math.Round(error3, 3);

                            error4 = (run4 / fn) * 100;
                            force.BasicCalibrationResult.ErrorRun4 = Math.Round(error4, 3);

                            if (item.BasicCalibrationResult.Position == 0)
                            {
                                double[] errors = new double[3] { error1, error3, error4 };
                                double[] errors2 = new double[3] { error2, error3, error4 };
                                if (isAdj)
                                {
                                    errorZero = errors2.Max();
                                }
                                else

                                {
                                    errorZero = errors.Max();
                                }
                            }
                        }
                        else
                        { 
                         error1 = Math.Round(force.BasicCalibrationResult.RUN1 - force.BasicCalibrationResult.Nominal  , 3);
                         error2 = Math.Round(force.BasicCalibrationResult.RUN2 - force.BasicCalibrationResult.Nominal, 3);
                         error3 = Math.Round(force.BasicCalibrationResult.RUN3 - force.BasicCalibrationResult.Nominal, 3);
                         error4 = Math.Round(force.BasicCalibrationResult.RUN4 - force.BasicCalibrationResult.Nominal, 3);
                        }

                        var error1_p = Math.Round((error1 / force.BasicCalibrationResult.Nominal) * 100, 3);
                        var error2_p = Math.Round((error2 / force.BasicCalibrationResult.Nominal) * 100, 3);
                        var error3_p = Math.Round((error3 / force.BasicCalibrationResult.Nominal) * 100, 3);
                        var error4_p = Math.Round((error4 / force.BasicCalibrationResult.Nominal) * 100, 3);

                        if (force.BasicCalibrationResult.Nominal == 0)
                        {
                            error1_p = error1;
                            error2_p = error2;
                            error3_p = error3;
                            error4_p = error4;

                            error1 = force.BasicCalibrationResult.RUN1;
                            error2 = force.BasicCalibrationResult.RUN2;
                            error3 = force.BasicCalibrationResult.RUN3;
                            error4 = force.BasicCalibrationResult.RUN4;
                           
                        }



                            if (double.IsNaN(Math.Round(error1_p, 3)) || double.IsInfinity(Math.Round(error1_p, 3)))
                        {
                            force.BasicCalibrationResult.ErrorpRun1 = 0;
                        }
                        else
                        {
                            force.BasicCalibrationResult.ErrorpRun1 = Math.Round(error1_p, 3);
                        }

                        if (double.IsNaN(Math.Round(error2_p, 3)) || double.IsInfinity(Math.Round(error2_p, 3)))
                        {
                            force.BasicCalibrationResult.ErrorpRun2 = 0;
                        }
                        else
                        {
                            force.BasicCalibrationResult.ErrorpRun2 = Math.Round(error2_p, 3);
                        }

                        if (double.IsNaN(Math.Round(error3_p, 3)) || double.IsInfinity(Math.Round(error3_p, 3)))
                        {
                            force.BasicCalibrationResult.ErrorpRun3 = 0;
                        }
                        else
                        {
                            force.BasicCalibrationResult.ErrorpRun3 = Math.Round(error3_p, 3);
                        }

                        if (double.IsNaN(Math.Round(error4_p, 3)) || double.IsInfinity(Math.Round(error4_p, 3)))
                        {
                            force.BasicCalibrationResult.ErrorpRun4 = 0;
                        }
                        else
                        {
                            force.BasicCalibrationResult.ErrorpRun4 = Math.Round(error4_p, 3);
                        }


                        if (double.IsNaN(Math.Round(error1, 3)) || double.IsInfinity(Math.Round(error1, 3)))
                        {
                            force.BasicCalibrationResult.ErrorRun1 = 0;
                        }
                        else
                        {
                            force.BasicCalibrationResult.ErrorRun1 = Math.Round(error1, 3);
                        }
                        if (double.IsNaN(Math.Round(error2, 3)) || double.IsInfinity(Math.Round(error2, 3)))
                        {
                            force.BasicCalibrationResult.ErrorRun2 = 0;
                        }
                        else
                        {
                            force.BasicCalibrationResult.ErrorRun2 = Math.Round(error2, 3);
                        }

                        if (double.IsNaN(Math.Round(error3, 3)) || double.IsInfinity(Math.Round(error3, 3)))
                        {
                            force.BasicCalibrationResult.ErrorRun3 = 0;
                        }
                        else
                        {
                            force.BasicCalibrationResult.ErrorRun3 = Math.Round(error3, 3);
                        }

                        if (double.IsNaN(Math.Round(error4, 3)) || double.IsInfinity(Math.Round(error4, 3)))
                        {
                            force.BasicCalibrationResult.ErrorRun4 = 0;
                        }
                        else
                        {
                            force.BasicCalibrationResult.ErrorRun4 = Math.Round(error4, 3);
                        }

                        force.BasicCalibrationResult.RelativeIndicationErrorR1 = Math.Abs(error1_p);
                        force.BasicCalibrationResult.RelativeIndicationErrorR2 = Math.Abs(error2_p);
                        force.BasicCalibrationResult.RelativeIndicationErrorR3 = Math.Abs(error3_p);
                        force.BasicCalibrationResult.RelativeIndicationErrorR4 = Math.Abs(error4_p);

                       
                        double relativeIndicationErr = 0;

                        if (isAdj)
                        {
                            relativeIndicationErr = (Math.Abs(error2_p) + Math.Abs(error3_p) + Math.Abs(error4_p)) / 3;
                            if (Math.Abs(error2) >= Math.Abs(error3) && Math.Abs(error2) >= Math.Abs(error4))
                                force.BasicCalibrationResult.MaxErrorNominal = Math.Round(error2);
                            else if (Math.Abs(error3) >= Math.Abs(error2) && Math.Abs(error3) >= Math.Abs(error4))
                                force.BasicCalibrationResult.MaxErrorNominal = Math.Round(error3);
                            else
                                force.BasicCalibrationResult.MaxErrorNominal = Math.Round(error4);
                        }
                        else
                        {
                            relativeIndicationErr = (Math.Abs(error1_p) + Math.Abs(error3_p) + Math.Abs(error4_p)) / 3;
                            if (Math.Abs(error1) >= Math.Abs(error3) && Math.Abs(error1) >= Math.Abs(error4))
                                force.BasicCalibrationResult.MaxErrorNominal = Math.Round(error1);
                            else if (Math.Abs(error3) >= Math.Abs(error1) && Math.Abs(error3) >= Math.Abs(error4))
                                force.BasicCalibrationResult.MaxErrorNominal = Math.Round(error3);
                            else
                                force.BasicCalibrationResult.MaxErrorNominal = Math.Round(error4);
                        }


                        if (double.IsNaN(Math.Round(relativeIndicationErr, 3)) || double.IsInfinity(Math.Round(relativeIndicationErr, 3)))
                        {
                            force.BasicCalibrationResult.RelativeIndicationError = 0;
                        }
                        else
                        {
                            force.BasicCalibrationResult.RelativeIndicationError = Math.Round(relativeIndicationErr, 3);
                        }

                        if (iso == 1)
                        {
                            //Calculate Relative Repeatability Error
                            double maxReading = 0;
                            double minReading = 10000;
                            //var ErrorPercent
                            double[] _maxminReading1 = new double[3] { Math.Abs(error1_p), Math.Abs(error3_p), Math.Abs(error4_p) };
                            double[] _maxminReading2 = new double[3] { Math.Abs(error2_p), Math.Abs(error3_p), Math.Abs(error4_p) };

                            if (isAdj)
                            {
                                for (int x = 0; x < _maxminReading2.Count(); x++)
                                {

                                    if (Math.Abs(_maxminReading2[x]) > Math.Abs(maxReading))
                                    {
                                        maxReading = _maxminReading2[x];
                                    }
                                    if (Math.Abs(_maxminReading2[x]) < Math.Abs(minReading))
                                    {
                                        minReading = _maxminReading2[x];
                                    }

                                }
                            }
                            else

                            {
                                for (int x = 0; x < _maxminReading1.Count(); x++)
                                {

                                    if (Math.Abs(_maxminReading1[x]) > Math.Abs(maxReading))
                                    {
                                        maxReading = _maxminReading1[x];
                                    }
                                    if (Math.Abs(_maxminReading1[x]) < Math.Abs(minReading))
                                    {
                                        minReading = _maxminReading1[x];
                                    }

                                }
                            }

                            var relativeRepError = Math.Abs(maxReading) - Math.Abs(minReading);

                            if (double.IsNaN(Math.Round(relativeRepError, 3)) || relativeRepError == - 10000 || double.IsInfinity(relativeRepError))
                            {
                                force.BasicCalibrationResult.RelativeRepeatabilityError = 0;
                            }
                            else
                            {
                                force.BasicCalibrationResult.RelativeRepeatabilityError = Math.Round(relativeRepError, 3);
                            }

                            //Calculate Class
                            //Calculate Class
                            //1 Calculate Class Indication

                            var relativeIndicationErr_ = Math.Abs(relativeIndicationErr);
                            if (relativeIndicationErr_ >= 0 && relativeIndicationErr_ <= 0.5)
                            {
                                classRelativeIndicationError = 0.5;
                               
                               
                            }
                            else if (relativeIndicationErr_ > 0.5 && relativeIndicationErr_ <= 1)
                            {
                                classRelativeIndicationError = 1;
                              
                            }
                            else if (relativeIndicationErr_ > 1 && relativeIndicationErr_ <= 2)
                            {
                                classRelativeIndicationError = 2;
                            
                            }
                            else if (relativeIndicationErr_ > 2 && relativeIndicationErr_ <= 3)

                            {
                                classRelativeIndicationError = 3;
                            
                            }
                            else
                            {
                              
                                classRelativeIndicationError = 4;
                            }

                            //2. Calculate Class Repeatability                                                                    f

                            var relativeRepError_ = Math.Abs(relativeRepError);
                            if (relativeRepError_ >= 0 && relativeRepError_ <= 0.5)
                                classRepeatabilityIndicationError = 0.5;
                            else if (relativeRepError_ > 0.5 && relativeRepError_ <= 1)
                                classRepeatabilityIndicationError = 1;
                            else if (relativeRepError_ > 1 && relativeRepError_ <= 2)
                                classRepeatabilityIndicationError = 2;
                            else if (relativeRepError_ > 2 && relativeRepError_ <= 3)
                                classRepeatabilityIndicationError = 3;
                            else
                                classRepeatabilityIndicationError = 4;
                        }

                       // double relativeResolution = force.BasicCalibrationResult.RelativeResolution;
                        //Class Run1 
                        //1 Calculate Class Indication Run1
                        double classRelativeIndicationErrorRun1 = 0;
                        var relativeIndicationErrRun1_ = Math.Abs(error1_p);
                        if (relativeIndicationErrRun1_ >= 0 && relativeIndicationErrRun1_ <= 0.5)
                            classRelativeIndicationErrorRun1 = 0.5;
                        else if (relativeIndicationErrRun1_ > 0.5 && relativeIndicationErrRun1_ <= 1)
                            classRelativeIndicationErrorRun1 = 1;
                        else if (relativeIndicationErrRun1_ > 1 && relativeIndicationErrRun1_ <= 2)
                            classRelativeIndicationErrorRun1 = 2;
                        else if (relativeIndicationErrRun1_ > 2 && relativeIndicationErrRun1_ <= 3)
                            classRelativeIndicationErrorRun1 = 3;
                        else
                            classRelativeIndicationErrorRun1 = 4;
                        ////
                        ///ASTM
                        double repeatability;
                        double maxError = 0;


                        double maxReadingASTM = 0;
                        double minReadingASTM = 0;
                        //var ErrorPercent
                        double[] _maxminReading1ASTM = new double[2] { Math.Abs(error1_p), Math.Abs(error3_p) };
                        double[] _maxminReading2ASTM = new double[2] { Math.Abs(error2_p), Math.Abs(error3_p) };

                        if (isAdj)
                        {
                            for (int x = 0; x < _maxminReading2ASTM.Count(); x++)
                            {

                                if (Math.Abs(_maxminReading2ASTM[x]) > Math.Abs(maxReadingASTM))
                                {
                                    maxReadingASTM = _maxminReading2ASTM[x];
                                }
                                if (Math.Abs(_maxminReading2ASTM[x]) < Math.Abs(minReadingASTM))
                                {
                                    minReadingASTM = _maxminReading2ASTM[x];
                                }

                            }

                            //Nominal Error
                            if (Math.Abs(error2) >= Math.Abs(error3))
                                force.BasicCalibrationResult.MaxErrorNominalASTM = error2;
                            else if (Math.Abs(error3) >= Math.Abs(error2))
                                force.BasicCalibrationResult.MaxErrorNominalASTM = error3;


                        }
                        else

                        {
                            for (int x = 0; x < _maxminReading1ASTM.Count(); x++)
                            {

                                if (Math.Abs(_maxminReading1ASTM[x]) > Math.Abs(maxReadingASTM))
                                {
                                    maxReadingASTM = _maxminReading1ASTM[x];
                                }
                                if (Math.Abs(_maxminReading1ASTM[x]) < Math.Abs(minReadingASTM))
                                {
                                    minReadingASTM = _maxminReading1ASTM[x];
                                }

                            }

                            // Nominal Error
                            if (Math.Abs(error1) >= Math.Abs(error3))
                                force.BasicCalibrationResult.MaxErrorNominalASTM = error1;
                            else if (Math.Abs(error3) >= Math.Abs(error1))
                                force.BasicCalibrationResult.MaxErrorNominalASTM = error3;
                        }


                        // 9964 
                        //Max Error % should not include Repeatailibty %
                        if (isAdj)
                        {
                            repeatability = error3_p - error2_p;
                            double[] maxClassAstm = new double[2] { Math.Abs(error2_p), Math.Abs(error3_p) }; //, Math.Abs(repeatability) };
                            maxError = maxClassAstm.Max();
                        }
                        else
                        {

                            repeatability = Math.Abs( error3_p - error1_p);
                            double[] maxClassAstm = new double[2] { Math.Abs(error1_p), Math.Abs(error3_p) }; // repeatability };
                            maxError = maxClassAstm.Max();

                            
                        }

                        if (double.IsNaN(Math.Round(maxError, 3)) || double.IsInfinity(Math.Round(maxError, 3)))
                        {
                            force.BasicCalibrationResult.MaxErrorp = 0;
                        }
                        else
                        {
                            force.BasicCalibrationResult.MaxErrorp = Math.Round(maxError, 3);
                        }


                        //////
                        /// Validation Difference higher than 1%
                        
                        double repeat = 0;
                        
                        force.BasicCalibrationResult.MessageForceDifference = null;
                        if (Math.Abs(repeatability) > 1)
                            force.BasicCalibrationResult.MessageForceDifference = "Difference higher than 1%";
                       ///////

                        //3. Calculate class Zero


                        //Get Zero
                        double zero = 0;
                        
                        var zero_ = errorZero;

                        
                            if (zero_ >= 0 && zero_ <= 0.05)
                                classZero = 0.5;
                            else if (zero_ > 0.05 && zero_ <= 0.1)
                                classZero = 1;
                            else if (zero_ > 0.1 && zero_ <= 0.2)
                                classZero = 2;
                            else if (zero_ > 0.2 && zero_ <= 0.3)
                                classZero = 3;
                            else
                                classZero = 0;
                                                

                        //calculate maximum pemisible value
                        double classReg = 0;



                        double[] maxClass = new double[4] { classRelativeIndicationError, classRepeatabilityIndicationError, classZero, classRelativeresolution};

                        for (int j = 0; j < maxClass.Count(); j++)
                        {

                            if (Math.Abs(maxClass[j]) > Math.Abs(classReg))
                            {
                                classReg = Math.Abs(maxClass[j]);
                            }

                        }

                       
                        //    if (force?.WeightSets?.Count() > 0 && force?.WeightSets != null)
                        //{
                        //    if ((double)force.WeightSets.FirstOrDefault().Class < classReg && force.WeightSets.FirstOrDefault().Class != 0)

                        //        force.BasicCalibrationResult.Class = Math.Round(force.WeightSets.FirstOrDefault().Class, 0).ToString();
                        //    else
                        //        force.BasicCalibrationResult.Class = classReg.ToString();
                        //}
                        //else
                        //{
                            force.BasicCalibrationResult.Class = classReg.ToString();
                            force.BasicCalibrationResult.ClassIso = 

                        //}


                        

                        force.BasicCalibrationResult.ClassRun1 = classRelativeIndicationErrorRun1.ToString();

                        
                        force.BasicCalibrationResult.ToastMessageForce = "";
                        force.BasicCalibrationResult.ToastMessageForceTotal = "";
                     
                        if (force.BasicCalibrationResult.Class == "4")
                        {

                            force.BasicCalibrationResult.Class = "NA";
                        }

                        if (force.BasicCalibrationResult.ClassRun1 == "4")
                        {

                            force.BasicCalibrationResult.ClassRun1 = "NA";
                        }




                        Helpers.JavaScriptFunctions func = new JavaScriptFunctions();
                        double resolution = 0;
                        if (wod.Tolerance.ToleranceValue != null && wod.Tolerance.ToleranceValue != 0 ) //= 100 || wod.Tolerance.ToleranceTypeID == 103 || wod.Tolerance.ToleranceTypeID == 1 || wod.Tolerance.ToleranceTypeID == 3)
                        {
                            resolution = wod.Tolerance.ToleranceValue;
                        }
                        else
                        {
                            resolution = wod.Resolution;
                        }
                        ////////////////////////
                        ///Test Point cero Class ASTM
                        ///////////////////////
                        if (force.BasicCalibrationResult.Nominal == 0)
                        {
                            
                            double run1 =  Math.Abs(force.BasicCalibrationResult.RUN1);
                            double run2 = Math.Abs(force.BasicCalibrationResult.RUN2);
                            double run3 = Math.Abs(force.BasicCalibrationResult.RUN3);
                            double run4 = Math.Abs(force.BasicCalibrationResult.RUN4);

                           
                            
                            double passrun1 = 0;
                            double passrun2 = 0;
                            double passrun3 = 0;
                            double passrun4 = 0;

                            if (run1 <= maxForce_ && run1 <= minForce_)
                            {
                                passrun1 = 1;
 
                            }
                            else
                            {
                                passrun1 = 0;
                                
                            }
                                

                            if (run2 <= maxForce_ && run2 <= minForce_)
                                passrun2 = 1;
                            else
                                passrun2 = 0;

                            if (run3 <= maxForce_ && run3 <= minForce_)
                                passrun3 = 1;
                            else
                                passrun3 = 0;

                            if (run4 <= maxForce_ && run4 <= minForce_)
                                passrun4 = 1;
                            else
                                passrun4 = 0;


                            double passTotal = 0;
                            double passTotalASTM = 0;
                            if (isAdj)
                            {
                                if (iso == 1)
                                {
                                    passTotal = passrun2 + passrun3 + passrun4;
                                }


                                if (wod.IncludeASTM == true || iso != 1)
                                {
                                    //passTotal = passrun2 + passrun3;
                                    passTotalASTM = passrun2 + passrun3;
                                }
                            }
                            else
                            {
                                if (iso == 1)
                                {
                                    passTotal = passrun1 + passrun3 + passrun4;
                                }
                                if (wod.IncludeASTM == true || iso != 1)
                                {
                                    //passTotal = passrun2 + passrun3;
                                    passTotalASTM = passrun1 + passrun3;
                                }

                                
                            }

                            if (passTotal != 0 && passrun3 == 1)
                                force.BasicCalibrationResult.InToleranceLeft = "Pass";
                            else
                                force.BasicCalibrationResult.InToleranceLeft = "Fail";

                            if (passTotalASTM != 0 && passrun2 == 1)
                                force.BasicCalibrationResult.InToleranceLeftASTM = "Pass";
                            else
                                force.BasicCalibrationResult.InToleranceLeftASTM = "Fail";


                            if (passrun1 != 0)
                                force.BasicCalibrationResult.InToleranceFound = "Pass";
                            else
                                force.BasicCalibrationResult.InToleranceFound = "Fail";


                            if (passrun2 != 0)
                                force.BasicCalibrationResult.InToleranceAdjusted = "Pass";
                            else
                                force.BasicCalibrationResult.InToleranceAdjusted = "Fail";

                            if (passrun4 != 0)
                                force.BasicCalibrationResult.IntoleranceRun4 = "Pass";
                            else
                                force.BasicCalibrationResult.IntoleranceRun4 = "Fail";
                        }
                        else
                        {

                            if (Math.Abs(maxError) > 1)
                            {
                                force.BasicCalibrationResult.InToleranceLeft = "Fail";
                                force.BasicCalibrationResult.InToleranceLeftASTM = "Fail";
                  
                            }
                            else
                            {
                                force.BasicCalibrationResult.InToleranceLeft = "Pass";
                                force.BasicCalibrationResult.InToleranceLeftASTM = "Pass";
                            }

                         


                            if (Math.Abs(error1_p) > 1)
                                force.BasicCalibrationResult.InToleranceFound = "Fail";
                            else
                                force.BasicCalibrationResult.InToleranceFound = "Pass";

                            if (Math.Abs(error2_p) > 1)
                                force.BasicCalibrationResult.InToleranceAdjusted = "Fail";
                            else
                                force.BasicCalibrationResult.InToleranceAdjusted = "Pass";

                            if (Math.Abs(error4_p) > 1 )
                                force.BasicCalibrationResult.IntoleranceRun4 = "Fail";
                            else
                                force.BasicCalibrationResult.IntoleranceRun4 = "Pass";
                        }


                       
                        forceResult.Add(force);
        
                      

                    }
                  


                    //var forcesDuplicated = forces.Where(x => x.BasicCalibrationResult.FS == force.BasicCalibrationResult.FS && x.BasicCalibrationResult.Nominal == force.BasicCalibrationResult.Nominal).ToList();
                    var forcesDuplicated = forceResult.Where(x=>x.BasicCalibrationResult.Nominal != 0)
                    .GroupBy(x => new { x.BasicCalibrationResult.FS, x.BasicCalibrationResult.Nominal })
                    .Where(group => group.Count() > 1)  // Solo selecciona los grupos con más de 1 elemento (duplicados)
                    .ToList();
                    
                    if (forcesDuplicated != null && forcesDuplicated.Count() > 0)
                    {
                        foreach (var group in forcesDuplicated)
                        {
                            var fs = group.Key.FS;
                            var nominal = group.Key.Nominal;
                            var classTemp1 = group.FirstOrDefault().BasicCalibrationResult.Class1Temp;
                            var classTempTotal = group.FirstOrDefault().BasicCalibrationResult.ClassTemp;

                            bool sameClassRun1 = group.All(x => x.BasicCalibrationResult.ClassRun1 == group.First().BasicCalibrationResult.ClassRun1);
                            bool sameClassTotal = group.All(x => x.BasicCalibrationResult.Class == group.First().BasicCalibrationResult.Class);
                            bool sameClassRun1Temp = group.All(x => x.BasicCalibrationResult.Class1Temp == group.First().BasicCalibrationResult.Class1Temp);
                            bool sameClassTotalTemp = group.All(x => x.BasicCalibrationResult.ClassTemp == group.First().BasicCalibrationResult.ClassTemp);
                            bool firstrender = group.All(x => string.IsNullOrEmpty(x.BasicCalibrationResult.Class1Temp));
                            
                            if (!sameClassRun1 || (firstrender && !sameClassRun1Temp) )
                            {
                                var maxClassRun1 = group.Select(x => x.BasicCalibrationResult.ClassRun1 == "NA" ? 4.0 : double.Parse(x.BasicCalibrationResult.ClassRun1)).Max();
                                    var maxClassGeneral = group.Select(x => x.BasicCalibrationResult.Class == "NA" ? 4.0 : double.Parse(x.BasicCalibrationResult.Class)).Max();

                                    foreach (var duplicated in group)
                                    {
                                        duplicated.BasicCalibrationResult.Class1Temp = duplicated.BasicCalibrationResult.ClassRun1;
                                        duplicated.BasicCalibrationResult.ClassRun1 = maxClassRun1.ToString() == "4" ? "NA" : maxClassRun1.ToString();
                                        duplicated.BasicCalibrationResult.ToastMessageForce = "Class mismatch on test point " + duplicated.BasicCalibrationResult.Nominal;
                                    }
                                
                            }

                            if (!sameClassTotal || (firstrender && !sameClassTotalTemp))
                            {
                                
                                var maxClassGeneral = group.Select(x => x.BasicCalibrationResult.Class == "NA" ? 4.0 : double.Parse(x.BasicCalibrationResult.Class)).Max();

                                foreach (var duplicated in group)
                                {
                                    duplicated.BasicCalibrationResult.ClassTemp = duplicated.BasicCalibrationResult.Class;
                                    duplicated.BasicCalibrationResult.Class = maxClassGeneral.ToString() == "4" ? "NA" : maxClassGeneral.ToString();
                                    duplicated.BasicCalibrationResult.ToastMessageForceTotal = "Class mismatch on test point " + duplicated.BasicCalibrationResult.Nominal;
                                }

                            }
                        }
                        
                    }

                    return forceResult;


                  
                }
                return forces;
            }
            catch (Exception ex)
            {
//                Console.WriteLine("GetCalculatesForISOandASTM Error ", ex.ToString());
                return ISOandASTM.Forces;
            }


        }

       

        public Contributor MapContributor()
        {

            return null;



        }


            public  async Task<ICollection<Force>> CalculateUncertainty(List<Force> forces, int iso, Tolerance? tolerance)
        {
            List<Force> forceResult = new List<Force>();

            List<WeightSet> standards = new List<WeightSet>();
            var id = forces.FirstOrDefault().WorkOrderDetailId;
            CalibrationResultContributor calibrationResultContributors = new CalibrationResultContributor();
            
          
            foreach (var item in forces)
            {
                ICollection<CalibrationResultContributor> resultContributors = new List<CalibrationResultContributor>();
                List<CalibrationResultContributor> resultContributors2 = new List<CalibrationResultContributor>();
                double expandedUncertainty = 0;

                AppState appState = new AppState();
                Dictionary<int, string> distributionUncertList = appState.DistributionUncertList;


                if (item.WeightSets != null)
                {
                    standards = item.WeightSets.ToList();
                    if (standards.Count > 0 && standards != null)
                    {

                        resultContributors = new List<CalibrationResultContributor>();
                        foreach (var standard in standards)
                        {


                            // Get Piece of Equipment contributors
                            var precontributorPoe = await pieceOfEquipmentRepository.GetUncertaintyByPoe(standard.PieceOfEquipmentID);
                            var poe = await pieceOfEquipmentRepository.GetPieceOfEquipmentByID(standard.PieceOfEquipmentID);
                           
                            foreach (var x in precontributorPoe)
                            {
                                 if (item.BasicCalibrationResult.Nominal <= x.RangeMax && item.BasicCalibrationResult.Nominal >= x.RangeMin)
                                //if (item.BasicCalibrationResult.Nominal <= x.WeightNominalValue && item.BasicCalibrationResult.Nominal >= x.WeightNominalValue2)
                                {
                                    CalibrationResultContributor resultContributor = new CalibrationResultContributor();
                                    resultContributor.PieceOfEquipmentID = x.PieceOfEquipmentID;
                                    resultContributor.PieceOfEquipment = poe;
                                    resultContributor.Magnitude = Math.Round(x.Value.Value,3);
                                    resultContributor.Quotient = x.Quotient;
                                    resultContributor.UnitOfMeasure = x.UnitOfMeasure;
                                    resultContributor.Divisor = x.Divisor;
                                    resultContributor.Square = x.Square;
                                    resultContributor.TypeContributor = "Standard";
                                    resultContributor.Distribution =  x.Distribution.GetDistribution(distributionUncertList);
                                    if (x.Description != null)
                                    resultContributor.Description = x.Description;
                                    else
                                        resultContributor.Description = "Standard";
                                    resultContributors.Add(resultContributor);
                                }



                            }


                            // Get Equipment Template contributors
                           // var poe = await pieceOfEquipmentRepository.GetPieceOfEquipmentByID(standard.PieceOfEquipmentID);
                            if (poe != null)
                            {
                                var precontributorET = await pieceOfEquipmentRepository.GetUncertaintyByEt(poe.EquipmentTemplateId);
                                foreach (var x in precontributorET)
                                {
                                    if (item.BasicCalibrationResult.Nominal <= x.RangeMax && item.BasicCalibrationResult.Nominal >= x.RangeMin)
                                    {
                                        CalibrationResultContributor resultContributor = new CalibrationResultContributor();
                                        resultContributor.PieceOfEquipmentID = x.PieceOfEquipmentID;
                                        resultContributor.PieceOfEquipment = poe;
                                        resultContributor.Magnitude = Math.Round(x.Value.Value, 3);
                                        resultContributor.Quotient = x.Quotient;
                                        resultContributor.UnitOfMeasure = x.UnitOfMeasure;
                                        resultContributor.Divisor = x.Divisor;
                                        resultContributor.Square = x.Square;
                                        resultContributor.TypeContributor = "EquipmentTemplate";
                                        resultContributor.Distribution = x.Distribution.GetDistribution(distributionUncertList);
                                        if (x.Description != null)
                                            resultContributor.Description = x.Description;
                                        else
                                            resultContributor.Description = "Standard";
                                        resultContributors.Add(resultContributor);

                                    }

                                }
                            }

                        }
                        double accumContributorsSquare = 0;
                       
                        if (resultContributors.Count() > 0 && resultContributors != null)
                        {

                            //Calculate Uncertainty
                            foreach (var y in resultContributors)
                            {
                                double divisor = 0;
                                if (y.Divisor == 0)
                                    divisor = 1;
                                else
                                    divisor = y.Divisor;
                                double quotient = (y.Magnitude / divisor);
                                y.Quotient = quotient;
                               
                                double square = Math.Pow(quotient, 2);
                               
                                accumContributorsSquare += square;
                                y.Square = square;
                                resultContributors2.Add(y);
                            }

                            double totalUncertainty = Math.Sqrt(accumContributorsSquare);

                            expandedUncertainty = Math.Round(totalUncertainty * 2, 3);//getKFactor();


                        }
                    }
                }
               
                item.BasicCalibrationResult.Uncertanty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(expandedUncertainty)));
                item.CalibrationResultContributors = resultContributors2;
                
                forceResult.Add(item);
            }
            List<Force> resultReturn = new List<Force>();

            if (iso == 1)
                resultReturn = await CalculateUncert(forceResult, iso, tolerance);
            else
                resultReturn = await CalculateUncertASTM(forceResult, iso, tolerance);
            
             
            return resultReturn;

        }
        public static decimal RoundFirstSignificantDigit(decimal numberInit)
        {
            try
            {
                ///
                if (numberInit == 0)
                    return 0;
                int precision = 0;

                long ints = 0;
                string numberEnd;

                decimal val = numberInit;
                decimal aux;
                string auxchar;

                while (Math.Abs(val) < 1)
                {
                    val *= 10;
                    precision++;
                }

                var hh = (long)numberInit;
                var mm = Math.Log10(hh);
                ints = (long)Math.Abs(numberInit); //(int)Math.Floor(Math.Log10(cedula) + 1);// len(abs(cast(@numberInit as bigint)))
                ints = ints.ToString().Length;

                if (precision < 1)
                {
                    if ((2 - ints) < 0)

                        precision = (int)ints;
                    else
                        precision = (int)(2 + (2 - ints));

                }

                else
                    precision = precision + 3;
                aux = Math.Round(Convert.ToDecimal(numberInit), precision - 2);
                auxchar = aux.ToString();

                numberEnd = auxchar.ToString().Substring(0, precision);
                var ssub = auxchar.ToString().Substring(0, precision);//substring(@auxchar, 1, @precision)
                return Convert.ToDecimal(numberEnd);
            }
            catch (Exception ex)
            {
//                Console.WriteLine(ex);
                return Math.Round(numberInit, 2);
            }
        }

        public async Task<List<Force>> CalculateUncert(List<Force> forces, int? iso, Tolerance tolerance)
         {
            List<Force> forceResult = new List<Force>();

            bool isAdj = true;
            if (forces.Sum(x => x.BasicCalibrationResult.RUN2) != 0)
                isAdj = true;
            else
                isAdj = false;
            foreach (var item in forces)
            {
                var force = item;
                //*****************


                double verificationReading2 = 0;

                double verificationReading1 = item.BasicCalibrationResult.RUN1;
                if (force.BasicCalibrationResult.RUN2 != 0)
                    verificationReading2 = item.BasicCalibrationResult.RUN2;
                else
                    verificationReading2 = item.BasicCalibrationResult.RUN3;
                double nominal = item.BasicCalibrationResult.Nominal;


                double Run1Delta = Math.Abs(nominal-verificationReading1);  //todo
                double Run2Delta = Math.Abs(nominal-verificationReading2);  //todo
                double minDelta = 0;
                double maxDelta = 0;
                double worstCaseDelta = 0;
                double deltaRepeatability = 0;

                //********** I Actual vs Reading
                //1. Min, Max
                if (Run1Delta < Run2Delta)
                {
                    minDelta = Run1Delta;
                    maxDelta = Run2Delta;
                }
                else
                {
                    minDelta = Run2Delta;
                    maxDelta = Run1Delta;
                }

                //2.Worst Case Delta
                if (minDelta < maxDelta)
                    worstCaseDelta = maxDelta;
                else
                    worstCaseDelta = minDelta;

                //3.Delta (Repeatability)
                deltaRepeatability = Run1Delta - Run2Delta;

                //********** II Actual vs Reading (Percent)
                double Run1Delta_p = Math.Abs(Run1Delta/verificationReading1);
                double Run2Delta_p = Math.Abs(Run2Delta/verificationReading2);
                double minDelta_p = 0;
                double maxDelta_p = 0;
                double worstCaseDelta_p = 0;
                double deltaRepeatability_p = 0;

                Run1Delta_p = Run1Delta - verificationReading1;
                Run2Delta_p = Run2Delta - verificationReading2;


                //1. Min, Max
                if (Run1Delta_p < Run2Delta_p)
                {
                    minDelta_p = Run1Delta_p;
                    maxDelta_p = Run2Delta_p;
                }
                else
                {
                    minDelta_p = Run2Delta_p;
                    maxDelta_p = Run1Delta_p;
                }

                //2.Worst Case Delta
                if (minDelta_p < maxDelta_p)
                    worstCaseDelta_p = maxDelta_p;
                else
                    worstCaseDelta_p = minDelta_p;

                //3.Delta (Repeatability)
                deltaRepeatability_p = Run1Delta_p - Run2Delta_p;

                //************ III Repeatability
                double repeatability = 0;
                double repeatabilityUncert = 0;

                repeatability = deltaRepeatability;// deltaRepeatability/100 * nominal;
                force.BasicCalibrationResult.Repeatability = repeatability;
                forceResult.Add(force);
            }

            //Calculate Repeteability Uncertainty
            //
            //

            int cont = 0;
            double repeteabilityUncert = 0;
            double sqrtRep = 0;
            double sqrtRes = 0;
            List<Force> Tension= new List<Force>();
            List<Force> Compresion = new List<Force>();
            List<Force> Universal = new List<Force>();
            var forcesTension = forces.Where(x => (x.CalibrationSubTypeId == 4 && iso == 1));
            var forcesTensionASTM = forces.Where(x => x.CalibrationSubTypeId == 6);
            var forcesCompression = forces.Where(x => (x.CalibrationSubTypeId == 5 && iso == 1));
            var forcesCompressionASTM = forces.Where(x => x.CalibrationSubTypeId == 7);
            var forcesUniversal = forces.Where(x => (x.CalibrationSubTypeId == 8 && iso == 1));
            var forcesUniversalASTM = forces.Where(x => x.CalibrationSubTypeId == 9);
            if (iso == 1)
            {
                Tension = forceResult.Where(x => x.CalibrationSubTypeId == 4).OrderBy(y=>y.BasicCalibrationResult.Position).ToList();
                Compresion = forceResult.Where(x => x.CalibrationSubTypeId == 5).OrderBy(y => y.BasicCalibrationResult.Position).ToList();
                Universal = forceResult.Where(x => x.CalibrationSubTypeId == 8).OrderBy(y => y.BasicCalibrationResult.Position).ToList();
            }
            else
            {
                Tension = forceResult.Where(x => x.CalibrationSubTypeId == 6).OrderBy(y => y.BasicCalibrationResult.Position).ToList();
                Compresion = forceResult.Where(x => x.CalibrationSubTypeId == 7).OrderBy(y => y.BasicCalibrationResult.Position).ToList();
                Universal = forceResult.Where(x => x.CalibrationSubTypeId == 9).OrderBy(y => y.BasicCalibrationResult.Position).ToList();

            }
           
            List<Force> forceResult1 = new List<Force>();
            try
            {

                //Caculate Tension
                for (int i = 0; i < Tension.Count(); i++)
                {
                    double stddevtemp = 0;
                    double stddev = 0;
                    double mean = 0;
                    
                    CalibrationResultContributor resultContributor = new CalibrationResultContributor();
                    double nominal = Tension[i].BasicCalibrationResult.Nominal;
                    var force = Tension[i];
                    if (iso == 1)  // Repeatability (std dev of 3 readings)
                    {
                        if (isAdj)
                        {
                            mean = (force.BasicCalibrationResult.RUN2 + force.BasicCalibrationResult.RUN3 + force.BasicCalibrationResult.RUN4) / 3;
                            var res = (force.BasicCalibrationResult.RUN2 - mean) * (force.BasicCalibrationResult.RUN2 - mean) + (force.BasicCalibrationResult.RUN3 - mean) * (force.BasicCalibrationResult.RUN3 - mean) + (force.BasicCalibrationResult.RUN4 - mean) * (force.BasicCalibrationResult.RUN4 - mean);
                            var variance = res / 2;
                            stddev = Math.Sqrt(Math.Round(variance, 6));
                        }
                        else
                        {
                            mean = (force.BasicCalibrationResult.RUN1 + force.BasicCalibrationResult.RUN3 + force.BasicCalibrationResult.RUN4) / 3;
                            var res = (force.BasicCalibrationResult.RUN1 - mean) * (force.BasicCalibrationResult.RUN1 - mean) + (force.BasicCalibrationResult.RUN3 - mean) * (force.BasicCalibrationResult.RUN3 - mean) + (force.BasicCalibrationResult.RUN4 - mean) * (force.BasicCalibrationResult.RUN4 - mean);
                            var variance = res / 2;
                            stddev = Math.Sqrt(variance);

                        }


                        resultContributor = new CalibrationResultContributor();
                        sqrtRep = sqrtRep + Math.Pow(stddev, 2);
                        resultContributor.Square = sqrtRep;

                        resultContributor.Description = "Repeatability";
                        force.CalibrationResultContributors.Add(resultContributor);

                    }
                   

                    //Resolution Contributor Tension
                    if (i+1 == force.BasicCalibrationResult.Position )
                    {
                        resultContributor = new CalibrationResultContributor();
                        var res = force.BasicCalibrationResult.Resolution / 3.46;
                        sqrtRes = Math.Pow(res, 2);
                        resultContributor.Square = sqrtRes;
                        resultContributor.Description = "Resolution";
                        resultContributor.TypeContributor = "Resolution";
                        resultContributor.Distribution = "Resolution";
                        resultContributor.Magnitude = Math.Round(force.BasicCalibrationResult.Resolution, 3);
                        force.CalibrationResultContributors.Add(resultContributor);
                    }

                    //Resolution at 0 Contributor Tension

                    resultContributor = new CalibrationResultContributor();
                    var res0 = Tension[0].BasicCalibrationResult.Resolution / 3.46;
                    sqrtRes = Math.Pow(res0, 2);
                    resultContributor.Square = sqrtRes;
                    resultContributor.Description = "Resolution at 0 ";
                    resultContributor.TypeContributor = "Resolution at 0";
                    resultContributor.Magnitude = Math.Round(Tension[0].BasicCalibrationResult.Resolution,3);
                    resultContributor.Distribution = "Resolution";
                    
                    force.CalibrationResultContributors.Add(resultContributor);
                    // 


                    repeteabilityUncert = Math.Sqrt(sqrtRep / 10);
                    cont = 0;

                    //************ IV Repeatability & Reproducbility
                    //1. Uncertainty (%) Magnitude
                    double repetUncertMagnitude = stddev;
                    //if (!double.IsInfinity(repeteabilityUncert) && !double.IsInfinity(nominal) && nominal!=0)
                    //   repetUncertMagnitude = (repeteabilityUncert / nominal) * 100;
                    double divisor = 3.46;
                    double quotient = 0;
                    double squared = 0;

                    //2. Quotient
                    if (divisor != 0)
                        quotient = repetUncertMagnitude / divisor;

                    //3. squared
                    squared = Math.Pow(quotient, 2);

                    //************ V Squared Attributes
                    //1. Standard Load Cell Non Linearity
                    //Note: This section is the UncertaintyValue of  WeightSets Object, Is don't calculate


                    //************ VI Uncertainty Calculations
                    //1.Total Uncertainty (sq. root of sum of sq)
                    double sqroot = 0;
                    double expandedUncertainty = 0;
                    cont = 0;


                    sqroot = squared; //here is sum of AR21:AZ21 but this fields are not calculated 

                    //2.Expanded Uncertainty
                    expandedUncertainty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(sqroot * 2)));
                    
                   
                   
                

                foreach (var item1 in force.CalibrationResultContributors.Where(x=>x.TypeContributor==null))
                    {
                        item1.Quotient = quotient;
                        
                        item1.Divisor = 1;
                        //item1.Square = squared;
                        item1.Magnitude = Math.Round(repetUncertMagnitude,3);
                        item1.TypeContributor = "Repeatability";
                        //item1.Description = "Repeatability";
                        item1.Distribution = "Normal";
                        
                    }

                    foreach (var item1 in force.CalibrationResultContributors.Where(x => x.TypeContributor.Contains("Resolution")))
                    {
                        item1.Quotient = quotient;
                        item1.Divisor = 3.46;
                        //item1.Square = squared;
                     
                    }
                    //************ VII FINAL PERCENT UNCERTAINTY OF VAALUES 
                    double uncertaintyvalue = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(expandedUncertainty))); 


                    if (Double.IsNaN(force.Uncertainty))
                        force.Uncertainty = 0;

                    force.Uncertainty = force.Uncertainty + uncertaintyvalue;
                    force.BasicCalibrationResult.Uncertanty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(force.Uncertainty)));

                    forceResult1.Add(force);

                }

                //Caculate Compression
                sqrtRep = 0;
                for (int i = 0; i < Compresion.Count(); i++)
                {
                    double stddevtemp = 0;
                    double stddev = 0;
                    double mean = 0;

                    CalibrationResultContributor resultContributor = new CalibrationResultContributor();
                    double nominal = Compresion[i].BasicCalibrationResult.Nominal;
                    var force = Compresion[i];

                    if (iso == 1)  // Repeatability (std dev of 3 readings)
                    {
                        if (isAdj)
                        {
                            mean = (force.BasicCalibrationResult.RUN2 + force.BasicCalibrationResult.RUN3 + force.BasicCalibrationResult.RUN4) / 3;
                            var res = (force.BasicCalibrationResult.RUN2 - mean) * (force.BasicCalibrationResult.RUN2 - mean) + (force.BasicCalibrationResult.RUN3 - mean) * (force.BasicCalibrationResult.RUN3 - mean) + (force.BasicCalibrationResult.RUN4 - mean) * (force.BasicCalibrationResult.RUN4 - mean);
                            var variance = res / 2;
                            stddev = Math.Sqrt(Math.Round(variance, 6));
                        }
                        else
                        {
                            mean = (force.BasicCalibrationResult.RUN1 + force.BasicCalibrationResult.RUN3 + force.BasicCalibrationResult.RUN4) / 3;
                            var res = (force.BasicCalibrationResult.RUN1 - mean) * (force.BasicCalibrationResult.RUN1 - mean) + (force.BasicCalibrationResult.RUN3 - mean) * (force.BasicCalibrationResult.RUN3 - mean) + (force.BasicCalibrationResult.RUN4 - mean) * (force.BasicCalibrationResult.RUN4 - mean);
                            var variance = res / 2;
                            stddev = Math.Sqrt(variance);

                        }

                        resultContributor = new CalibrationResultContributor();
                        sqrtRep = sqrtRep + Math.Pow(stddev, 2);
                        resultContributor.Square = sqrtRep;

                        resultContributor.Description = "Repeatability";
                        force.CalibrationResultContributors.Add(resultContributor);

                    }
                  
                    //Resolution Contributor Compression
                    if (i + 1 == force.BasicCalibrationResult.Position)
                    {
                        resultContributor = new CalibrationResultContributor();
                        var res = force.BasicCalibrationResult.Resolution / 3.46;
                        sqrtRes = Math.Pow(res, 2);
                        resultContributor.Square = sqrtRes;
                        resultContributor.Description = "Resolution";
                        resultContributor.TypeContributor = "Resolution";
                        resultContributor.Distribution = "Resolution";
                        resultContributor.Magnitude = Math.Round(force.BasicCalibrationResult.Resolution,3);
                        force.CalibrationResultContributors.Add(resultContributor);
                    }

                    //Resolution at 0 Contributor Compression

                    resultContributor = new CalibrationResultContributor();
                    var res0 = Compresion[0].BasicCalibrationResult.Resolution / 3.46;
                    sqrtRes = Math.Pow(res0, 2);
                    resultContributor.Square = sqrtRes;
                    resultContributor.Description = "Resolution at 0 ";
                    resultContributor.TypeContributor = "Resolution at 0";
                    resultContributor.Magnitude = Math.Round(Compresion[0].BasicCalibrationResult.Resolution,3);
                    resultContributor.Distribution = "Resolution";
                    
                    force.CalibrationResultContributors.Add(resultContributor);
                    // 


                    repeteabilityUncert = Math.Sqrt(sqrtRep / 10);
                    cont = 0;

                    //************ IV Repeatability & Reproducbility
                    //1. Uncertainty (%) Magnitude
                    double repetUncertMagnitude = stddev;
                    //if (!double.IsInfinity(repeteabilityUncert) && !double.IsInfinity(nominal) && nominal != 0)
                    //{
                    //    repetUncertMagnitude = (repeteabilityUncert / nominal) * 100;
                    //}
                    double divisor = 3.46;
                    double quotient = 0;
                    double squared = 0;

                    //2. Quotient
                    if (divisor != 0)
                        quotient = repetUncertMagnitude / divisor;

                    //3. squared
                    squared = Math.Pow(quotient, 2);

                    //************ V Squared Attributes
                    //1. Standard Load Cell Non Linearity
                    //Note: This section is the UncertaintyValue of  WeightSets Object, Is don't calculate


                    //************ VI Uncertainty Calculations
                    //1.Total Uncertainty (sq. root of sum of sq)
                    double sqroot = 0;
                    double expandedUncertainty = 0;
                    cont = 0;


                    sqroot = squared; //here is sum of AR21:AZ21 but this fields are not calculated 

                    //2.Expanded Uncertainty
                    expandedUncertainty =  ((double)RoundFirstSignificantDigit(Convert.ToDecimal(sqroot * 2)));


                    foreach (var item1 in force.CalibrationResultContributors.Where(x => x.TypeContributor == null))
                    {
                        item1.Quotient = quotient;
                        item1.Divisor = 1;
                        //  item1.Square = squared;
                        item1.Magnitude = repetUncertMagnitude;
                        item1.TypeContributor = "Repeatability";
                        //item1.Description = "Repeatability";
                        item1.Distribution = "Normal";
                    }

                    //************ VII FINAL PERCENT UNCERTAINTY OF VAALUES 
                    double uncertaintyvalue = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(expandedUncertainty)));
            
                    if (Double.IsNaN(force.Uncertainty))
                        force.Uncertainty = 0;
                    force.Uncertainty = force.Uncertainty + uncertaintyvalue;

                    force.BasicCalibrationResult.Uncertanty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(force.Uncertainty))); 
                    forceResult1.Add(force);

                }


                //Universal ******
                for (int i = 0; i < Universal.Count(); i++)
            {
                    CalibrationResultContributor resultContributor = new CalibrationResultContributor();
                    double nominal = Universal[i].BasicCalibrationResult.Nominal;
                var force = Universal[i];
                    double stddevtemp = 0;
                    double stddev = 0;
                    double mean = 0;
                    if (iso == 1)  // Repeatability (std dev of 3 readings)
                    {
                        if (isAdj)
                        {
                            mean = (force.BasicCalibrationResult.RUN2 + force.BasicCalibrationResult.RUN3 + force.BasicCalibrationResult.RUN4) / 3;
                            var res = (force.BasicCalibrationResult.RUN2  - mean ) * (force.BasicCalibrationResult.RUN2 - mean) + (force.BasicCalibrationResult.RUN3 - mean) * (force.BasicCalibrationResult.RUN3 - mean) + (force.BasicCalibrationResult.RUN4 - mean) * (force.BasicCalibrationResult.RUN4 - mean);
                            var variance = res / 2;
                            stddev = Math.Sqrt(Math.Round(variance,6));
                        }
                        else
                        {
                            mean = (force.BasicCalibrationResult.RUN1 + force.BasicCalibrationResult.RUN3 + force.BasicCalibrationResult.RUN4) / 3;
                            var res = (force.BasicCalibrationResult.RUN1 - mean) * (force.BasicCalibrationResult.RUN1 - mean) + (force.BasicCalibrationResult.RUN3 - mean) * (force.BasicCalibrationResult.RUN3 - mean) + (force.BasicCalibrationResult.RUN4 - mean) * (force.BasicCalibrationResult.RUN4 - mean);
                            var variance = res / 2;
                            stddev = Math.Sqrt(variance);

                        }

                        resultContributor = new CalibrationResultContributor();
                        sqrtRep = sqrtRep + Math.Pow(stddev, 2);
                        resultContributor.Square = sqrtRep;
                        resultContributor.Description = "Repeatability";
                        force.CalibrationResultContributors.Add(resultContributor);

                    }
                    
                    //Resolution Contributor Universal
                    if (i + 1 == force.BasicCalibrationResult.Position)
                    {
                        resultContributor = new CalibrationResultContributor();
                        var res = force.BasicCalibrationResult.Resolution / 3.46;
                        sqrtRes = Math.Pow(res, 2);
                        resultContributor.Square = sqrtRes;
                        resultContributor.Description = "Resolution";
                        resultContributor.TypeContributor = "Resolution";
                        resultContributor.Distribution = "Resolution";
                        resultContributor.Magnitude = force.BasicCalibrationResult.Resolution;
                        force.CalibrationResultContributors.Add(resultContributor);
                    }

                    //Resolution at 0 Contributor Universal

                    resultContributor = new CalibrationResultContributor();
                    var res0 = Universal[0].BasicCalibrationResult.Resolution / 3.46;
                    sqrtRes = Math.Pow(res0, 2);
                    resultContributor.Square = sqrtRes;
                    resultContributor.Description = "Resolution at 0 ";
                    resultContributor.TypeContributor = "Resolution at 0";
                    resultContributor.Distribution = "Resolution";
                    resultContributor.Magnitude = Universal[0].BasicCalibrationResult.Resolution;
                    force.CalibrationResultContributors.Add(resultContributor);
                    // 


                    repeteabilityUncert = Math.Sqrt(sqrtRep / 10);
                cont = 0;

                //************ IV Repeatability & Reproducbility
                //1. Uncertainty (%) Magnitude
                double repetUncertMagnitude = stddev;
                    //if (!double.IsInfinity(stddev) && !double.IsInfinity(nominal) && nominal != 0)
                    //    repetUncertMagnitude = //(repeteabilityUncert / nominal) * 100;
                   
                double divisor = 3.46;
                double quotient = 0;
                double squared = 0;

                //2. Quotient
                if (divisor != 0)
                quotient = repetUncertMagnitude / divisor;
                else
                        quotient = 0;
                    //3. squared
                    squared = Math.Pow(quotient, 2);

                //************ V Squared Attributes
                //1. Standard Load Cell Non Linearity
                //Note: This section is the UncertaintyValue of  WeightSets Object, Is don't calculate


                //************ VI Uncertainty Calculations
                //1.Total Uncertainty (sq. root of sum of sq)
                double sqroot = 0;
                double expandedUncertainty = 0;
                cont = 0;


                    sqroot = squared; //here is sum of AR21:AZ21 but this fields are not calculated 

                //2.Expanded Uncertainty
                expandedUncertainty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(sqroot * 2))); 

                //************* Add Contributors

                foreach (var item1 in force.CalibrationResultContributors.Where(x => x.TypeContributor == null))
                    { 
                item1.Quotient = quotient;
                        item1.Divisor = 1;
                        //    item1.Square = squared;
                        item1.Magnitude = repetUncertMagnitude;
                        item1.TypeContributor = "Repeatability";
                        //item1.Description = "Repeatability";
                        item1.Distribution = "Normal";
                    }
                    //force.CalibrationResultContributors.Add(resultContributor);

                //************ VII FINAL PERCENT UNCERTAINTY OF VAALUES 
                double uncertaintyvalue = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(expandedUncertainty)));
                    if (Double.IsNaN(force.Uncertainty))
                        force.Uncertainty = 0;
                    force.Uncertainty = force.Uncertainty + uncertaintyvalue;

                    force.BasicCalibrationResult.Uncertanty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(force.Uncertainty)));
                    forceResult1.Add(force);

             }

                
            }
            catch (Exception ex)
            {

            }
   
            foreach (var item in forceResult1)
            {
                double xpandedUncert = 0;
                double sumSquares = 0;
                
                foreach (var _item in item.CalibrationResultContributors)
                {
                    if (_item.Distribution == "Resolution")
                    {
                        double divisor = 3.46;
                        _item.Divisor = divisor;
                        double quotient = (_item.Magnitude / divisor);
                        _item.Quotient = quotient;

                        double square = Math.Pow(quotient, 2);
                        _item.Square = square;

                    }
                    sumSquares = (sumSquares + _item.Square);
                    var totalUncert = Math.Sqrt(sumSquares);
                    xpandedUncert = Math.Round(totalUncert * 2, 5);
                   
                }

                item.BasicCalibrationResult.Uncertanty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(xpandedUncert))); 
                item.Uncertainty = xpandedUncert;

                ///TUR
                ///
                if (tolerance != null)
                {
                    var lowTolerance = ValidTolerance(item.BasicCalibrationResult.Nominal, (int)tolerance.ToleranceTypeID, tolerance.Resolution, tolerance.AccuracyPercentage, "low");
                    var maxTolerance = ValidTolerance(item.BasicCalibrationResult.Nominal, (int)tolerance.ToleranceTypeID, tolerance.Resolution, tolerance.AccuracyPercentage, "max");
                    var toleranceDifference = maxTolerance - lowTolerance;
                    var uncertx2 = (xpandedUncert * 2);
                    if (uncertx2 == 0)
                    {
                        uncertx2 = 1;
                    }

                    var tur = (toleranceDifference / uncertx2);
                    item.BasicCalibrationResult.TUR = tur;
                }
            }
            
            return forceResult1;
        }

        public async Task<List<Force>> CalculateUncertASTM(List<Force> forces, int? iso, Tolerance? tolerance)
        {
            List<Force> forceResult = new List<Force>();
            List<Force> forceResultAux = new List<Force>();
            bool isAdj;
            if (forces.Sum(x => x.BasicCalibrationResult.RUN2) != 0)
                isAdj = true;
            else
                isAdj = false;

            double adjTension = 0;
            double adjCompression = 0;
            double adjUniversal = 0;

            var forcesTension = forces.Where(x => (x.CalibrationSubTypeId == 4 && iso == 1)).OrderBy(y => y.BasicCalibrationResult.Position);
            var forcesTensionASTM = forces.Where(x => x.CalibrationSubTypeId == 6).OrderBy(y => y.BasicCalibrationResult.Position);
            var forcesCompression = forces.Where(x => (x.CalibrationSubTypeId == 5 && iso == 1)).OrderBy(y => y.BasicCalibrationResult.Position);
            var forcesCompressionASTM = forces.Where(x => x.CalibrationSubTypeId == 7).OrderBy(y => y.BasicCalibrationResult.Position);
            var forcesUniversal = forces.Where(x => (x.CalibrationSubTypeId == 8 && iso == 1)).OrderBy(y => y.BasicCalibrationResult.Position);
            var forcesUniversalASTM = forces.Where(x => x.CalibrationSubTypeId == 9).OrderBy(y => y.BasicCalibrationResult.Position);
            double repeatabilityASTM = 0;

            double errorRun1Per = 0;
            double errorRun2Per = 0;
            double errorRun3Per = 0;
            double errorRun4Per = 0;
            foreach (var item in forces)
            {

                var force = item;
                //****************************************************************

                double verificationReading2 = 0;

                double verificationReading1 = item.BasicCalibrationResult.RUN1;
                if (force.BasicCalibrationResult.RUN2 != 0)
                    verificationReading2 = item.BasicCalibrationResult.RUN2;
                else
                    verificationReading2 = item.BasicCalibrationResult.RUN3;
                double nominal = item.BasicCalibrationResult.Nominal;


                double Run1Delta = Math.Abs(nominal - verificationReading1);  //todo
                double Run2Delta = Math.Abs(nominal - verificationReading2);  //todo
                double minDelta = 0;
                double maxDelta = 0;
                double worstCaseDelta = 0;
                double deltaRepeatability = 0;

                //********** I Actual vs Reading
                //1. Min, Max
                if (Run1Delta < Run2Delta)
                {
                    minDelta = Run1Delta;
                    maxDelta = Run2Delta;
                }
                else
                {
                    minDelta = Run2Delta;
                    maxDelta = Run1Delta;
                }

                //2.Worst Case Delta
                if (minDelta < maxDelta)
                    worstCaseDelta = maxDelta;
                else
                    worstCaseDelta = minDelta;

                //3.Delta (Repeatability)
                deltaRepeatability = Run1Delta - Run2Delta;

                //********** II Actual vs Reading (Percent)
                double Run1Delta_p = Math.Abs(Run1Delta / verificationReading1);
                double Run2Delta_p = Math.Abs(Run2Delta / verificationReading2);
                double minDelta_p = 0;
                double maxDelta_p = 0;
                double worstCaseDelta_p = 0;
                double deltaRepeatability_p = 0;

                Run1Delta_p = Run1Delta - verificationReading1;
                Run2Delta_p = Run2Delta - verificationReading2;


                //1. Min, Max
                if (Run1Delta_p < Run2Delta_p)
                {
                    minDelta_p = Run1Delta_p;
                    maxDelta_p = Run2Delta_p;
                }
                else
                {
                    minDelta_p = Run2Delta_p;
                    maxDelta_p = Run1Delta_p;
                }

                //2.Worst Case Delta
                if (minDelta_p < maxDelta_p)
                    worstCaseDelta_p = maxDelta_p;
                else
                    worstCaseDelta_p = minDelta_p;

                //3.Delta (Repeatability)
                deltaRepeatability_p = Run1Delta_p - Run2Delta_p;

                //************ III Repeatability
                double repeatability = 0;
                double repeatabilityUncert = 0;


                /////////////////////////////////////////
                ///Repeatability based on Report LTIReport
                ///Begin
                ///

                int contAsFound = 0;

               
                errorRun1Per = Math.Round((item.BasicCalibrationResult.ErrorRun1 / item.BasicCalibrationResult.Nominal) * 100, 3);

                if (double.IsNaN(errorRun1Per) || double.IsInfinity(errorRun1Per))
                    errorRun1Per = 0;

                errorRun2Per = Math.Round((item.BasicCalibrationResult.ErrorRun2 / item.BasicCalibrationResult.Nominal) * 100, 3);

                if (double.IsNaN(errorRun2Per) || double.IsInfinity(errorRun2Per))
                    errorRun2Per = 0;

                errorRun3Per = Math.Round((item.BasicCalibrationResult.ErrorRun3 / item.BasicCalibrationResult.Nominal) * 100, 3);
                if (double.IsNaN(errorRun3Per) || double.IsInfinity(errorRun3Per))
                    errorRun3Per = 0;

                 errorRun4Per = Math.Round((item.BasicCalibrationResult.ErrorRun4 / item.BasicCalibrationResult.Nominal) * 100, 3);
                if (double.IsNaN(errorRun4Per) || double.IsInfinity(errorRun4Per))
                    errorRun4Per = 0;

        
                

                var tension = forcesTension;

                ///////
                ///
                if (forcesTension.Count() == 0 && forcesTensionASTM.Count() > 0)
                {
                    tension = forcesTensionASTM;

                }
                adjTension = tension.Sum(x => x.BasicCalibrationResult.RUN2);

                if (isAdj)
                {
                    repeatabilityASTM = Math.Round(item.BasicCalibrationResult.RUN3 - item.BasicCalibrationResult.RUN2, 3);//Math.Round(errorRun3Per - errorRun2Per, 3);

                }
                else
                {
                    repeatabilityASTM = Math.Round(item.BasicCalibrationResult.RUN3 - item.BasicCalibrationResult.RUN1, 3);
                }
                force.BasicCalibrationResult.Repeatability = repeatabilityASTM;

                ///////////////////////////////////
                ///End

                forceResult.Add(force);

            }

            //Calculate Repeteability Uncertainty
            //
            //

            int cont = 0;
            double repeteabilityUncert = 0;
            double sqrtRep = 0;
            double sqrtRes = 0;
            List<Force> Tension = new List<Force>();
            List<Force> Compresion = new List<Force>();
            List<Force> Universal = new List<Force>();
            //var forcesTensionASTM = forces.Where(x => x.CalibrationSubTypeId == 4);
            //var forcesCompressionASTM = forces.Where(x => x.CalibrationSubTypeId == 5);
            //var forcesUniversalASTM = forces.Where(x => x.CalibrationSubTypeId == 8);

            if (iso == 1)
            {
                Tension = forceResult.Where(x => x.CalibrationSubTypeId == 4).OrderBy(y => y.BasicCalibrationResult.Position).ToList();
                Compresion = forceResult.Where(x => x.CalibrationSubTypeId == 5).OrderBy(y => y.BasicCalibrationResult.Position).ToList();
                Universal = forceResult.Where(x => x.CalibrationSubTypeId == 8).OrderBy(y => y.BasicCalibrationResult.Position).ToList();
            }
            else
            {
                Tension = forceResult.Where(x => x.CalibrationSubTypeId == 6).OrderBy(y => y.BasicCalibrationResult.Position).ToList();
                Compresion = forceResult.Where(x => x.CalibrationSubTypeId == 7).OrderBy(y => y.BasicCalibrationResult.Position).ToList();
                Universal = forceResult.Where(x => x.CalibrationSubTypeId == 9).OrderBy(y => y.BasicCalibrationResult.Position).ToList();

            }

            List<Force> forceResult1 = new List<Force>();
            try
            {

                //Caculate Tension
                for (int i = 0; i < Tension.Count(); i++)
                {
                    double stddevtemp = 0;
                    double stddev = 0;
                    double mean = 0;

                    CalibrationResultContributor resultContributor = new CalibrationResultContributor();
                    double nominal = Tension[i].BasicCalibrationResult.Nominal;
                    var force = Tension[i];
                    var UncertaintyPre = force.Uncertainty;
                    

                    var passfailASTM = "Pass";
                    if (Math.Abs(repeatabilityASTM) > 1)
                        passfailASTM = "Fail";
                    else
                        passfailASTM = Tension[i].BasicCalibrationResult.InToleranceLeftASTM;

                   
                    //////
                    if (i == 0 || i == 1)
                        {
                            sqrtRep = 0;
                            for (int j = 1; j < 6; j++)
                            {
                                resultContributor = new CalibrationResultContributor();
                            sqrtRep = sqrtRep + Math.Pow((Tension[j].BasicCalibrationResult.Repeatability), 2);
                            resultContributor.Description = "Repeatability ";

                        }
                        
                    }
                        else if (i == Tension.Count() - 1 || i == Tension.Count() || i == Tension.Count() - 2)
                        {
                            sqrtRep = 0;
                            for (int j = Tension.Count() - 6; j < Tension.Count()-1; j++)
                            {
                                resultContributor = new CalibrationResultContributor();
                            sqrtRep = sqrtRep + Math.Pow((Tension[j].BasicCalibrationResult.Repeatability), 2);
                            resultContributor.Description = "Repeatability ";
                        }
                       
                    }
                        else
                        {

                            sqrtRep = 0;
                            for (int j = i - 2; j <= i + 2; j++)
                            {
                                resultContributor = new CalibrationResultContributor();
                            sqrtRep = sqrtRep + Math.Pow((Tension[j].BasicCalibrationResult.Repeatability ), 2);
                            resultContributor.Description = "Repeatability ";
                        }
                        
                    }


                    force.CalibrationResultContributors.Add(resultContributor);

                    //Resolution Contributor Tension
                    if (i + 1 == force.BasicCalibrationResult.Position)
                    {
                        resultContributor = new CalibrationResultContributor();
                        var res = force.BasicCalibrationResult.Resolution / 3.46;
                        sqrtRes = Math.Pow(res, 2);
                        resultContributor.Square = sqrtRes;
                        resultContributor.Description = "Resolution";
                        resultContributor.TypeContributor = "Resolution";
                        resultContributor.Distribution = "Resolution";
                        resultContributor.Magnitude = force.BasicCalibrationResult.Resolution;
                        force.CalibrationResultContributors.Add(resultContributor);
                    }

                    //Resolution at 0 Contributor Tension

                    resultContributor = new CalibrationResultContributor();
                    var res0 = Tension[0].BasicCalibrationResult.Resolution / 3.46;
                    sqrtRes = Math.Pow(res0, 2);
                    resultContributor.Square = sqrtRes;
                    resultContributor.Description = "Resolution at 0 ";
                    resultContributor.TypeContributor = "Resolution at 0";
                    resultContributor.Magnitude = Tension[0].BasicCalibrationResult.Resolution;
                    resultContributor.Distribution = "Resolution";

                    force.CalibrationResultContributors.Add(resultContributor);
                    // 


                    repeteabilityUncert = Math.Sqrt(sqrtRep / 10);
                    cont = 0;

                    //************ IV Repeatability & Reproducbility
                    //1. Uncertainty (%) Magnitude
                    double repetUncertMagnitude = repeteabilityUncert;
                    //if (!double.IsInfinity(repeteabilityUncert) && !double.IsInfinity(nominal) && nominal != 0)
                    //    repetUncertMagnitude = (repeteabilityUncert / nominal) * 100;
                    double divisor =1;
                    double quotient = 0;
                    double squared = 0;

                    //2. Quotient
                    if (divisor != 0)
                        quotient = repetUncertMagnitude / divisor;

                    //3. squared
                    squared = Math.Pow(quotient, 2);

                    //************ V Squared Attributes
                    //1. Standard Load Cell Non Linearity
                    //Note: This section is the UncertaintyValue of  WeightSets Object, Is don't calculate


                    //************ VI Uncertainty Calculations
                    //1.Total Uncertainty (sq. root of sum of sq)
                    double sqroot = 0;
                    double expandedUncertainty = 0;
                    cont = 0;


                    sqroot = squared; //here is sum of AR21:AZ21 but this fields are not calculated 

                    //2.Expanded Uncertainty
                    expandedUncertainty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(sqroot * 2)));




                    foreach (var item1 in force.CalibrationResultContributors.Where(x => x.TypeContributor == null))
                    {
                        item1.Quotient = quotient;

                        item1.Divisor = 1;
                        //item1.Square = squared;
                        item1.Magnitude = repetUncertMagnitude;
                        item1.TypeContributor = "Repeatability";
                       // item1.Description = "Repeatability";
                        item1.Distribution = "Normal";
                        item1.Square = repetUncertMagnitude * repetUncertMagnitude;// squared;
                    }

                    foreach (var item1 in force.CalibrationResultContributors.Where(x => x.TypeContributor.Contains("Resolution")))
                    {
                        item1.Quotient = quotient;
                        item1.Divisor = 3.46;
                        //item1.Square = squared;

                    }
                    //************ VII FINAL PERCENT UNCERTAINTY OF VAALUES 
                    double uncertaintyvalue = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(expandedUncertainty)));


                    if (Double.IsNaN(UncertaintyPre))
                        force.UncertaintyASTM = 0;

                    //force.Uncertainty = force.Uncertainty + uncertaintyvalue;
                    force.UncertaintyASTM = UncertaintyPre + uncertaintyvalue;
                    force.BasicCalibrationResult.Uncertanty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(force.Uncertainty)));

                    forceResult1.Add(force);

                }

                //Caculate Compression
                sqrtRep = 0;
                for (int i = 0; i < Compresion.Count(); i++)
                {
                    double stddevtemp = 0;
                    double stddev = 0;
                    double mean = 0;

                    CalibrationResultContributor resultContributor = new CalibrationResultContributor();
                    double nominal = Compresion[i].BasicCalibrationResult.Nominal;
                    var force = Compresion[i];
                    var UncertaintyPre = force.Uncertainty;
                    
                    var passfailASTM = "Pass";
                    if (Math.Abs(repeatabilityASTM) > 1)
                        passfailASTM = "Fail";
                    else
                        passfailASTM = Compresion[i].BasicCalibrationResult.InToleranceLeftASTM;

                    
                    //////
                    if (i == 0 || i == 1)
                        {
                            sqrtRep = 0;
                            for (int j = 1; j < 6; j++)
                            {
                                resultContributor = new CalibrationResultContributor();
                            sqrtRep = sqrtRep + Math.Pow((Compresion[j].BasicCalibrationResult.Repeatability), 2);
                            resultContributor.Description = "Repeatability ";


                        }
                        }
                        else if (i == Compresion.Count() - 1 || i == Compresion.Count() || i == Compresion.Count() - 2)
                        {
                            sqrtRep = 0;
                            for (int j = Compresion.Count() - 6; j < Compresion.Count()-1; j++)
                            {
                                resultContributor = new CalibrationResultContributor();
                            sqrtRep = sqrtRep + Math.Pow((Compresion[j].BasicCalibrationResult.Repeatability ), 2);
                            resultContributor.Description = "Repeatability ";
                        }

                        }
                        else
                        {

                            sqrtRep = 0;
                            for (int j = i - 2; j <= i + 2; j++)
                            {
                                resultContributor = new CalibrationResultContributor();
                            sqrtRep = sqrtRep + Math.Pow((Compresion[j].BasicCalibrationResult.Repeatability), 2);
                            resultContributor.Description = "Repeatability ";
                        }

                        }

                    
                    force.CalibrationResultContributors.Add(resultContributor);
                    //Resolution Contributor Compression
                    if (i + 1 == force.BasicCalibrationResult.Position)
                    {
                        resultContributor = new CalibrationResultContributor();
                        var res = force.BasicCalibrationResult.Resolution / 3.46;
                        sqrtRes = Math.Pow(res, 2);
                        resultContributor.Square = sqrtRes;
                        resultContributor.Description = "Resolution";
                        resultContributor.TypeContributor = "Resolution";
                        resultContributor.Distribution = "Resolution";
                        resultContributor.Magnitude = force.BasicCalibrationResult.Resolution;
                        force.CalibrationResultContributors.Add(resultContributor);
                    }

                    //Resolution at 0 Contributor Compression

                    resultContributor = new CalibrationResultContributor();
                    var res0 = Compresion[0].BasicCalibrationResult.Resolution / 3.46;
                    sqrtRes = Math.Pow(res0, 2);
                    resultContributor.Square = sqrtRes;
                    resultContributor.Description = "Resolution at 0 ";
                    resultContributor.TypeContributor = "Resolution at 0";
                    resultContributor.Magnitude = Compresion[0].BasicCalibrationResult.Resolution;
                    resultContributor.Distribution = "Resolution";
                    force.CalibrationResultContributors.Add(resultContributor);
                    // 


                    repeteabilityUncert = Math.Sqrt(sqrtRep / 10);
                    cont = 0;

                    //************ IV Repeatability & Reproducbility
                    //1. Uncertainty (%) Magnitude
                    double repetUncertMagnitude = repeteabilityUncert;
                    //if (!double.IsInfinity(repeteabilityUncert) && !double.IsInfinity(nominal) && nominal != 0)
                    //{
                    //    repetUncertMagnitude = (repeteabilityUncert / nominal) * 100;
                    //}
                    double divisor = 1;
                    double quotient = 0;
                    double squared = 0;

                    //2. Quotient

                    quotient = repetUncertMagnitude / divisor;

                    //3. squared
                    squared = Math.Pow(quotient, 2);

                    double sqroot = 0;
                    double expandedUncertainty = 0;
                    cont = 0;


                    sqroot = squared;

                    //2.Expanded Uncertainty
                    expandedUncertainty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(sqroot * 2)));


                    foreach (var item1 in force.CalibrationResultContributors.Where(x => x.TypeContributor == null))
                    {
                        item1.Quotient = quotient;
                        item1.Divisor = 1;
                        //  item1.Square = squared;
                        item1.Magnitude = repetUncertMagnitude;
                        item1.TypeContributor = "Repeatability";
                       // item1.Description = "Repeatability";
                        item1.Distribution = "Normal";
                        item1.Square = repetUncertMagnitude * repetUncertMagnitude;// squared;
                    }

                    //************ VII FINAL PERCENT UNCERTAINTY OF VAALUES 
                    double uncertaintyvalue = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(expandedUncertainty)));
                    if (Double.IsNaN(UncertaintyPre))
                        force.UncertaintyASTM = 0;

                    //force.Uncertainty = force.Uncertainty + uncertaintyvalue;
                    force.UncertaintyASTM = UncertaintyPre + uncertaintyvalue;
                    force.BasicCalibrationResult.Uncertanty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(force.Uncertainty)));

                    forceResult1.Add(force);

                }


                //Universal ******
                for (int i = 0; i < Universal.Count(); i++)
                {
                    CalibrationResultContributor resultContributor = new CalibrationResultContributor();
                    double nominal = Universal[i].BasicCalibrationResult.Nominal;
                    var force = Universal[i];
                    double stddevtemp = 0;
                    double stddev = 0;
                    double mean = 0;
                    var UncertaintyPre = force.Uncertainty;

                    
                    var passfailASTM = "Pass";
                    if (Math.Abs(repeatabilityASTM) > 1)
                        passfailASTM = "Fail";
                    else
                        passfailASTM = Universal[i].BasicCalibrationResult.InToleranceLeftASTM;

                    
                    //////

                    if (i == 0 || i == 1)
                        {
                            sqrtRep = 0;
                            for (int j = 1; j < 6; j++)
                            {
                                resultContributor = new CalibrationResultContributor();
                                sqrtRep = sqrtRep + Math.Pow((Universal[j].BasicCalibrationResult.Repeatability), 2);
                            resultContributor.Description = "Repeatability ";


                        }
                        }
                        else if (i == Universal.Count() - 1 || i == Universal.Count() || i == Universal.Count() - 2)
                        {
                            sqrtRep = 0;
                            for (int j = Universal.Count() - 6; j < Universal.Count()-1; j++)
                            {
                                resultContributor = new CalibrationResultContributor();
                            sqrtRep = sqrtRep + Math.Pow((Universal[j].BasicCalibrationResult.Repeatability), 2);
                            resultContributor.Description = "Repeatability ";
                        }

                        }
                        else
                        {
                            sqrtRep = 0;
                            for (int j = i - 2; j <= i + 2; j++)
                            {
                                resultContributor = new CalibrationResultContributor();
                            sqrtRep = sqrtRep + Math.Pow((Universal[j].BasicCalibrationResult.Repeatability), 2);
                            resultContributor.Description = "Repeatability ";
                        }

                        }
                    force.CalibrationResultContributors.Add(resultContributor);
               
                    //Resolution Contributor Universal
                    if (i + 1 == force.BasicCalibrationResult.Position)
                    {
                        resultContributor = new CalibrationResultContributor();
                        var res = force.BasicCalibrationResult.Resolution / 3.46;
                        sqrtRes = Math.Pow(res, 2);
                        resultContributor.Square = sqrtRes;
                        resultContributor.Description = "Resolution";
                        resultContributor.TypeContributor = "Resolution";
                        resultContributor.Distribution = "Resolution";
                        resultContributor.Magnitude = force.BasicCalibrationResult.Resolution;
                        force.CalibrationResultContributors.Add(resultContributor);
                    }

                    //Resolution at 0 Contributor Universal

                    resultContributor = new CalibrationResultContributor();
                    var res0 = Universal[0].BasicCalibrationResult.Resolution / 3.46;
                    sqrtRes = Math.Pow(res0, 2);
                    resultContributor.Square = sqrtRes;
                    resultContributor.Description = "Resolution at 0 ";
                    resultContributor.TypeContributor = "Resolution at 0";
                    resultContributor.Distribution = "Resolution";
                    resultContributor.Magnitude = Universal[0].BasicCalibrationResult.Resolution;
                    force.CalibrationResultContributors.Add(resultContributor);
                    // 


                    repeteabilityUncert = Math.Sqrt(sqrtRep / 10);
                    cont = 0;

                    //************ IV Repeatability & Reproducbility
                    //1. Uncertainty (%) Magnitude
                    double repetUncertMagnitude = repeteabilityUncert;
                    //if (!double.IsInfinity(repeteabilityUncert) && !double.IsInfinity(nominal) && nominal != 0)
                    //    repetUncertMagnitude = (repeteabilityUncert / nominal) * 100;
                    double divisor = 1;
                    double quotient = 0;
                    double squared = 0;

                    //2. Quotient
                    if (divisor != 0)
                    quotient = repetUncertMagnitude / divisor;

                    //3. squared
                    squared = Math.Pow(quotient, 2);

                    //************ V Squared Attributes
                    //1. Standard Load Cell Non Linearity
                    //Note: This section is the UncertaintyValue of  WeightSets Object, Is don't calculate


                    //************ VI Uncertainty Calculations
                    //1.Total Uncertainty (sq. root of sum of sq)
                    double sqroot = 0;
                    double expandedUncertainty = 0;
                    cont = 0;


                    sqroot = squared; //here is sum of AR21:AZ21 but this fields are not calculated 

                    //2.Expanded Uncertainty
                    expandedUncertainty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(sqroot * 2)));

                    //************* Add Contributors

                    foreach (var item1 in force.CalibrationResultContributors.Where(x => x.TypeContributor == null))
                    {
                        item1.Quotient = quotient;
                        item1.Divisor = 1;
                        //    item1.Square = squared;
                        item1.Magnitude = repetUncertMagnitude;
                        item1.TypeContributor = "Repeatability";
                        //item1.Description = "Repeatability";
                        item1.Distribution = "Normal";
                        item1.Square = repetUncertMagnitude * repetUncertMagnitude;// squared;
                    }
                    //force.CalibrationResultContributors.Add(resultContributor);

                    //************ VII FINAL PERCENT UNCERTAINTY OF VAALUES 
                    double uncertaintyvalue = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(expandedUncertainty)));
                    
                    if (Double.IsNaN(UncertaintyPre))
                        force.UncertaintyASTM = 0;

                    //force.Uncertainty = force.Uncertainty + uncertaintyvalue;
                    force.UncertaintyASTM = UncertaintyPre + uncertaintyvalue;
                    force.BasicCalibrationResult.Uncertanty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(force.Uncertainty)));

                    forceResult1.Add(force);

                }

            }
            catch (Exception ex)
            {

            }

            foreach (var item in forceResult1)
            {
                double xpandedUncert = 0;
                double sumSquares = 0;

                foreach (var _item in item.CalibrationResultContributors)
                {
                    if (_item.Distribution == "Resolution")
                    {
                        double divisor = 3.46;
                        _item.Divisor = divisor;
                        double quotient = (_item.Magnitude / divisor);
                        _item.Quotient = quotient;

                        double square = Math.Pow(quotient, 2);
                        _item.Square = square;

                    }
                    sumSquares = (sumSquares + _item.Square);
                    var totalUncert = Math.Sqrt(sumSquares);
                    xpandedUncert = Math.Round(totalUncert * 2, 5);
                    

                }

                item.BasicCalibrationResult.Uncertanty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(xpandedUncert))); ;
                //item.Uncertainty = xpandedUncert;
                item.UncertaintyASTM = xpandedUncert;

                ///TUR
                ///
                if (tolerance != null)
                {
                    var lowTolerance = ValidTolerance(item.BasicCalibrationResult.Nominal, (int)tolerance.ToleranceTypeID, tolerance.Resolution, tolerance.AccuracyPercentage, "low");
                    var maxTolerance = ValidTolerance(item.BasicCalibrationResult.Nominal, (int)tolerance.ToleranceTypeID, tolerance.Resolution, tolerance.AccuracyPercentage, "max");
                    var toleranceDifference = maxTolerance - lowTolerance;

                    var uncertx2 = (xpandedUncert * 2);
                    if (uncertx2 == 0)
                    {
                        uncertx2 = 1;
                    }

                    var tur = (toleranceDifference / uncertx2);
                    item.BasicCalibrationResult.TURAstm = tur;
                }
            }

            return forceResult1;
        }
        public async Task<ResultSet<PieceOfEquipment>> GetPOEByTestCodePag(Pagination<PieceOfEquipment> pagination)
        {
             return await pieceOfEquipmentRepository.GetPOEByTestCodePag(pagination);
        }

        public async Task<ResultSet<POE_Scale>> GetPOEScale(Pagination<POE_Scale> pagination)
        {
            return await pieceOfEquipmentRepository.GetPOEScale(pagination);
        }

        public async Task<IEnumerable<PieceOfEquipment>> GetPieceOfEquipmentByScale(string id)
        {

            return await pieceOfEquipmentRepository.GetPieceOfEquipmentByScale(id);

        }

        public async Task<IEnumerable<PieceOfEquipment>> GetResolutionByMass(IEnumerable<PieceOfEquipment> DTO)
        {

            return await pieceOfEquipmentRepository.GetResolutionByMass(DTO);

        }


        public async Task<IEnumerable<PieceOfEquipment>> GetResolutionByLenght(IEnumerable<PieceOfEquipment> DTO)
        {
            var a= await pieceOfEquipmentRepository.GetResolutionByLenght(DTO);
            return a;

        }

        public async Task<CalibrationType> CreateConfiguration(CalibrationType DTO)
        {
            return await pieceOfEquipmentRepository.CreateConfiguration(DTO);
        }


        public async Task<CalibrationSubType> DeleteConfiguration(CalibrationSubType DTO)
        {
            return await pieceOfEquipmentRepository.DeleteConfiguration(DTO);
        }

        public async Task<CalibrationSubType> EnableConfiguration(CalibrationSubType DTO)
        {
            return await pieceOfEquipmentRepository.EnableConfiguration(DTO);
        }


        public async Task<CalibrationType> GetDynamicConfiguration(int CalibrationTypeID, string Component="")
        {
            return await pieceOfEquipmentRepository.GetDynamicConfiguration(CalibrationTypeID, Component);
        }

        public async Task<UncertaintyViewModel> GetReportUncertaintyBudget(Linearity li, int seq, WorkOrderDetail wodById, WorkOrderDetail workOrderDetail_, List<PieceOfEquipment>? poes_, List<WeightSet>? weightSets_)
        {


            WorkOrderDetail wo = new WorkOrderDetail();
            wo.WorkOrderDetailID = li.WorkOrderDetailId;
            WorkOrderDetail workOrderDetail1 = new WorkOrderDetail();
            IEnumerable<PieceOfEquipment>  poes = new List<PieceOfEquipment>();
            WorkOrderDetail workOrderDetail = new WorkOrderDetail();
            WorkOrderDetail wodWihtWeights = await wodRepository.GetWorkOrderDetailByID(wodById.WorkOrderDetailID);
            workOrderDetail = wodById;
            
            if(workOrderDetail_ != null && workOrderDetail_.BalanceAndScaleCalibration != null)
            {
                workOrderDetail.BalanceAndScaleCalibration = workOrderDetail_.BalanceAndScaleCalibration;
            }
            
            if (workOrderDetail != null && workOrderDetail.BalanceAndScaleCalibration == null)
            {
                workOrderDetail = workOrderDetail_;
                workOrderDetail1 = await wodRepository.GetWorkOrderDetailByID(wo.WorkOrderDetailID);
                workOrderDetail.BalanceAndScaleCalibration = workOrderDetail1.BalanceAndScaleCalibration;
            }

            var poe = wodById.PieceOfEquipment;

            //if (poes_ == null)
            //    poes = await pieceOfEquipmentRepository.GetAllWeightSets(poe);
            //else
            //    poes = poes_;
            
            foreach (var ws in workOrderDetail.WOD_Weights)
            {
                if(ws.WeightSet== null)
                {
                    var w = await pieceOfEquipmentRepository.GetWeigthSetById(ws.WeightSetID);
                    ws.WeightSet = w;
                }
            }

            var UoMs = await uomRepository.GetAll();

            if(li !=null)
            {
                CalibrationType calibrationType = new CalibrationType()
                {
                    CalibrationTypeId = workOrderDetail.BalanceAndScaleCalibration.CalibrationTypeId
                };

                calibrationType = await Basics.GetCalibrationTypeByID(calibrationType);
                
                var result = await Querys.WOD.GetReportUncertaintyBudget(li, seq, workOrderDetail, workOrderDetail, null,  UoMs);
                var lin = workOrderDetail.BalanceAndScaleCalibration.Linearities.Where(x => x.SequenceID == seq).FirstOrDefault();
                if (calibrationType != null && calibrationType.CMCValues != null && calibrationType.CMCValues.Count() > 0)
                {
                    var nominalValue = lin.TestPoint.NominalTestPoit;
                    
                    foreach (var item in calibrationType.CMCValues)
                    {
                        bool replace = false;
                        if (item.IncludeEqualsMin && item.IncludeEqualsMax && nominalValue >= item.MinRange && nominalValue <= item.MaxRange)
                        {
                            replace = true;
                        }
                        else if (item.IncludeEqualsMin && !item.IncludeEqualsMax && nominalValue >= item.MinRange && nominalValue < item.MaxRange)
                        {
                            replace = true;
                        }
                        else if (!item.IncludeEqualsMin && item.IncludeEqualsMax && nominalValue > item.MinRange && nominalValue <= item.MaxRange)
                        {
                            replace = true;
                        }
                        else if (!item.IncludeEqualsMin && !item.IncludeEqualsMax && nominalValue > item.MinRange && nominalValue < item.MaxRange)
                        {
                            replace = true;
                        }

                        if (replace && lin.TotalUncertainty < item.CMC && result.UncertaintyOfRerences?.Count > 0)
                        {
                            var uom = result.UncertaintyOfRerences?.FirstOrDefault().Units;
                            lin.BasicCalibrationResult.UncertaintyNew = item.CMC;
                            
                            result.cmcValuesReplace = "The original value for this uncertainty was: " +
                                    result.Totales.TotalUncerainty + "-" + uom +
                                    " but it was replaced by the CMC value of " + lin.BasicCalibrationResult.UncertaintyNew +
                                    " according to the range limit of " + item.MinRange + " " + uom + "-" + item.MaxRange + " " + uom;
                            result.Totales.ExpandedUncertainty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(item.CMC))); 
                            break;
                        }
                    }

                }

               

                    return result;

            }
            else
            {
                return new UncertaintyViewModel();
            }



        }



       

        public double ValidTolerance(double Nominal, int ToleranceTypeID, double Resolution, double AccuracyPercentage, string lownmax)
        {
            //if (run1 == -1)
            //{ 
            //    var message = "In";
            //}

            double nominal = Nominal;
       
            int toleranceType = ToleranceTypeID;
            double toleranceLow = 0;
            double toleranceMax = 0;
            

            if (toleranceType == 1)
            {
                toleranceLow = ResolutionTolerance(Resolution, 1, nominal);
                toleranceMax = ResolutionTolerance(Resolution, 2, nominal);
            }
            else if (toleranceType == 2)
            {
                toleranceLow = PercentageResolutionTolerance(AccuracyPercentage, Resolution, 1, nominal);
                toleranceMax = PercentageResolutionTolerance(AccuracyPercentage, Resolution, 2, nominal);
            }
            else if (toleranceType == 4)
            {
                toleranceLow = Percentage(AccuracyPercentage, 1, nominal);
                toleranceMax = Percentage(AccuracyPercentage, 2, nominal);
            }

            if (lownmax == "low")
            {
                return toleranceLow;
            }
            else
            {
                return toleranceMax;
            }


        }

        private double Percentage(double tolerance, int ToleranceRange, double testpoint)
        {
            double Accuracy = tolerance;
            double toleranceLow = 0;

            double TestPoint = testpoint;
            if (ToleranceRange == 1)
                toleranceLow = TestPoint - (TestPoint * Accuracy) / 100;
            else
                toleranceLow = TestPoint + (TestPoint * Accuracy) / 100;




            return toleranceLow;
        }
        private double PercentageResolutionTolerance(double tolerance, double resol, int ToleranceRange, double testpoint)
        {
            double toleranceLow;
            double Accuracy = tolerance;
            double resolution = resol;
            double TestPoint;


            TestPoint = testpoint;


            if (ToleranceRange == 1)
                toleranceLow = TestPoint - (((TestPoint * Accuracy) / 100) + resolution);
            else
                toleranceLow = TestPoint + (((TestPoint * Accuracy) / 100) + resolution);

            return toleranceLow;
        }
        //Resolution
        private double ResolutionTolerance(double tolerance, int ToleranceRange, double testpoint)
        {
            double resolution = tolerance;
            double toleranceLow;


            double TestPoint = testpoint;
            if (ToleranceRange == 1)
                toleranceLow = TestPoint - resolution;
            else
                toleranceLow = TestPoint + resolution;
            return toleranceLow;
        }
        public async Task<IEnumerable<PieceOfEquipment>> GetPieceOfEquipmentChildrenAll(PieceOfEquipment poe)
        {

            var a = await pieceOfEquipmentRepository.GetPieceOfEquipmentChildrenAll(poe);

            return a;
        }

        public async Task<PieceOfEquipment> UpdateChildPieceOfEquipment(PieceOfEquipment pieceOfEquipmentDTO)
        {
            return await pieceOfEquipmentRepository.UpdateChildPieceOfEquipment(pieceOfEquipmentDTO);
        }


        public async Task<ResultSet<PieceOfEquipment>> GetSelectPOEChildren(Pagination<PieceOfEquipment> pagination)
        {


            return await pieceOfEquipmentRepository.GetSelectPOEChildren(pagination);


        }


    }
}
