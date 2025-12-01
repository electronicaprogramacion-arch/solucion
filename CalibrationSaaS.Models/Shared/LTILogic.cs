using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Views;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Globalization;

using System.Threading.Tasks;
using Bogus;
using System.Dynamic;
using static CalibrationSaaS.Domain.Aggregates.Querys.Querys;

namespace CalibrationSaaS.Domain.Aggregates.Shared
{
    public partial class LTILogic
    {
        

        public static bool ValidateTemperature(Double Temperature)
        {


            if (Temperature >= 10 && Temperature <= 35)
            {

                return true;
            }
            else
            {
                return false;
            }

        }

        public static bool ValidateTemperatureDifference(Double TempInit, Double TempEnd)
        {


            if (Math.Abs(TempEnd - TempInit) <= 2)
            {

                return true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// Validate that the run is at least 200 times larger than the resolution of the machine
        /// </summary>
        /// <param name="TempInit"></param>
        /// <param name="TempEnd"></param>
        /// <returns></returns>
        public static bool ValidateRunValue(Double RunValue, Double Capacity)
        {
            if (RunValue * Capacity >= 200)
            {

                return true;
            }
            else
            {
                return false;
            }

        }



        public static ICollection<Force> GetCalculatesForISOandASTM(List<Force> forces, int? iso)
        {
            try
            {
                if (iso != null)
                {
                    List<Force> forceResult = new List<Force>();
                    foreach (var item in forces)
                    {
                        double classRelativeIndicationError = 0;
                        double classRepeatabilityIndicationError = 0;
                        var force = item;
                        var error1 = Math.Round(force.BasicCalibrationResult.Nominal - force.BasicCalibrationResult.RUN1, 3);
                        var error2 = Math.Round(force.BasicCalibrationResult.Nominal - force.BasicCalibrationResult.RUN2, 3);
                        var error3 = Math.Round(force.BasicCalibrationResult.Nominal - force.BasicCalibrationResult.RUN3, 3);
                        var error4 = Math.Round(force.BasicCalibrationResult.Nominal - force.BasicCalibrationResult.RUN4, 3);

                        var error1_p = Math.Round((error1 / force.BasicCalibrationResult.Nominal) * 100, 3);

                        if (double.IsNaN(Math.Round(error1_p, 3)))
                        {
                            force.BasicCalibrationResult.ErrorpRun1 = 0;
                        }
                        else
                        {
                            force.BasicCalibrationResult.ErrorpRun1 = Math.Round(error1_p, 3);
                        }

                        if (double.IsNaN(Math.Round(error1, 3)))
                        {
                            force.BasicCalibrationResult.ErrorRun1 = 0;
                        }
                        else
                        {
                            force.BasicCalibrationResult.ErrorRun1 = Math.Round(error1, 3);
                        }



                        var error2_p = Math.Round((error2 / force.BasicCalibrationResult.Nominal) * 100, 3);

                        var error3_p = Math.Round((error3 / force.BasicCalibrationResult.Nominal) * 100, 3);
                        var error4_p = Math.Round((error4 / force.BasicCalibrationResult.Nominal) * 100, 3);

                        //if (force.ISO == true)
                        //{
                        force.BasicCalibrationResult.RelativeIndicationErrorR1 = error1;
                        force.BasicCalibrationResult.RelativeIndicationErrorR2 = error2;
                        force.BasicCalibrationResult.RelativeIndicationErrorR3 = error3;
                        force.BasicCalibrationResult.RelativeIndicationErrorR4 = error4;

                        //Relative Indication Error
                        double relativeIndicationErr = 0;

                        if (force.BasicCalibrationResult.RUN2 != null && force.BasicCalibrationResult.RUN2 != 0)
                        {
                            relativeIndicationErr = (Math.Abs(error2) + Math.Abs(error3) + Math.Abs(error4)) / 3;
                            if (Math.Abs(error2) >= Math.Abs(error3) && Math.Abs(error2) >= Math.Abs(error4))
                                force.BasicCalibrationResult.MaxErrorNominal = error2;
                            else if (Math.Abs(error3) >= Math.Abs(error2) && Math.Abs(error3) >= Math.Abs(error4))
                                force.BasicCalibrationResult.MaxErrorNominal = error3;
                            else
                                force.BasicCalibrationResult.MaxErrorNominal = error4;
                        }
                        else
                        {
                            relativeIndicationErr = (Math.Abs(error1) + Math.Abs(error3) + Math.Abs(error4)) / 3;
                            if (Math.Abs(error1) >= Math.Abs(error3) && Math.Abs(error1) >= Math.Abs(error4))
                                force.BasicCalibrationResult.MaxErrorNominal = error1;
                            else if (Math.Abs(error3) >= Math.Abs(error1) && Math.Abs(error3) >= Math.Abs(error4))
                                force.BasicCalibrationResult.MaxErrorNominal = error3;
                            else
                                force.BasicCalibrationResult.MaxErrorNominal = error4;
                        }


                        if (double.IsNaN(Math.Round(relativeIndicationErr, 3)))
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
                            double minReading = 0;
                            //var ErrorPercent
                            double[] _maxminReading1 = new double[3] { error1_p, error3_p, error4_p };
                            double[] _maxminReading2 = new double[3] { error2_p, error3_p, error4_p };

                            if (force.BasicCalibrationResult.RUN2 != 0)
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

                            var relativeRepError = maxReading - minReading;

                            if (double.IsNaN(Math.Round(relativeRepError, 3)))
                            {
                                force.BasicCalibrationResult.RelativeRepeatabilityError = 0;
                            }
                            else
                            {
                                force.BasicCalibrationResult.RelativeRepeatabilityError = Math.Round(relativeRepError, 3);
                            }



                            //Calculate Class
                            //1 Calculate Class Indication

                            var relativeIndicationErr_ = Math.Abs(relativeIndicationErr);
                            if (relativeIndicationErr_ >= 0 && relativeIndicationErr_ <= 0.5)
                                classRelativeIndicationError = 0.5;
                            else if (relativeIndicationErr_ > 0.5 && relativeIndicationErr_ <= 1)
                                classRelativeIndicationError = 1;
                            else if (relativeIndicationErr_ > 1 && relativeIndicationErr_ <= 2)
                                classRelativeIndicationError = 2;
                            else if (relativeIndicationErr_ > 2 && relativeIndicationErr_ <= 3)
                                classRelativeIndicationError = 3;
                            else
                                classRelativeIndicationError = 0;



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
                                classRepeatabilityIndicationError = 0;




                        }

                        double relativeResolution = force.BasicCalibrationResult.RelativeResolution;
                        //Class Run1 
                        //1 Calculate Class Indication Run1
                        double classRelativeIndicationErrorRun1 = 0;
                        var relativeIndicationErrRun1_ = Math.Abs(error1);
                        if (relativeIndicationErrRun1_ >= 0 && relativeIndicationErrRun1_ <= 0.5)
                            classRelativeIndicationErrorRun1 = 0.5;
                        else if (relativeIndicationErrRun1_ > 0.5 && relativeIndicationErrRun1_ <= 1)
                            classRelativeIndicationErrorRun1 = 1;
                        else if (relativeIndicationErrRun1_ > 1 && relativeIndicationErrRun1_ <= 2)
                            classRelativeIndicationErrorRun1 = 2;
                        else if (relativeIndicationErrRun1_ > 2 && relativeIndicationErrRun1_ <= 3)
                            classRelativeIndicationErrorRun1 = 3;
                        else
                            classRelativeIndicationErrorRun1 = 0;
                        ////
                        ///ASTM
                        double repeatability;
                        double maxError = 0;


                        //if (iso == 3 )                                                             
                        //{

                        double maxReadingASTM = 0;
                        double minReadingASTM = 0;
                        //var ErrorPercent
                        double[] _maxminReading1ASTM = new double[2] { error1_p, error3_p };
                        double[] _maxminReading2ASTM = new double[2] { error2_p, error3_p };

                        if (force.BasicCalibrationResult.RUN2 != 0)
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

                        var relativeRepErrorASTM = maxReadingASTM - minReadingASTM;

                        if (double.IsNaN(Math.Round(relativeRepErrorASTM, 3)))
                        {
                            force.BasicCalibrationResult.RelativeRepeatabilityError = 0;
                        }
                        else
                        {
                            force.BasicCalibrationResult.RelativeRepeatabilityError = Math.Round(relativeRepErrorASTM, 3);
                        }
                        ///

                        if (force.BasicCalibrationResult.RUN2 != 0)
                        {
                            repeatability = error2_p - error1_p;
                            if (Math.Abs(error1_p) >= Math.Abs(error2_p))
                                maxError = error1_p;
                            else
                                maxError = error2_p;
                        }
                        else
                        {

                            repeatability = error3_p - error1_p;
                            if (Math.Abs(error1_p) >= Math.Abs(error3_p))
                                maxError = error1_p;
                            else
                                maxError = error3_p;
                        }


                        if (double.IsNaN(Math.Round(repeatability, 3)))
                        {
                            force.BasicCalibrationResult.RelativeRepeatabilityError = 0;
                        }
                        else
                        {
                            force.BasicCalibrationResult.RelativeRepeatabilityError = Math.Round(repeatability, 3);
                        }


                        if (double.IsNaN(Math.Round(maxError, 3)))
                        {
                            force.BasicCalibrationResult.MaxErrorp = 0;
                        }
                        else
                        {
                            force.BasicCalibrationResult.MaxErrorp = Math.Round(maxError, 3);
                        }

                        //}

                        //3. Calculate class Zero


                        //Get Zero
                        double zero = 0;
                        double classZero = 0;

                        if (force.BasicCalibrationResult.IsZeroReturn)
                        {
                            zero = force.BasicCalibrationResult.RUN1;


                            var zero_ = Math.Abs(zero);
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
                        }
                        //calculate maximum pemisible value
                        double classReg = 0;



                        double[] maxClass = new double[2] { classRelativeIndicationError, classRepeatabilityIndicationError };

                        for (int i = 0; i < maxClass.Count(); i++)
                        {

                            if (Math.Abs(maxClass[i]) > Math.Abs(classReg))
                            {
                                classReg = Math.Abs(maxClass[i]);
                            }

                        }

                        if (force.BasicCalibrationResult.IsZeroReturn)
                            force.BasicCalibrationResult.Class = classZero.ToString();
                        else
                            if (force?.WeightSets?.Count() > 0 && force?.WeightSets != null)
                        {
                            if ((double)force.WeightSets.FirstOrDefault().Class < classReg && force.WeightSets.FirstOrDefault().Class != 0)

                                force.BasicCalibrationResult.Class = force.WeightSets.FirstOrDefault().Class.ToString();
                            else
                                force.BasicCalibrationResult.Class = classReg.ToString();
                        }
                        else
                        {
                            force.BasicCalibrationResult.Class = classReg.ToString();
                        }

                        force.BasicCalibrationResult.ClassRun1 = classRelativeIndicationErrorRun1.ToString();
                        if (Math.Abs(maxError) >= 1)
                            force.BasicCalibrationResult.InToleranceLeft = "Fail";
                        else
                            force.BasicCalibrationResult.InToleranceLeft = "Pass";

                        if (Math.Abs(error1_p) > 1)
                            force.BasicCalibrationResult.InToleranceFound = "Fail";
                        else
                            force.BasicCalibrationResult.InToleranceFound = "Pass";
                        forceResult.Add(force);

                        if (!force.ISO)
                        {
                            if (force?.WeightSets?.Count() > 0)
                                forceResult = CalculateUncert(forceResult).ToList();
                        }


                    }

                    return forceResult;
                }
                return forces;
            }
            catch (Exception ex)
            {
//                Console.WriteLine("GetCalculatesForISOandASTM Error ", ex.ToString());
                return forces;
            }


        }

        public static List<Force> GetCalculatesRanges(double capacity)
        {
            // Calculate Ranges from 20% to 100 % 
            List<Force> forceResults = new List<Force>();
            double[] ranges = new double[11] { 0, 0.5, 1, 2, 5, 10, 20, 40, 60, 80, 100 };//new double[11] {0, 0.4, 1.4, 4, 8, 14, 20, 40, 60, 80, 100 };

            for (int i = 0; i <= 11; i++)
            {
                Force forceResult = new Force();
                forceResult.BasicCalibrationResult.Nominal = Math.Round(capacity * ranges[i]);
                forceResults.Add(forceResult);
            }

            return forceResults;
        }

        public static double GetFS(int index)
        {
            double[] ranges = new double[13] { 0, 0, 0.5, 1, 2, 5, 10, 20, 40, 60, 80, 100, 0 };//new double[13] {0, 0, 0.4, 1.4, 4, 8, 14, 20, 40, 60, 80, 100, 0 };

            return ranges[index];
        }

        public static double GetFSNew(int index)
        {
            double[] ranges = new double[13] { 0, 0, 1, 6, 8, 10, 14, 20, 40, 60, 80, 100, 0};//new double[13] {0, 0, 0.4, 1.4, 4, 8, 14, 20, 40, 60, 80, 100, 0 };

            return ranges[index];
        }
        public static List<Domain.Aggregates.Entities.Rockwell> CreateRockwellList(WorkOrderDetail eq, int Compresion)
        {

            return new List<Rockwell>();

        }


        public static List<Domain.Aggregates.Entities.Rockwell> GetRockwellList(WorkOrderDetail eq, int Compresion)
        {

            return new List<Rockwell>();

        }

        public static bool ValidateRockwellList(WorkOrderDetail eq, int IsCompresion)
        {
            if (!eq.CalibrationTypeID.HasValue)
            {
                return false;       
            }


            var cs = GetCalibrationSubType(eq, IsCompresion);

            if (eq != null && (eq?.BalanceAndScaleCalibration?.Rockwells?.Where(x => x.CalibrationSubTypeId == cs).Count() > 0))
            {

                return true;
            }
            else
            {
                return false;
            }


            return true;

        }


        public static bool ValidateGenericlList(WorkOrderDetail eq, int IsCompresion)
        {
            if (!eq.CalibrationTypeID.HasValue)
            {
                return false;
            }


            var cs = GetCalibrationSubType(eq, IsCompresion);

            if (eq != null && (eq?.BalanceAndScaleCalibration?.GenericCalibration?.Where(x => x.CalibrationSubTypeId == cs).Count() > 0))
            {

                return true;
            }
            else
            {
                return false;
            }


            return true;

        }


        public static List<Domain.Aggregates.Entities.Force> CreateForceList(WorkOrderDetail eq, int Compresion)
        {

            bool isiso = false;



            int subtipec = GetCalibrationSubType(eq, Compresion);

            if (subtipec == 4 || subtipec == 5 || subtipec == 8)
            {
                isiso = true;
            }


            List<Domain.Aggregates.Entities.Force> listlienarity = new List<Domain.Aggregates.Entities.Force>();

            //CompresionResult r = new CompresionResult();

            var ad = new { standard = 1, test = 2 };

            dynamic cvv = new ExpandoObject();

            cvv.test=0;

            //var f =  Faker.FromAnonymousType(cvv).RuleFor(x=>x.test,f=>1);



            //var f = Faker.FromAnonymousType(ad).RuleFor(x=>ad.st;


            Faker<ForceResult> fo = new Faker<ForceResult>();
            int cont = 1;


            fo.
                RuleFor(r => r.FS, f => CalibrationSaaS.Domain.Aggregates.Shared.LTILogic.GetFS(cont))
                .RuleFor(r => r.Nominal, f => Math.Round((CalibrationSaaS.Domain.Aggregates.Shared.LTILogic.GetFS(cont) / 100) * eq.PieceOfEquipment.Capacity))
              .RuleFor(r => r.Resolution, f => eq.PieceOfEquipment.Resolution)
              .RuleFor(r => r.Error, x => 0)
              .RuleFor(r => r.ErrorPer, 0)
              .RuleFor(r => r.DecimalNumber, (f, u) => eq.PieceOfEquipment.DecimalNumber)
               .RuleFor(r => r.CalibrationSubTypeId, f => subtipec)
               .RuleFor(r => r.WorkOrderDetailId, f => eq.WorkOrderDetailID)
               .RuleFor(r => r.IsZeroReturn, f => IsZero(CalibrationSaaS.Domain.Aggregates.Shared.LTILogic.GetFS(cont)))
               .RuleFor(r => r.Position, f => cont)
              .RuleFor(r => r.SequenceID, f => cont++);


            //var a = fo.Generate(10);


            int cont2 = 1;
            Faker<Force> t = new Faker<Force>();

            t.RuleFor(r => r.BasicCalibrationResult, f => fo.Generate())
                //.RuleFor(r => r.SequenceID, x => x.Random.Int())
                .RuleFor(r => r.UnitOfMeasureId, f => eq.PieceOfEquipment.UnitOfMeasureID)
                .RuleFor(r => r.CalibrationUncertaintyValueUnitOfMeasureId, f => eq.PieceOfEquipment.UnitOfMeasureID)
                .RuleFor(r => r.CalibrationSubTypeId, f => subtipec)
                .RuleFor(r => r.WorkOrderDetailId, f => eq.WorkOrderDetailID)                
                .RuleFor(r => r.SequenceID, f => cont2++)
                .RuleFor(r => r.ISO, f => isiso)
                ;

            var aa = t.Generate(12);

            int cont1 = 1;
            double resolution = 0;
            if (aa[1].BasicCalibrationResult.Resolution == null || aa[1].BasicCalibrationResult.Resolution == 0)
            {

                resolution = eq.Resolution;
            }
            else
            {
                resolution = aa[1].BasicCalibrationResult.Resolution;
            }
            if (aa[1].BasicCalibrationResult.FS != 0 && aa[1].BasicCalibrationResult.Nominal < resolution * 200)
            {
                int contFS = 0;
                foreach (var item in aa)
                {
                    item.BasicCalibrationResult.Resolution = eq.Resolution;

                    if (item.SequenceID == 2)
                    {

                        item.BasicCalibrationResult.Nominal = resolution * 200;
                        item.BasicCalibrationResult.FS = Math.Round((item.BasicCalibrationResult.Nominal * 100) / eq.PieceOfEquipment.Capacity, 3);

                    }
                    else
                    {
                        item.BasicCalibrationResult.Nominal = Math.Round((CalibrationSaaS.Domain.Aggregates.Shared.LTILogic.GetFSNew(cont1) / 100) * eq.PieceOfEquipment.Capacity);

                        item.BasicCalibrationResult.FS = Math.Round((item.BasicCalibrationResult.Nominal * 100) / eq.PieceOfEquipment.Capacity, 3); // CalibrationSaaS.Domain.Aggregates.Shared.LTILogic.GetFSNew(cont1);
                    }
                    item.BasicCalibrationResult.NominalTemp = item.BasicCalibrationResult.Nominal;
                    cont1++;
                }
            }
            


                
            //foreach (var item in aa) 
            //{
            //    item.BasicCalibrationResult.Resolution = eq.Resolution;
                
            //    if (item.BasicCalibrationResult.FS != 0 && item.BasicCalibrationResult.Nominal < item.BasicCalibrationResult.Resolution * 200 )
            //    {
            //        item.BasicCalibrationResult.Nominal = item.BasicCalibrationResult.Resolution * 200;
            //        item.BasicCalibrationResult.FS = Math.Round((item.BasicCalibrationResult.Nominal * 100) / eq.PieceOfEquipment.Capacity, 3);

            //    }
            //    item.BasicCalibrationResult.NominalTemp = item.BasicCalibrationResult.Nominal;
            //    cont1++;
            //}
//            Console.WriteLine("CreateForceListxxx");
//            Console.WriteLine(aa.Count);
            return aa;


        }


        public static List<T> CreateListGeneric<T>(WorkOrderDetail eq, int Compresion)
        {

            bool isiso = false;



            int subtipec = GetCalibrationSubType(eq, Compresion);

            if (subtipec == 4 || subtipec == 5 || subtipec == 8)
            {
                isiso = true;
            }


            List<T> listlienarity = new List<T>();

            //CompresionResult r = new CompresionResult();

            return listlienarity;

        }



        public static bool IsZero(double fs)
        {
            if (fs == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public decimal test()
        {
            return 0;
        }

        public static List<Domain.Aggregates.Entities.Force> GetForceList(WorkOrderDetail eq, int Compresion)
        {

            int subtipec = GetCalibrationSubType(eq, Compresion);

            //if(subtipec==4 || subtipec==5 || subtipec == 8)
            //{
            //    isiso = true;
            //}


            if (eq?.BalanceAndScaleCalibration?.Forces?.Count > 0)
            {

//                Console.WriteLine(subtipec);
                var aa = eq.BalanceAndScaleCalibration.Forces.Where(x => x.CalibrationSubTypeId > 0 && x.CalibrationSubTypeId == subtipec).ToList();

//                Console.WriteLine("GetForceListxxx");
//                Console.WriteLine(aa.Count);

                return aa;
            }
            else
            {
                return new List<Domain.Aggregates.Entities.Force>();
            }


        }


        public static List<T> GetForceListGeneric<T>(WorkOrderDetail eq, int Compresion)
        {

            int subtipec = GetCalibrationSubType(eq, Compresion);

            //if(subtipec==4 || subtipec==5 || subtipec == 8)
            //{
            //    isiso = true;
            //}


            if (eq?.BalanceAndScaleCalibration?.Rockwells?.Count > 0)
            {
                var aa = eq.BalanceAndScaleCalibration.Rockwells.Where(x => x.CalibrationSubTypeId == subtipec).ToList();

                return aa as List<T>;
            }
            else
            {
                return new List<T>();
            }


        }


        public static bool ValidateForceList(WorkOrderDetail eq, int IsCompresion, int iso)
        {

            double sumRunsComp = 0;
            List<Force> TotalForcesComp;


            var cs =  GetCalibrationSubType(eq, IsCompresion);
           
             if (eq != null && (eq?.BalanceAndScaleCalibration?.Forces?.Count(x => x.CalibrationSubTypeId == cs) > 0))
            {

                return true;
            }
            else
            {
                return false;
            }

        }


        public static int GetCalibrationSubType(WorkOrderDetail eq, int Compresion)
        {
            int temp = 3;
            int temp2 = 0;
            bool isiso = false;
            return Compresion;

            if (eq.CalibrationTypeID == 2)
            {
                if (eq.CertificationID.Value == 1)
                {
                    if (eq.IsUniversal)
                    {
                        temp = temp + eq.CertificationID.Value + 4 + Compresion;
                    }
                    else
                    {
                        temp = temp + eq.CertificationID.Value + Compresion;
                    }
                }
                if (eq.CertificationID.Value == 2)
                {
                    if (eq.IsUniversal)
                    {
                        temp = temp + eq.CertificationID.Value + 4 + Compresion;
                    }
                    else
                    {
                        temp = temp + eq.CertificationID.Value + Compresion + 1;
                    }
                }
                return temp;
            }

            if (eq.CalibrationTypeID >= 3)
            {
                    temp = Compresion;//temp + 6 + ((eq.CertificationID.Value) * (Compresion +1));

                    return temp;

            }

            return Compresion;


        }


        public static string GetCalibrationSubTypeStr(WorkOrderDetail eq, int Compresion)
        {
            var ss = GetCalibrationSubType(eq, Compresion);
            var result = "";
            switch (ss)
            {

                case 4:
                    result = "ISOTension";
                    break;
                case 5:
                    result = "ISOCompression";
                    break;
                case 6:
                    result = "ASTMTension";
                    break;
                case 7:
                    result = "ASTMCompression";
                    break;
                case 8:
                    result = "ISOUniversal";
                    break;
                case 9:
                    result = "ASTMUniversal";
                    break;
                     case 10:
                    result = "ASFoundISORockwell";
                    break;
                     case 11:                   

                    result = "ASLeftISORockwell";
                    break;
                case 12:
                    result = "ASFoundASTMRockwell";
                    break;
                case 13:

                    result = "ASLeftASTMRockwell";
                    break;
                case 14:
                    result = "MicroVickerAsLeft";
                    break;
                case 15:

                    result = "ASLeftASTMRockwell";
                    break;
                case 16:
                    result = "MicroVickerAsLeft";
                    break;
                case 17:

                    result = "ASLeftASTMRockwell";
                    break;

            }

            return result;

        }

        public static ICollection<Force> CalculateUncert(List<Force> forces)
        {
            List<Force> forceResult = new List<Force>();

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


                double Run1Delta = Math.Abs(0);  //todo
                double Run2Delta = Math.Abs(0);  //todo
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
                double Run1Delta_p = Math.Abs(0);
                double Run2Delta_p = Math.Abs(0);
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

                repeatability = deltaRepeatability / 100 * nominal;
                force.BasicCalibrationResult.Repeatability = repeatability;
                forceResult.Add(force);
            }

            //Calculate Repeteability Uncertainty
            //
            //

            int cont = 0;
            double repeteabilityUncert = 0;
            double sqrtRep = 0;
            List<Force> forceResult1 = new List<Force>();
            for (int i = 0; i < forceResult.Count(); i++)
            {
                double nominal = forceResult[i].BasicCalibrationResult.Nominal;
                var force = forceResult[i];
                if (i == 0 || i == 1)
                {
                    sqrtRep = 0;
                    for (int j = 0; j < 5; j++)
                    {
                        sqrtRep = sqrtRep + Math.Pow(forceResult[j].BasicCalibrationResult.Repeatability, 2);
                    }
                }
                else if (i == forceResult.Count() - 1 || i == forceResult.Count())
                {
                    sqrtRep = 0;
                    for (int j = forceResult.Count() - 5; j < forceResult.Count(); j++)
                    {
                        sqrtRep = sqrtRep + Math.Pow(forceResult[j].BasicCalibrationResult.Repeatability, 2);
                    }

                }
                else
                {

                    sqrtRep = 0;
                    for (int j = i - 2; j < i + 2; j++)
                    {
                        sqrtRep = sqrtRep + Math.Pow(forceResult[j].BasicCalibrationResult.Repeatability, 2);
                    }

                }

                repeteabilityUncert = Math.Sqrt(sqrtRep / 10);
                cont = 0;

                //************ IV Repeatability & Reproducbility
                //1. Uncertainty (%) Magnitude
                double repetUncertMagnitude = 0;
                repetUncertMagnitude = (repeteabilityUncert / nominal) * 100;
                double divisor = 1;
                double quotient = 0;
                double squared = 0;

                //2. Quotient

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


                sqroot = Math.Sqrt(force.WeightSets.ToList()[i].CalibrationUncertValue);


                //2.Expanded Uncertainty
                expandedUncertainty = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(sqroot * 2))); 


                //************ VII FINAL PERCENT UNCERTAINTY OF VAALUES 
                double uncertaintyvalue = ((double)RoundFirstSignificantDigit(Convert.ToDecimal(expandedUncertainty))); 

                force.Uncertainty = uncertaintyvalue;


                forceResult1.Add(force);

            }



            return forceResult1;
        }

        
        public static int[] GetLTICalibrationSubTypes()
        {
            int[] ranges = new int[8] { 4, 5, 6, 7, 8, 9,10,11 };

            return ranges;
        }
         public static int GetCalibrationSubType(WorkOrderDetail eq)
        {
            return 1;
        }

        public static bool ValidCalibrationSubType(int? Option1,WorkOrderDetail eq,params int[] pr)
        {
            bool result=false;

            if (!Option1.HasValue)
            {
                return false;
            }


            var Option = Option1.Value;

            if (pr.Count()==2 && eq.CalibrationTypeID==2)
            {
                if (!eq.IsUniversal && Option==1 && pr[0]==4 && pr[1]==5)
                {
                    result = true;
                }
                if (!eq.IsUniversal && Option==2 && pr[0]==6 && pr[1]==7)
                {
                    result = true;
                }
               
            
            
            }
            if (pr.Count() == 1 && eq.CalibrationTypeID == 2)
            { 
              if (eq.IsUniversal && Option==1 && pr[0]==8 )
                {
                    result = true;
                }
                if (eq.IsUniversal && Option==2 && pr[0]==9 )
                {
                    result = true;
                }
            }

            if (pr.Count() == 2 && eq.CalibrationTypeID == 3)
            {


                if (Option == 1 && pr[0] == 10 && pr[1] == 11)
                {
                    result = true;
                }
                if (Option == 2 && pr[0] == 12 && pr[1] == 13)
                {
                    result = true;
                }



            }


                return result;
        }

    }
}
