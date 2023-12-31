﻿using Maui.Ideas.App.Abstractions;
using Maui.Ideas.App.Models;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace Maui.Ideas.App.Presentation.ViewModels.Pages;

public abstract class BaseViewModel : ObservableObject, IQueryAttributable
{
    #region Fields

    private bool isBusy;

    private readonly ILazyDependency<INavigationService> _navigationService;

    private readonly ILazyDependency<ILoaderService> _loaderService;

    #endregion

    #region Properties

    public bool IsBusy
    {
        get => isBusy;
        set => SetProperty(ref isBusy, value, onChanged: OnIsBusyChanged);
    }

    protected INavigationService NavigationService { get => _navigationService.Value; }

    protected ILoaderService LoaderService { get => _loaderService.Value; }

    protected ILogger Logger { get; }

    public IMainThreadService MainThreadService { get; }

    #endregion

    #region Constructors

    protected BaseViewModel(
        ILazyDependency<ILoaderService> loaderService,
        ILazyDependency<INavigationService> navigationService,
        IMainThreadService mainThreadService,
        ILogger logger) : base(mainThreadService)
    {
        _navigationService = navigationService;
        _loaderService = loaderService;
        Logger = logger;
        MainThreadService = mainThreadService;
    }

    #endregion

    #region Protected Methods

    protected void ExecuteBusyAction(
        Action theBusyAction,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filePath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            theBusyAction?.Invoke();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Busy action process error File: {filePath} | Line: {lineNumber} | Method: {memberName}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    protected async Task ExecuteBusyActionAsync(
        Func<Task> theBusyAction,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filePath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            await theBusyAction?.Invoke();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Busy action process error File: {filePath} | Line: {lineNumber} | Method: {memberName}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    protected Task ExecuteBusyActionOnNewTaskAsync(
        Func<Task> theBusyAction,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filePath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        return ExecuteBusyActionAsync(
            () => Task.Run(async () => await theBusyAction?.Invoke()),
            memberName,
            filePath,
            lineNumber);
    }

    /// <summary>
    /// This method will fire and forget the action on new Task
    /// </summary>
    /// <param name="theBusyAction"></param>
    /// <param name="memberName"></param>
    /// <param name="filePath"></param>
    /// <param name="lineNumber"></param>
    protected void ExecuteBusyActionOnNewTask(
        Action theBusyAction,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filePath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        _ = Task.Run(() => ExecuteBusyAction(theBusyAction, memberName, filePath, lineNumber));
    }

    private void OnIsBusyChanged()
    {
        if (IsBusy)
            LoaderService.ShowLoading();
        else
            LoaderService.HideLoading();
    }

    #endregion

    #region IQueryAttributable

    public virtual void ApplyQueryAttributes(IDictionary<string, object> query) { }

    #endregion
}
