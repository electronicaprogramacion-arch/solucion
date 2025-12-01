using Blazed.Controls;
using BlazorInputFile;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.AssetsBasics
{
    public class Weight_CreateCSVBase
        : Base_Create<WeightSet, Func<dynamic, Application.Services.IPieceOfEquipmentService<CallContext>>, Domain.Aggregates.Shared.AppStateCompany>
        , IPage<WeightSet, IPieceOfEquipmentService<CallContext>, Domain.Aggregates.Shared.AppStateCompany>
    {


        public ResponsiveTable<WeightSet> Grid { get; set; }
        public Search<WeightSet, IPieceOfEquipmentService<CallContext>, AppStateCompany> searchComponent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public Task Delete(WeightSet DTO)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WeightSet> FilterList(string filter)
        {
            throw new NotImplementedException();
        }

        public Task SelectModal(WeightSet DTO)
        {
            throw new NotImplementedException();
        }


        public const string DefaultStatus = "Drop a csv file here to view it, or click to choose a CSV file";
        public const int MaxFileSize = 5 * 1024 * 1024; // 5MB
        public string status = DefaultStatus;

        public string fileName;
        public string fileTextContents;

       public async Task ViewFile(IFileListEntry[] files)
        {
            var file = files.FirstOrDefault();
            if (file == null)
            {
                return;
            }
            else if (file.Size > MaxFileSize)
            {
                status = $"That's too big. Max size: {MaxFileSize} bytes.";
            }
            else
            {
                status = "Loading...";

                //using (var reader = new StreamReader(file.Data))
                //{
                //    fileTextContents = await reader.ReadToEndAsync();
                //    fileName = file.Name;
                //}
                //SpinnerModal.ShowSpinner();
                var singleFile = file;

                Regex regex = new Regex(".+\\.csv", RegexOptions.Compiled);
                if (!regex.IsMatch(singleFile.Name))
                {
                    //show error invalidad format file
                }
                else
                {

                    try
                    {

                        List<WeightSet> lw = new List<WeightSet>();
                        var stream = new MemoryStream();
                        await file.Data.CopyToAsync(stream);



                        var outputFileString = System.Text.Encoding.UTF8.GetString(stream.ToArray());

                        var lines = outputFileString.Split(Environment.NewLine);


                        List<string[]> csv = new List<string[]>();
                        int cont = 0;
                        char sep = ';';

                        bool ini2 = false;
                        foreach (string line in lines)
                        {
                            if (cont > 0)
                            {

                                if (!ini2)
                                {
                                    if (line.Split(sep).Count() <= 5)
                                    {
                                        sep = ',';
                                    }
                                    if (line.Split(sep).Count() <= 5)
                                    {
                                        throw new Exception("Separator Error");
                                    }
                                    ini2 = true;
                                }

                                csv.Add(line.Split(sep)); // or, populate YourClass
                            }

                            cont = cont + 1;
                        }


                        //string json = System.Text.Json.JsonSerializer.Serialize(csv);

                        //var obj = System.Text.Json.JsonSerializer.Deserialize<List<dynamic>>(json);

                        List<string> error = new List<string>();

                        foreach (var ii in csv)
                        {
                            WeightSet w = new WeightSet();
                            //var a = ii[0];
                            //var b= ii.WeightID;
                            try
                            {
                                if (ii != null && ii.Count() > 5 && ii[0] != null)
                                {
                                    w.Reference = ii[0];
                                    w.CalibrationUncertValue = Convert.ToDouble(ii[5]);

                                    if (ii[9].Trim().ToUpper() == "RECTANGULAR")
                                        w.Distribution = "1";
                                    else if (ii[9].Trim().ToUpper() == "NORMAL")
                                        w.Distribution = "2";
                                    else if (ii[9].Trim().ToUpper() == "EXPANDED")
                                        w.Distribution = "3";
                                    else if (ii[9].Trim().ToUpper() == "RESOLUTION")
                                        w.Distribution = "4";

                                    if (string.IsNullOrEmpty(ii[7]))
                                    {
                                        w.Divisor = 1;
                                    }
                                    else
                                    {
                                        w.Divisor = Convert.ToDouble(ii[7]);
                                    }
                                    w.Serial = ii[1];

                                    if (ii[8].Trim().ToUpper() == "A")
                                        w.Type = "1";
                                    else if (ii[8].Trim().ToUpper() == "B")
                                        w.Type = "2";
                            

                                    var uom1 = ii[4].GetUoMFromAbb(AppState.UnitofMeasureList);
                                    if (uom1 != null)
                                    {
                                        w.UnitOfMeasureID = uom1.UnitOfMeasureID;
                                    }
                                    else
                                    {
                                        throw new Exception("Unit of measure not found");
                                    }

                                    var uom2 = ii[6].GetUoMFromAbb(AppState.UnitofMeasureList);
                                    if (uom2 != null)
                                    {
                                        w.UncertaintyUnitOfMeasureId = uom2.UnitOfMeasureID;
                                    }
                                    else
                                    {
                                        throw new Exception("Uncertainty Unit of measure not found");
                                    }

                                    //w.UnitOfMeasureID = ii[4].GetUoMFromAbb(AppState.UnitofMeasureList).UnitOfMeasureID;
                                    //w.UncertaintyUnitOfMeasureId = ii[6].GetUoMFromAbb(AppState.UnitofMeasureList).UnitOfMeasureID;

                                    double result11 = 0;
                                    if (double.TryParse(ii[3], out result11))
                                    {
                                        w.WeightActualValue = result11;
                                    }
                                    else
                                    {
                                        throw new Exception("Column Actual Value error");
                                    }

                                    double result10 = 0;
                                    if (double.TryParse(ii[2], out result10))
                                    {
                                        w.WeightNominalValue = result10;
                                    }
                                    else
                                    {
                                        throw new Exception("Column Nominal Value error");
                                    }


                                    //= Convert.ToInt32(ii[2]);
                                    w.Note = ii[10];
                                    lw.Add(w);
                                }
                            }
                            catch (Exception ex)
                            {
                                error.Add(ex.Message);
                            }
                        }
                        if (error.Count > 0)
                        {
                            throw new Exception(string.Join("|", error));
                        }




                        PieceOfEquipmentGRPC poerpc = new PieceOfEquipmentGRPC(Client,DbFactory);

                        var result = await poerpc.SaveWeights(lw);

                        LIST1 = result.List;

                        if (!string.IsNullOrEmpty(result.Message))
                        {
                            throw new Exception(result.Message);
                        }

                    }
                    catch (Exception ex)
                    {
                        await ExceptionManager(ex);

                    }
                }

                status = DefaultStatus;
            }
        }







        public IFileListEntry file;

        [Parameter]
        public PieceOfEquipment POE { get; set; }


        public WeightSet DefaultItem()
        {
            WeightSet w = new WeightSet();

            w.UnitOfMeasureID = POE.UnitOfMeasureID.Value;
            w.UncertaintyUnitOfMeasureId = POE.UnitOfMeasureID.Value;

            return w;

        }

        public WeightSet DefaultItemAcr()
        {
            WeightSet w = new WeightSet();

            w.UnitOfMeasureID = POE.UnitOfMeasureID.Value;

            return w;

        }


        public bool Enabled = true;
        [Inject] Application.Services.IAssetsServices<CallContext> _assetsServices { get; set; }
        public WeightSet eqWeightSet = new WeightSet();


        public bool IsForAccredited { get; set; } = false;
        public List<WeightSet> LIST1 { get; set; }

        [Inject] public AppState AppState { get; set; }


        public ResponsiveTable<WeightSet> RT { get; set; } = new ResponsiveTable<WeightSet>();

        public void NewItem()
        {

        }

        public void Show(List<WeightSet> group, bool IsAccredited)
        {

            LIST1 = group;
            //RT.ItemsDataSource = LIST1;
            IsForAccredited = IsAccredited;
            //Logger.LogDebug("listWeightSets on Show Wc" + LIST1.Count());
            //Logger.LogDebug("IsAccredited on Show Wc" + IsAccredited);
            StateHasChanged();
            //Console.WriteLine(RT.ItemsDataSource.Count());
        }


        public EditContext EditContext { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

           

            EditContext = new EditContext(eqWeightSet);


        }

        protected override async Task OnParametersSetAsync()

        {

        }

        public async Task OnInputFileChange(object e)
        {
            //SpinnerModal.ShowSpinner();
            //    var singleFile = ""; // e.File;

            //        Regex regex = new Regex(".+\\.csv", RegexOptions.Compiled);
            //    if (!regex.IsMatch(singleFile.Name))
            //    {
            //        //show error invalidad format file
            //    }
            //    else
            //    {
            //        var stream = singleFile.OpenReadStream();
            //        var csv = new List<string[]>();
            //        MemoryStream ms = new MemoryStream();
            //        await stream.CopyToAsync(ms);
            //        stream.Close();
            //        var outputFileString = System.Text.Encoding.UTF8.GetString(ms.ToArray());

            //        foreach (var item in outputFileString.Split(Environment.NewLine))
            //        {
            //            csv.Add(SplitCSV(item.ToString()));
            //        }

            //}
        }

        public async Task HandleFileSelected(IFileListEntry[] files)
        {
            file = files.FirstOrDefault();
            if (file != null)
            {
                //SpinnerModal.ShowSpinner();
                var singleFile = file;

                Regex regex = new Regex(".+\\.csv", RegexOptions.Compiled);
                if (!regex.IsMatch(singleFile.Name))
                {
                    //show error invalidad format file
                }
                else
                {
                    List<WeightSet> lw = new List<WeightSet>();
                    var stream = new MemoryStream();
                    await file.Data.CopyToAsync(stream);

                    //var stream = await singleFile.ReadAllAsync();
                    //MemoryStream ms = new MemoryStream();
                    //await stream.CopyToAsync(ms);
                    //stream.Close();

                    //var csv = new List<string[]>();

                    var outputFileString = System.Text.Encoding.UTF8.GetString(stream.ToArray());

                    var lines = outputFileString.Split(Environment.NewLine);


                    List<string[]> csv = new List<string[]>();
                    int cont = 0;

                    char sep = ';';

                    bool ini2 = false;

                    foreach (string line in lines)
                    {
                        if (cont > 0)
                        {
                            if(!ini2)
                            {
                                if (line.Split(sep).Count() <= 5)
                                {
                                    sep = ',';
                                }
                                if (line.Split(sep).Count() <= 5)
                                {
                                    throw new Exception("Separator Error");
                                }
                                ini2 = true;
                            }
                           

                            csv.Add(line.Split(sep)); // or, populate YourClass
                        
                        }

                        cont = cont + 1;
                    }


                    string json = System.Text.Json.JsonSerializer.Serialize(csv);

                    var obj = System.Text.Json.JsonSerializer.Deserialize<List<dynamic>>(json);

                    List<string> error = new List<string>();

                    foreach (var ii in csv)
                    {
                        WeightSet w = new WeightSet();
                        //var a = ii[0];
                        //var b= ii.WeightID;
                        try
                        {
                            if (ii != null && ii.Count() > 5 && ii[0] != null)
                            {
                                w.Reference = ii[0];
                                w.CalibrationUncertValue = Convert.ToDouble(ii[5]);
                                w.Distribution = ii[9];
                                w.Divisor = Convert.ToDouble(ii[7]);
                                w.Serial = ii[1];
                                w.Type = ii[8];
                                w.UnitOfMeasureID = 1;
                                w.WeightActualValue = Convert.ToDouble(ii[3]);
                                w.WeightNominalValue = Convert.ToInt32(ii[2]);

                                lw.Add(w);
                            }
                        }
                        catch (Exception ex)
                        {
                            error.Add(ex.Message);
                        }
                    }
                    if (error.Count > 0)
                    {
                        throw new Exception("CSV file is corrupt");
                    }


                    //var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
                    //{
                    //    HasHeaderRecord = false,
                    //    Delimiter = ","
                    //};

                    //using (var reader = new StreamReader(stream))
                    ////var reader = stream;


                    //using (var csv1 = new CsvReader(reader, config))
                    //{


                    //    var records = csv1.GetRecords<dynamic>();

                    //    var s = records.ToList();

                    //    foreach (var ii in s)
                    //    {
                    //        WeightSet w = new WeightSet();

                    //        lw.Add(w);
                    //    }
                    //}


                    try
                    {
                        PieceOfEquipmentGRPC poerpc = new PieceOfEquipmentGRPC(Client,DbFactory);

                        var result = poerpc.SaveWeights(lw);
                    }


                    catch (Exception ex)
                    {

                        await ShowError(ex.Message);

                        foreach (var ee in error)
                        {
                            await ShowError(ee);
                        }


                    }
                    //foreach (var item in outputFileString.Split(Environment.NewLine))
                    //{
                    //    csv.Add(SplitCSV(item.ToString()));
                    //}



                }

            }
        }

        public string[] SplitCSV(string input)
        {
            //Excludes commas within quotes
            Regex csvSplit = new Regex("(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)", RegexOptions.Compiled);
            List<string> list = new List<string>();
            string curr = null;
            foreach (Match match in csvSplit.Matches(input))
            {
                curr = match.Value;
                if (0 == curr.Length) list.Add("");

                list.Add(curr.TrimStart(','));
            }

            return list.ToArray();
        }

    }
}
