﻿using Fluxor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalibrifyApp.Server.Store.State
{
    public class TodoDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public bool Completed { get; set; }
        public int UserId { get; set; }
    }

    public class RootState
    {
        public RootState(bool isLoading, string? currentErrorMessage, string url) =>
            (IsLoading, CurrentErrorMessage, Url) = (isLoading, currentErrorMessage, url);

        public bool IsLoading { get; set; }
        public string? CurrentErrorMessage { get; set; }
        public string? Url { get; set; }
        public bool HasCurrentErrors => !string.IsNullOrWhiteSpace(CurrentErrorMessage);
    }

    public class TodosStateDic
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }

    public class TodosState : RootState
    {
        public List<TodosStateDic> _dicState { get; set; } = new List<TodosStateDic>();

        public TodosState(bool isLoading, string? currentErrorMessage, string url, dynamic currentTodos, TodoDto currentTodo, object pagination)
            : base(isLoading, currentErrorMessage, url)
        {
            CurrentTodos = currentTodos;
            CurrentTodo = currentTodo;
            CurrentPagination = pagination;
        }

        public dynamic CurrentTodos { get; set; }
        public TodoDto CurrentTodo { get; set; }
        public object CurrentPagination { get; set; }
        public string CurrentType { get; set; } = "test";

        public string AddPagination(object pagination)
        {
            return string.Empty;
        }
    }

    public class TodosFeature : Feature<TodosState>
    {
        public override string GetName() => "Todos";

        protected override TodosState GetInitialState() =>
            new TodosState(false, null, null, null, null, null);
    }
}
