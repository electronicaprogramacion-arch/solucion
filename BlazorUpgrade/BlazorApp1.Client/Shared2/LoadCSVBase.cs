using CalibrationSaaS.Application.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using System.IO;
using Microsoft.AspNetCore.Components.Forms;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using System.Linq;
using CalibrationSaaS.Domain.Aggregates.Querys;
using Blazored.Modal.Services;

using FormatValidator;
using Helpers.Models;

namespace BlazorApp1.Blazor.Blazor.GenericMethods
{
    public class LoadCSVBase
        : Base_Create<WeightSet, IPieceOfEquipmentService<CallContext>, CalibrationSaaS.Domain.Aggregates.Shared.AppStateCompany>
        , IPage<WeightSet, IPieceOfEquipmentService<CallContext>, CalibrationSaaS.Domain.Aggregates.Shared.AppStateCompany>
    {


        [Parameter]
        public CalibrationSubType CalSubType { get; set; }

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

          const string DefaultStatus = "Drop a csv file here to view it, or click to choose a CSV file";
    const int MaxFileSize = 5 * 1024 * 1024; // 5MB
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

                        string config = CreateValidator(CalSubType);

                        CsvValidation csvValidation = CsvValidation.FromJson(config);

                         var validate = await ValidateCSV(stream, config);


                        if (validate)
                        {
                            var outputFileString = System.Text.Encoding.UTF8.GetString(stream.ToArray());


                            var lines = outputFileString.Split(Environment.NewLine);

                            List<string> lines2 = new List<string>();
                            int cont = 0;

                            if (csvValidation.HasHeaderRow)
                            {
                                cont = 1;
                            }
                            else
                            {

                            }

                            for(int yu=cont; yu < lines.Length; yu++)
                            {
                                if (lines[yu].Trim() != string.Empty)
                                {
                                    lines2.Add(lines[yu]);
                                }
                               
                            }

                            await SelectModal2(lines2);


                        }
                        else
                        {

                            throw new Exception(ErrorMessage);

                            //await SelectModal2(null);
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







    IFileListEntry file;

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


    bool Enabled = true;
    [Inject] CalibrationSaaS.Application.Services.IAssetsServices<CallContext> _assetsServices { get; set; }
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


    EditContext EditContext { get; set; }


        [Inject] Microsoft.Extensions.Configuration.IConfiguration Configuration { get; set; }
        public string url { get; set; }
        protected override async Task OnInitializedAsync()
    {


        await base.OnInitializedAsync();

        

        EditContext = new EditContext(eqWeightSet);

            string config = CreateValidator(CalSubType);

            CsvValidation csvValidation = CsvValidation.FromJson(config);

            url = @"/";//Configuration.GetSection("Reports")["URL"];

            if (!string.IsNullOrEmpty(csvValidation.ExampleUrl))
            {
                url = url + csvValidation.ExampleUrl;
            }
           


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

    async Task HandleFileSelected(IFileListEntry[] files)
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
                foreach (string line in lines)
                {
                    if (cont > 0)
                    {
                        csv.Add(line.Split(';')); // or, populate YourClass
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
                    PieceOfEquipmentGRPC poerpc = new PieceOfEquipmentGRPC(Client);

                    var result = poerpc.SaveWeights(lw);
                }


                catch (Exception ex)
                {

                    await ShowError(ex);

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



        public string CreateValidator(CalibrationSubType calsubtype)
        {


            if (calsubtype.CalibrationSubTypeView != null && !string.IsNullOrEmpty(calsubtype.CalibrationSubTypeView.CSVValidator))
            {
                return calsubtype.CalibrationSubTypeView.CSVValidator;
            }



            Dictionary<string, Column> columns = new Dictionary<string, Column>();

            Column c1= new Column();
            c1.Name= "ID";
            c1.Unique = true;
            c1.IsRequired = true;
            c1.IsNumeric = true;    
            Column c2 = new Column();
            c2.Name= "NAME";  
            c2.MaxLength = 100;
            Column c3 = new Column();
            c3.Name= "DOB";
            c3.Pattern = @"^\\d\\d\\d\\d-\\d\\d-\\d\\d$";
            Column c4 = new Column();
            c4.Name = "USERNAME";
            c4.IsRequired   = true;

            columns.Add("1",c1); 
            columns.Add("2", c2); 
            columns.Add("3", c3);
            columns.Add("4", c4);

            CsvValidation validatorobj = new CsvValidation() { RowSeperator = "\r\n", ColumnSeperator=",", HasHeaderRow=true, Columns= columns };


            var str= validatorobj.ToJson();

               // RowSeperator = "\r\n",
               //ColumnSeperator = ",";




            return str;

        }


        public async Task<bool> ValidateCSV(Stream json, string config)
        {
            List<RowValidationError> errors = new List<RowValidationError>();



            DateTime start = DateTime.Now;
            //Validator validator = Validator.FromJson(System.IO.File.ReadAllText(config));
            //FileSourceReader source = new FileSourceReader(json);

            Validator validator = Validator.FromJson(config);

            FileSourceReader source = new FileSourceReader(json);

            foreach (RowValidationError current in validator.Validate(source))
            {
                errors.Add(current);

            }
            ErrorMessage = string.Join(", ", errors);
            if (errors.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public async Task SelectModal2(List<string> result)
        {

          
                await BlazoredModal.CloseAsync(ModalResult.Ok(result));

           
        }



        private string[] SplitCSV(string input)
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
