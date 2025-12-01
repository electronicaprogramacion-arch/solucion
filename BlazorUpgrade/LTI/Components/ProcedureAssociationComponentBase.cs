using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Infraestructure.Blazor.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Components
{
    public class ProcedureAssociationComponentBase : ComponentBase
    {
        [Inject] protected IConfiguration Configuration { get; set; }
        [Inject] protected ILogger<ProcedureAssociationComponentBase> Logger { get; set; }
        [Inject] protected Func<dynamic, IBasicsServices<CallContext>> ProcedureServiceFactory { get; set; }

        [Parameter] public string PieceOfEquipmentID { get; set; }
        [Parameter] public bool Enabled { get; set; } = true;
        [Parameter] public EventCallback<List<ProcedureEquipment>> OnAssociationsChanged { get; set; }

        protected List<Procedure> AvailableProcedures { get; set; } = new List<Procedure>();
        protected List<ProcedureEquipment> AssociatedProcedures { get; set; } = new List<ProcedureEquipment>();
        protected List<ProcedureEquipment> RemovedAssociations { get; set; } = new List<ProcedureEquipment>();
        protected bool IsLoading { get; set; } = false;
        protected bool ShowProcedureSection { get; set; } = false;
        protected int SelectedProcedureId { get; set; } = 0;
        protected bool HasChanges { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            await CheckThermotempVisibility();
            
            if (ShowProcedureSection)
            {
                await LoadAvailableProcedures();
                await LoadAssociatedProcedures();
            }
        }

        protected async Task CheckThermotempVisibility()
        {
            try
            {
                // Always show procedure section for all customers
                ShowProcedureSection = true;

                //Logger.LogInformation("Procedure association section visibility: {Visible} (Always enabled)", ShowProcedureSection);
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error checking procedure section visibility");
                ShowProcedureSection = false;
            }
        }

        protected async Task LoadAvailableProcedures()
        {
            try
            {
                IsLoading = true;
                StateHasChanged();

                var procedureService = ProcedureServiceFactory(null);
                var callOptions = new CallOptions();
                
                // Get all active procedures
                var result = await procedureService.GetAllProcedures(new CallContext());
                
                if (result?.Procedures != null)
                {
                    AvailableProcedures = result.Procedures.ToList();
                    //Logger.LogInformation("Loaded {Count} available procedures", AvailableProcedures.Count);
                }
                else
                {
                    //Logger.LogWarning("No procedures returned from service");
                    AvailableProcedures = new List<Procedure>();
                }
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error loading available procedures");
                AvailableProcedures = new List<Procedure>();
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }

        protected async Task LoadAssociatedProcedures()
        {
            if (string.IsNullOrEmpty(PieceOfEquipmentID)) return;

            try
            {
                var procedureService = ProcedureServiceFactory(null);
                var callOptions = new CallOptions();
                
                // TODO: Implement GetProceduresByEquipment method in service
                // For now, initialize empty list
                AssociatedProcedures = new List<ProcedureEquipment>();
                //Logger.LogInformation("Initialized empty procedure associations for equipment {EquipmentId}", PieceOfEquipmentID);
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error loading associated procedures for equipment {EquipmentId}", PieceOfEquipmentID);
                AssociatedProcedures = new List<ProcedureEquipment>();
            }
        }

        protected async Task OnProcedureSelected(ChangeEventArgs e)
        {
            try
            {
                if (int.TryParse(e.Value?.ToString(), out int procedureId))
                {
                    SelectedProcedureId = procedureId;
                }
                else
                {
                    SelectedProcedureId = 0;
                }
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error handling procedure selection");
                SelectedProcedureId = 0;
            }
        }

        protected async Task AddProcedureAssociation()
        {
            try
            {
                if (SelectedProcedureId == 0 || string.IsNullOrEmpty(PieceOfEquipmentID))
                {
                    return;
                }

                // Check if already associated
                if (AssociatedProcedures.Any(ap => ap.ProcedureID == SelectedProcedureId))
                {
                    //Logger.LogWarning("Procedure {ProcedureId} is already associated with equipment {EquipmentId}",
                    //    SelectedProcedureId, PieceOfEquipmentID);
                    return;
                }

                // Find the procedure details
                var procedure = AvailableProcedures.FirstOrDefault(p => p.ProcedureID == SelectedProcedureId);
                if (procedure == null)
                {
                    //Logger.LogError("Procedure {ProcedureId} not found in available procedures", SelectedProcedureId);
                    return;
                }

                // Create new association
                var newAssociation = new ProcedureEquipment
                {
                    ProcedureID = SelectedProcedureId,
                    PieceOfEquipmentID = PieceOfEquipmentID,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System", // TODO: Get current user
                    Procedure = procedure,
                    IsNew = true
                };

                AssociatedProcedures.Add(newAssociation);
                SelectedProcedureId = 0;
                HasChanges = true;

                // Notify parent component
                if (OnAssociationsChanged.HasDelegate)
                {
                    await OnAssociationsChanged.InvokeAsync(GetAllAssociations());
                }

                //Logger.LogInformation("Added procedure association: Procedure {ProcedureId} to Equipment {EquipmentId}", 
                //                    procedure.ProcedureID, PieceOfEquipmentID);

                StateHasChanged();
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error adding procedure association");
            }
        }

        protected async Task RemoveProcedureAssociation(ProcedureEquipment association)
        {
            try
            {
                AssociatedProcedures.Remove(association);
                
                // If it's an existing association (not new), add to removed list
                if (!association.IsNew)
                {
                    RemovedAssociations.Add(association);
                }

                HasChanges = true;

                // Notify parent component
                if (OnAssociationsChanged.HasDelegate)
                {
                    await OnAssociationsChanged.InvokeAsync(GetAllAssociations());
                }

                //Logger.LogInformation("Removed procedure association: Procedure {ProcedureId} from Equipment {EquipmentId}", 
                //                    association.ProcedureID, PieceOfEquipmentID);

                StateHasChanged();
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error removing procedure association");
            }
        }

        public List<ProcedureEquipment> GetAllAssociations()
        {
            return AssociatedProcedures.ToList();
        }

        public List<ProcedureEquipment> GetRemovedAssociations()
        {
            return RemovedAssociations.ToList();
        }

        public async Task SaveAssociations()
        {
            try
            {
                var procedureService = ProcedureServiceFactory(null);
                var callOptions = new CallOptions();

                // TODO: Implement CreateProcedureEquipment and DeleteProcedureEquipment methods in service
                // For now, just log the operations
                foreach (var association in AssociatedProcedures.Where(a => a.IsNew))
                {
                    //Logger.LogInformation("Would create procedure equipment association: {ProcedureId} - {EquipmentId}",
                //                        association.ProcedureID, association.PieceOfEquipmentID);
                }

                foreach (var association in RemovedAssociations)
                {
                    //Logger.LogInformation("Would delete procedure equipment association: {ProcedureId} - {EquipmentId}",
                //                        association.ProcedureID, association.PieceOfEquipmentID);
                }

                // Clear change tracking
                foreach (var association in AssociatedProcedures)
                {
                    association.IsNew = false;
                }
                RemovedAssociations.Clear();
                HasChanges = false;

                //Logger.LogInformation("Saved procedure associations for equipment {EquipmentId}", PieceOfEquipmentID);
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error saving procedure associations for equipment {EquipmentId}", PieceOfEquipmentID);
                throw;
            }
        }

        public async Task RefreshAssociations()
        {
            await LoadAssociatedProcedures();
            HasChanges = false;
            RemovedAssociations.Clear();
            StateHasChanged();
        }
    }
}
