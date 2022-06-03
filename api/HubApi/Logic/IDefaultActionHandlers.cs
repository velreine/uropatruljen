using CommonData.Model.Action;

namespace HubApi.Logic;

/// <summary>
/// 
/// </summary>
public interface IDefaultActionHandlers
{
    Task AssignToHandler(IAction action);
}