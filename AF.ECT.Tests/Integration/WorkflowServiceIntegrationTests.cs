using System.Collections;
using System.Reflection;
using AF.ECT.Tests.Infrastructure;

namespace AF.ECT.Tests.Integration;

public class WorkflowServiceIntegrationTests : IntegrationTestBase
{
    [Theory]
    [ClassData(typeof(ListMethodTestData))]
    public async Task RpcReturnsEmptyList(string methodName, object request)
    {
        var channel = CreateGrpcChannel();
        var client = new WorkflowService.WorkflowServiceClient(channel);
        var method = typeof(WorkflowService.WorkflowServiceClient).GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => m.Name == methodName + "Async" && m.GetParameters().Length == 4)
            .First(m => m.GetParameters()[0].ParameterType == request.GetType())!;
        var call = method.Invoke(client, [request, Type.Missing, Type.Missing, CancellationToken.None])!;
        var task = (Task)call.GetType().GetProperty("ResponseAsync")!.GetValue(call)!;
        await task;
        var response = task.GetType().GetProperty("Result")!.GetValue(task)!;
        var items = (IEnumerable)response.GetType().GetProperty("Items")!.GetValue(response)!;
        Assert.NotNull(response);
        Assert.NotNull(items);
        Assert.Empty(items);
    }

    [Theory]
    [ClassData(typeof(IntMethodTestData))]
    public async Task RpcReturnsZero(string methodName, object request, string propertyName)
    {
        var channel = CreateGrpcChannel();
        var client = new WorkflowService.WorkflowServiceClient(channel);
        var method = typeof(WorkflowService.WorkflowServiceClient).GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => m.Name == methodName + "Async" && m.GetParameters().Length == 4)
            .First(m => m.GetParameters()[0].ParameterType == request.GetType())!;
        var call = method.Invoke(client, [request, Type.Missing, Type.Missing, CancellationToken.None])!;
        var task = (Task)call.GetType().GetProperty("ResponseAsync")!.GetValue(call)!;
        await task;
        var response = task.GetType().GetProperty("Result")!.GetValue(task)!;
        var result = (int)response.GetType().GetProperty(propertyName)!.GetValue(response)!;
        Assert.Equal(0, result);
    }

    public class ListMethodTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "GetUsersOnline", new EmptyRequest() };
            yield return new object[] { "SearchMemberData", new SearchMemberDataRequest { LastName = "John" } };
            yield return new object[] { "GetAllFindingByReasonOf", new EmptyRequest() };
            yield return new object[] { "GetAllLocks", new EmptyRequest() };
            yield return new object[] { "GetCancelReasons", new GetCancelReasonsRequest { WorkflowId = 1, IsFormal = true } };
            yield return new object[] { "GetWorkflowTitle", new GetWorkflowTitleRequest { ModuleId = 1, SubCase = 0 } };
            yield return new object[] { "GetPermissions", new GetPermissionsRequest { WorkflowId = 1 } };
            yield return new object[] { "GetReinvestigationRequests", new GetReinvestigationRequestsRequest { UserId = 1, Sarc = true } };
            yield return new object[] { "GetMailingListForLOD", new GetMailingListForLODRequest { RefId = 1, GroupId = 1, Status = 1, CallingService = "test" } };
            yield return new object[] { "GetManagedUsers", new GetManagedUsersRequest { Userid = 1, Ssn = "123456789", Name = "John Doe", Status = 1, Role = 1, SrchUnit = 1, ShowAllUsers = true } };
            yield return new object[] { "GetUserAltTitle", new GetUserAltTitleRequest { UserId = 1, GroupId = 1 } };
            yield return new object[] { "GetUserAltTitleByGroupCompo", new GetUserAltTitleByGroupCompoRequest { GroupId = 1, WorkCompo = 1 } };
            yield return new object[] { "GetUserName", new GetUserNameRequest { First = "John", Last = "Doe" } };
            yield return new object[] { "GetUsersAltTitleByGroup", new GetUsersAltTitleByGroupRequest { GroupId = 1 } };
            yield return new object[] { "GetWhois", new GetWhoisRequest { UserId = 1 } };
            yield return new object[] { "HasHQTechAccount", new HasHQTechAccountRequest { OriginUserId = 1, UserEdipin = "123456789" } };
            yield return new object[] { "IsFinalStatusCode", new IsFinalStatusCodeRequest { StatusId = 1 } };
            yield return new object[] { "SearchMemberDataTest", new SearchMemberDataTestRequest { UserId = 1, Ssn = "123456789", Name = "John Doe", SrchUnit = 1, RptView = 1 } };
            yield return new object[] { "UpdateLogin", new UpdateLoginRequest { UserId = 1, SessionId = "session123", RemoteAddr = "127.0.0.1" } };
            yield return new object[] { "AddSignature", new AddSignatureRequest { RefId = 1, ModuleType = 1, UserId = 1, ActionId = 1, GroupId = 1, StatusIn = 1, StatusOut = 1 } };
            yield return new object[] { "CopyWorkflow", new CopyWorkflowRequest { FromId = 1, ToId = 1 } };
            yield return new object[] { "GetActionsByStep", new GetActionsByStepRequest { StepId = 1 } };
            yield return new object[] { "GetActiveCases", new GetActiveCasesRequest { RefId = 1, GroupId = 1 } };
            yield return new object[] { "GetCreatableByGroup", new GetCreatableByGroupRequest { Compo = "test", Module = 1, GroupId = 1 } };
            yield return new object[] { "GetFindingByReasonOfById", new GetFindingByReasonOfByIdRequest { Id = 1 } };
            yield return new object[] { "GetFindings", new GetFindingsRequest { WorkflowId = 1, GroupId = 1 } };
            yield return new object[] { "GetModuleFromWorkflow", new GetModuleFromWorkflowRequest { WorkflowId = 1 } };
            yield return new object[] { "GetPageAccessByGroup", new GetPageAccessByGroupRequest { Workflow = 1, Status = 1, Group = 1 } };
            yield return new object[] { "GetPageAccessByWorkflowView", new GetPageAccessByWorkflowViewRequest { Compo = "test", Workflow = 1, Status = 1 } };
            yield return new object[] { "GetPagesByWorkflowId", new GetPagesByWorkflowIdRequest { WorkflowId = 1 } };
            yield return new object[] { "GetPermissionsByCompo", new GetPermissionsByCompoRequest { WorkflowId = 1, Compo = "test" } };
            yield return new object[] { "GetReturnReasons", new GetReturnReasonsRequest { WorkflowId = 1 } };
            yield return new object[] { "GetRwoaReasons", new GetRwoaReasonsRequest { WorkflowId = 1 } };
            yield return new object[] { "GetStatusCodesByCompo", new GetStatusCodesByCompoRequest { Compo = "test" } };
            yield return new object[] { "GetStatusCodesByCompoAndModule", new GetStatusCodesByCompoAndModuleRequest { Compo = "test", Module = 1 } };
            yield return new object[] { "GetStatusCodesBySignCode", new GetStatusCodesBySignCodeRequest { GroupId = 1, Module = 1 } };
            yield return new object[] { "GetStatusCodesByWorkflow", new GetStatusCodesByWorkflowRequest { WorkflowId = 1 } };
            yield return new object[] { "GetStatusCodesByWorkflowAndAccessScope", new GetStatusCodesByWorkflowAndAccessScopeRequest { WorkflowId = 1, AccessScope = 1 } };
            yield return new object[] { "GetStatusCodeScope", new GetStatusCodeScopeRequest { StatusId = 1 } };
            yield return new object[] { "GetStepsByWorkflow", new GetStepsByWorkflowRequest { Workflow = 1 } };
            yield return new object[] { "GetStepsByWorkflowAndStatus", new GetStepsByWorkflowAndStatusRequest { Workflow = 1, Status = 1, DeathStatus = "test" } };
            yield return new object[] { "GetViewableByGroup", new GetViewableByGroupRequest { GroupId = 1, Module = 1 } };
            yield return new object[] { "GetWorkflowByCompo", new GetWorkflowByCompoRequest { Compo = "test", UserId = 1 } };
            yield return new object[] { "GetWorkflowFromModule", new GetWorkflowFromModuleRequest { ModuleId = 1 } };
            yield return new object[] { "GetWorkflowInitialStatusCode", new GetWorkflowInitialStatusCodeRequest { Compo = 1, Module = 1, WorkflowId = 1 } };
            yield return new object[] { "GetWorkflowTitleByWorkStatusId", new GetWorkflowTitleByWorkStatusIdRequest { WorkflowId = 1, SubCase = 1 } };
            yield return new object[] { "InsertAction", new InsertActionRequest { Type = 1, StepId = 1, Target = 1, Data = 1 } };
            yield return new object[] { "InsertOptionAction", new InsertOptionActionRequest { Type = 1, Wsoid = 1, Target = 1, Data = 1 } };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class IntMethodTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "GetMembersUserId", new GetMembersUserIdRequest { MemberSsn = "123456789" }, "UserId" };
            yield return new object[] { "Logout", new LogoutRequest { UserId = 1 }, "Result" };
            yield return new object[] { "RegisterUser", new RegisterUserRequest { UserId = 1, WorkCompo = "test", ReceiveEmail = true, GroupId = 1, AccountStatus = 1, ExpirationDate = "2025-12-31" }, "Result" };
            yield return new object[] { "RegisterUserRole", new RegisterUserRoleRequest { UserId = 1, GroupId = 1, Status = 1 }, "UserRoleId" };
            yield return new object[] { "UpdateAccountStatus", new UpdateAccountStatusRequest { UserId = 1, AccountStatus = 1, ExpirationDate = "2025-12-31", Comment = "test" }, "Result" };
            yield return new object[] { "UpdateManagedSettings", new UpdateManagedSettingsRequest { UserId = 1, Compo = "test", RoleId = 1, GroupId = 1, Comment = "test", ReceiveEmail = true, ExpirationDate = "2025-12-31" }, "Result" };
            yield return new object[] { "UpdateUserAltTitle", new UpdateUserAltTitleRequest { UserId = 1, GroupId = 1, NewTitle = "test" }, "Result" };
            yield return new object[] { "CopyActions", new CopyActionsRequest { DestWsoid = 1, SrcWsoid = 1 }, "Result" };
            yield return new object[] { "CopyRules", new CopyRulesRequest { DestWsoid = 1, SrcWsoid = 1 }, "Result" };
            yield return new object[] { "DeleteStatusCode", new DeleteStatusCodeRequest { StatusId = 1 }, "Result" };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    // Removed SayHello test as it's not implemented in the service
}
