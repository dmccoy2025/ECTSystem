using AF.ECT.Data.Interfaces;
using AF.ECT.Data.ResultTypes;
using AF.ECT.Server;
using AF.ECT.Shared.Extensions;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AF.ECT.Tests.Infrastructure;

public class IntegrationTestBase : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseContentRoot(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        builder.ConfigureServices(services =>
        {
            // Remove the real data service
            var dataServiceDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IDataService));
            if (dataServiceDescriptor != null)
            {
                services.Remove(dataServiceDescriptor);
            }

            // Mock the data service to return empty results
            var mockDataService = new Mock<IDataService>();
            // Setup mock methods to return empty lists
            mockDataService.Setup(ds => ds.GetUsersOnlineAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_user_sp_GetUsersOnlineResult>()));
            mockDataService.Setup(ds => ds.SearchMemberDataAsync(It.IsAny<SearchMemberDataRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_user_sp_SearchMemberDataResult>()));
            mockDataService.Setup(ds => ds.GetAllFindingByReasonOfAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetAllFindingByReasonOfResult>()));
            mockDataService.Setup(ds => ds.GetAllLocksAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetAllLocksResult>()));
            mockDataService.Setup(ds => ds.GetCancelReasonsAsync(It.IsAny<byte?>(), It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetCancelReasonsResult>()));
            mockDataService.Setup(ds => ds.GetMembersUserIdAsync(It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0));
            mockDataService.Setup(ds => ds.GetWorkflowTitleAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetWorkflowTitleResult>()));
            mockDataService.Setup(ds => ds.GetPermissionsAsync(It.IsAny<byte?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_Workflow_sp_GetPermissionsResult>()));
            // Core User Methods
            mockDataService.Setup(ds => ds.GetReinvestigationRequestsAsync(It.IsAny<int?>(), It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_lod_sp_GetReinvestigationRequestsResult>()));
            mockDataService.Setup(ds => ds.GetMailingListForLODAsync(It.IsAny<GetMailingListForLODRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_user_sp_GetMailingListForLODResult>()));
            mockDataService.Setup(ds => ds.GetManagedUsersAsync(It.IsAny<GetManagedUsersRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_user_sp_GetManagedUsersResult>()));
            mockDataService.Setup(ds => ds.GetUserAltTitleAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_user_sp_GetUserAltTitleResult>()));
            mockDataService.Setup(ds => ds.GetUserAltTitleByGroupCompoAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_user_sp_GetUserAltTitleByGroupCompoResult>()));
            mockDataService.Setup(ds => ds.GetUserNameAsync(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_user_sp_GetUserNameResult>()));
            mockDataService.Setup(ds => ds.GetUsersAltTitleByGroupAsync(It.IsAny<int?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_user_sp_GetUsersAltTitleByGroupResult>()));
            mockDataService.Setup(ds => ds.GetWhoisAsync(It.IsAny<int?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_user_sp_GetWhoisResult>()));
            mockDataService.Setup(ds => ds.HasHQTechAccountAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_user_sp_HasHQTechAccountResult>()));
            mockDataService.Setup(ds => ds.IsFinalStatusCodeAsync(It.IsAny<byte?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_user_sp_IsFinalStatusCodeResult>()));
            mockDataService.Setup(ds => ds.LogoutAsync(It.IsAny<int?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0));
            mockDataService.Setup(ds => ds.RegisterUserAsync(It.IsAny<RegisterUserRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0));
            mockDataService.Setup(ds => ds.RegisterUserRoleAsync(It.IsAny<int?>(), It.IsAny<short?>(), It.IsAny<byte?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0));
            mockDataService.Setup(ds => ds.SearchMemberDataAsync(It.IsAny<SearchMemberDataRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_user_sp_SearchMemberDataResult>()));
            mockDataService.Setup(ds => ds.SearchMemberDataTestAsync(It.IsAny<SearchMemberDataTestRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_user_sp_SearchMemberData_TestResult>()));
            mockDataService.Setup(ds => ds.UpdateAccountStatusAsync(It.IsAny<UpdateAccountStatusRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0));
            mockDataService.Setup(ds => ds.UpdateLoginAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_user_sp_UpdateLoginResult>()));
            mockDataService.Setup(ds => ds.UpdateManagedSettingsAsync(It.IsAny<UpdateManagedSettingsRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0));
            mockDataService.Setup(ds => ds.UpdateUserAltTitleAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0));
            // Core Workflow Methods
            mockDataService.Setup(ds => ds.AddSignatureAsync(It.IsAny<AddSignatureRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_AddSignatureResult>()));
            mockDataService.Setup(ds => ds.CopyActionsAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0));
            mockDataService.Setup(ds => ds.CopyRulesAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0));
            mockDataService.Setup(ds => ds.CopyWorkflowAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_CopyWorkflowResult>()));
            mockDataService.Setup(ds => ds.DeleteStatusCodeAsync(It.IsAny<int?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(0));
            mockDataService.Setup(ds => ds.GetActionsByStepAsync(It.IsAny<int?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetActionsByStepResult>()));
            mockDataService.Setup(ds => ds.GetActiveCasesAsync(It.IsAny<int?>(), It.IsAny<short?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetActiveCasesResult>()));
            mockDataService.Setup(ds => ds.GetCreatableByGroupAsync(It.IsAny<string?>(), It.IsAny<byte?>(), It.IsAny<byte?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetCreatableByGroupResult>()));
            mockDataService.Setup(ds => ds.GetFindingByReasonOfByIdAsync(It.IsAny<int?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetFindingByReasonOfByIdResult>()));
            mockDataService.Setup(ds => ds.GetFindingsAsync(It.IsAny<byte?>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetFindingsResult>()));
            mockDataService.Setup(ds => ds.GetModuleFromWorkflowAsync(It.IsAny<int?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetModuleFromWorkflowResult>()));
            mockDataService.Setup(ds => ds.GetPageAccessByGroupAsync(It.IsAny<byte?>(), It.IsAny<int?>(), It.IsAny<byte?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetPageAccessByGroupResult>()));
            mockDataService.Setup(ds => ds.GetPageAccessByWorkflowViewAsync(It.IsAny<string?>(), It.IsAny<byte?>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetPageAccessByWorkflowViewResult>()));
            mockDataService.Setup(ds => ds.GetPagesByWorkflowIdAsync(It.IsAny<int?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetPagesByWorkflowIdResult>()));
            mockDataService.Setup(ds => ds.GetPermissionsByCompoAsync(It.IsAny<byte?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_Workflow_sp_GetPermissionsByCompoResult>()));
            mockDataService.Setup(ds => ds.GetReturnReasonsAsync(It.IsAny<byte?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetReturnReasonsResult>()));
            mockDataService.Setup(ds => ds.GetRwoaReasonsAsync(It.IsAny<byte?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetRwoaReasonsResult>()));
            mockDataService.Setup(ds => ds.GetStatusCodesByCompoAsync(It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetStatusCodesByCompoResult>()));
            mockDataService.Setup(ds => ds.GetStatusCodesByCompoAndModuleAsync(It.IsAny<string?>(), It.IsAny<byte?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetStatusCodesByCompoAndModuleResult>()));
            mockDataService.Setup(ds => ds.GetStatusCodesBySignCodeAsync(It.IsAny<short?>(), It.IsAny<byte?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetStatusCodesBySignCodeResult>()));
            mockDataService.Setup(ds => ds.GetStatusCodesByWorkflowAsync(It.IsAny<byte?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetStatusCodesByWorkflowResult>()));
            mockDataService.Setup(ds => ds.GetStatusCodesByWorkflowAndAccessScopeAsync(It.IsAny<byte?>(), It.IsAny<byte?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetStatusCodesByWorkflowAndAccessScopeResult>()));
            mockDataService.Setup(ds => ds.GetStatusCodeScopeAsync(It.IsAny<byte?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetStatusCodeScopeResult>()));
            mockDataService.Setup(ds => ds.GetStepsByWorkflowAsync(It.IsAny<byte?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetStepsByWorkflowResult>()));
            mockDataService.Setup(ds => ds.GetStepsByWorkflowAndStatusAsync(It.IsAny<byte?>(), It.IsAny<byte?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetStepsByWorkflowAndStatusResult>()));
            mockDataService.Setup(ds => ds.GetViewableByGroupAsync(It.IsAny<byte?>(), It.IsAny<byte?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetViewableByGroupResult>()));
            mockDataService.Setup(ds => ds.GetWorkflowByCompoAsync(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetWorkflowByCompoResult>()));
            mockDataService.Setup(ds => ds.GetWorkflowFromModuleAsync(It.IsAny<int?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetWorkflowFromModuleResult>()));
            mockDataService.Setup(ds => ds.GetWorkflowInitialStatusCodeAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetWorkflowInitialStatusCodeResult>()));
            mockDataService.Setup(ds => ds.GetWorkflowTitleByWorkStatusIdAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_GetWorkflowTitleByWorkStatusIdResult>()));
            mockDataService.Setup(ds => ds.InsertActionAsync(It.IsAny<InsertActionRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_InsertActionResult>()));
            mockDataService.Setup(ds => ds.InsertOptionActionAsync(It.IsAny<InsertOptionActionRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<core_workflow_sp_InsertOptionActionResult>()));
            // Add more setups as needed for other methods

            services.AddSingleton(mockDataService.Object);
        });
    }

    // Helper to create a gRPC channel to the test server
    protected GrpcChannel CreateGrpcChannel()
    {
        var client = base.CreateClient();
        return GrpcChannelFactory.CreateForTesting(client.BaseAddress!, client);
    }
}
