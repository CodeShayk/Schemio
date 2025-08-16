using System;
using System.Collections.Generic;

namespace Schemio.Core.Impl
{
    internal class EventPublisher
    {
        private readonly ISubscriber<ExecutorResultArgs> subscriber;

        public EventPublisher(ISubscriber<ExecutorResultArgs> subscriber)
        {
            this.subscriber = subscriber;
        }

        public void PublishEvent(IDataContext context, ExecutorResultArgs args) => subscriber.OnEventHandler(context, args);
    }

    internal class ExecutorResultArgs : EventArgs
    {
        public ExecutorResultArgs(IEnumerable<IQueryResult> result)
        {
            Result = result;
        }

        public IEnumerable<IQueryResult> Result { get; }
    }

    internal interface ISubscriber<ExecutorResultArgs>
    {
        void OnEventHandler(IDataContext context, ExecutorResultArgs e);
    }
}