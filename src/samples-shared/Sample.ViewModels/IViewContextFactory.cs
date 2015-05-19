using System;

namespace Sample.ViewModels
{
	public interface IViewContextFactory<TContext> where TContext : IDisposable
	{
		TContext Create();
	}
}
