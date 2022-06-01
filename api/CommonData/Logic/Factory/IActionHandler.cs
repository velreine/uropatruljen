﻿using System.Threading.Tasks;
using CommonData.Model.Action;

namespace CommonData.Logic.Factory
{
    /**
     * An Action handler takes in an action and knows what to do with it.
     */
    public interface IActionHandler
    {
        Task HandleAsync(IAction action);
    }
}